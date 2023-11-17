using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS_Scripts
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct TurretRotationSystem : ISystem
    {
        //I added a comment
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerMovementInput>();
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
           // //SystemAPI.QueryBuilder().WithAll<>()
           // var x = new RotateTurret()
           // {
           //     
           // };

            //var rot = SystemAPI.Time.DeltaTime * SystemAPI.GetSingleton<PlayerMovementInput>().RotationDirection;

            foreach (var (localToWorldLookup,x) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMovementInput>>())
            {

                localToWorldLookup.ValueRW = localToWorldLookup.ValueRW.RotateY(SystemAPI.Time.DeltaTime * SystemAPI.GetSingleton<PlayerMovementInput>().RotationDirection);
                CameraSingleton.instance.localRotation =  localToWorldLookup.ValueRO.Rotation;
            }

            //transform = transform.RotateY(DeltaTime * input.RotationDirection);
            //CameraSingleton.instance.rotation = transform.Rotation;

           // x.Schedule();
        }
    }
    /*
    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct RotateTurret : IJobEntity
    {
        public float DeltaTime;
        [BurstCompile]
        private void Execute(ref LocalTransform transform, in PlayerMovementInput input)
        {
            transform = transform.RotateY(DeltaTime * input.RotationDirection);
            CameraSingleton.instance.rotation = transform.Rotation;
        }
    }*/
}
