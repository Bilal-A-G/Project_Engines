using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    float totalTime = 0.0f;
    int frameCount = 0;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate average FPS
        totalTime += Time.unscaledDeltaTime;
        frameCount++;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        float averageFPS = frameCount / totalTime;
        string text = $"{msec:0.0} ms ({fps:0.} fps) | Avg FPS: {averageFPS:0.}";
        GUI.Label(rect, text, style);
    }
}