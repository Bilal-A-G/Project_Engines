using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ProceduralGrassRenderer : MonoBehaviour
{
    [Serializable]
    public class GrassSettings
    {
        [Header("General")]
        public float grassHeight = 0.5f;
        public int maxLayers = 16;
        public float uvScale = 1;
        [Header("Tiling")]
        public bool useWorldPositionAsUV;
        public float worldPositionUVScale;
        [Header("LOD")] 
        public float lodMinCameraDistance = 1;
        public float lodMaxCameraDistance = 1;
        [Min(0.05f)] public float lodFactor = 2;
    }
    
    
    [SerializeField] private ComputeShader grassComputeShader;
    [SerializeField] private ComputeShader triToVertComputeShader;
    [SerializeField] private Mesh sourceMesh;
    [SerializeField] private Material material;
    [SerializeField] private GrassSettings grassSettings;

    [StructLayout(LayoutKind.Sequential)]
    private struct SourceVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
    }

    private bool initialized;

    private ComputeBuffer sourceVertBuffer;
    private ComputeBuffer sourceTriBuffer;
    private ComputeBuffer drawBuffer;
    private ComputeBuffer argsBuffer;

    private int idGrassKernal;
    private int idTriToVertKernal;
    private int dispatchSize;

    private Bounds localBounds;
    private static readonly int SourceVertices = Shader.PropertyToID("_SourceVertices");
    private static readonly int SourceTriangles = Shader.PropertyToID("_SourceTriangles");
    private static readonly int DrawTriangles = Shader.PropertyToID("_DrawTriangles");
    private static readonly int NumSourceTriangles = Shader.PropertyToID("_NumSourceTriangles");
    private static readonly int IndirectArgsBuffer = Shader.PropertyToID("_IndirectArgsBuffer");
    private static readonly int LocalToWorld = Shader.PropertyToID("_LocalToWorld");
    private static readonly int TotalHeight = Shader.PropertyToID("_TotalHeight");
    private static readonly int MaxLayers = Shader.PropertyToID("_MaxLayers");

    private const int SOURCE_VERT_STRIDE = sizeof(float) * (3 + 3 + 2);
    private const int SOURCE_TRI_STRIDE = sizeof(int);
    private const int DRAW_STRIDE = sizeof(float) * (2 + (3 + 3 + 2) * 3);
    private const int ARGS_STRIDE = sizeof(int) * 4;

    private ComputeShader instansiatedGrassComputeShader;
    private ComputeShader instansiatedTriToVertComputeShader;
    private Material instansiatedMaterial;
    private static readonly int WorldPositionToUVScale = Shader.PropertyToID("_WorldPositionToUVScale");
    private static readonly int CameraDistanceMin = Shader.PropertyToID("_CameraDistanceMin");
    private static readonly int CameraDistanceMax = Shader.PropertyToID("_CameraDistanceMax");
    private static readonly int CameraDistanceFactor = Shader.PropertyToID("_CameraDistanceFactor");
    private Camera _camera;
    private static readonly int CameraPosition = Shader.PropertyToID("_CameraPosition");
    private static readonly int UvScale = Shader.PropertyToID("_UvScale");

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        if(initialized) OnDisable();
        initialized = true;
        
        #if UNITY_EDITOR
        _camera = Camera.main;
        #endif

        instansiatedGrassComputeShader = Instantiate(grassComputeShader);
        instansiatedTriToVertComputeShader = Instantiate(triToVertComputeShader);
        instansiatedMaterial = Instantiate(material);

        Vector3[] positions = sourceMesh.vertices;
        Vector3[] normals = sourceMesh.normals;
        Vector2[] uvs = sourceMesh.uv;

        int[] tris = sourceMesh.triangles;

        SourceVertex[] vertices = new SourceVertex[positions.Length];
        for (int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = new SourceVertex()
            {
                position = positions[i],
                normal = normals[i],
                uv = uvs[i]
            };
        }
        int numTriangles = tris.Length / 3;

        sourceVertBuffer = new ComputeBuffer(vertices.Length, SOURCE_VERT_STRIDE, ComputeBufferType.Structured, ComputeBufferMode.Immutable);
        sourceVertBuffer.SetData(vertices);
        
        sourceTriBuffer = new ComputeBuffer(tris.Length, SOURCE_TRI_STRIDE, ComputeBufferType.Structured, ComputeBufferMode.Immutable);
        sourceTriBuffer.SetData(tris);

        drawBuffer = new ComputeBuffer(numTriangles * grassSettings.maxLayers, DRAW_STRIDE, ComputeBufferType.Append);
        drawBuffer.SetCounterValue(0);

        argsBuffer = new ComputeBuffer(1, ARGS_STRIDE, ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(new [] {0,1,0,0});

        idGrassKernal = instansiatedGrassComputeShader.FindKernel("Main");
        idTriToVertKernal = instansiatedTriToVertComputeShader.FindKernel("Main");
        
        instansiatedGrassComputeShader.SetBuffer(idGrassKernal, SourceVertices, sourceVertBuffer);
        instansiatedGrassComputeShader.SetBuffer(idGrassKernal, SourceTriangles, sourceTriBuffer);
        instansiatedGrassComputeShader.SetBuffer(idGrassKernal, DrawTriangles, drawBuffer);
        instansiatedGrassComputeShader.SetInt(NumSourceTriangles, numTriangles);
        instansiatedGrassComputeShader.SetInt(MaxLayers, grassSettings.maxLayers);
        
        instansiatedGrassComputeShader.SetFloat(TotalHeight, grassSettings.grassHeight);
        instansiatedGrassComputeShader.SetFloat(WorldPositionToUVScale, grassSettings.worldPositionUVScale);
        instansiatedGrassComputeShader.SetFloat(UvScale, grassSettings.uvScale);
        
        instansiatedGrassComputeShader.SetFloat(CameraDistanceMin, grassSettings.lodMinCameraDistance);
        instansiatedGrassComputeShader.SetFloat(CameraDistanceMax, grassSettings.lodMaxCameraDistance);
        instansiatedGrassComputeShader.SetFloat(CameraDistanceFactor, grassSettings.lodFactor);
        
        if(grassSettings.useWorldPositionAsUV) instansiatedGrassComputeShader.EnableKeyword("USE_WORLD_POSITION_AS_UV");
   
        
        
        
        instansiatedTriToVertComputeShader.SetBuffer(idTriToVertKernal, IndirectArgsBuffer, argsBuffer);
        
        instansiatedMaterial.SetBuffer(DrawTriangles, drawBuffer);
        
        instansiatedGrassComputeShader.GetKernelThreadGroupSizes(idGrassKernal, out uint threadGroupSize, out _, out _);
        dispatchSize = Mathf.CeilToInt((float)numTriangles / threadGroupSize);

        localBounds = sourceMesh.bounds;
        localBounds.Expand(grassSettings.grassHeight);
        localBounds.Expand(transform.localScale);
    }

    private void OnDisable()
    {
        bool prv = initialized;
        initialized = false;

        if (!prv) return;
        
     sourceVertBuffer.Release();
     sourceTriBuffer.Release();
     drawBuffer.Release();
     argsBuffer.Release();
     #if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Destroy(instansiatedGrassComputeShader);
            Destroy(instansiatedTriToVertComputeShader);
            Destroy(instansiatedMaterial);
        }
        else
        {
            DestroyImmediate(instansiatedGrassComputeShader);
            DestroyImmediate(instansiatedTriToVertComputeShader);
            DestroyImmediate(instansiatedMaterial);
        }
     #else
            Destroy(instansiatedGrassComputeShader);
            Destroy(instansiatedTriToVertComputeShader);
            Destroy(instansiatedMaterial);
     #endif
         
        
        
    }

    private Bounds TransformBounds(Bounds boundOS)
    {
        Vector3 center = transform.TransformPoint(boundOS.center);

        Vector3 extents = boundOS.extents;
        Vector3 axisX = transform.TransformVector(extents.x, 0, 0);
        Vector3 axisY = transform.TransformVector(0,extents.y, 0);
        Vector3 axisZ = transform.TransformVector(0,0,extents.z);

        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }
    
    private void LateUpdate()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            OnDisable();
            OnEnable();
        }
        #endif
        
        drawBuffer.SetCounterValue(0);

        Bounds bounds = TransformBounds(localBounds);

        instansiatedGrassComputeShader.SetMatrix(LocalToWorld, transform.localToWorldMatrix);
        instansiatedGrassComputeShader.SetVector(CameraPosition, _camera!.transform.position);
        
        instansiatedGrassComputeShader.Dispatch(idGrassKernal, dispatchSize, 1,1);
        
        ComputeBuffer.CopyCount(drawBuffer, argsBuffer, 0);
        
        instansiatedTriToVertComputeShader.Dispatch(idTriToVertKernal, 1,1,1);
        
        Graphics.DrawProceduralIndirect(instansiatedMaterial, bounds, MeshTopology.Triangles, argsBuffer, 0, null,
            null, ShadowCastingMode.Off, true, gameObject.layer);
        
    }
}
