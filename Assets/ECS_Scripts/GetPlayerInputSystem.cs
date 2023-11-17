using System.Collections;
using System.Collections.Generic;
using ECS_Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class GetPlayerInputSystem : SystemBase
{
    private SomethingControls controls;

    private Entity _playerEntity;
    private bool state;

    protected override void OnCreate()
    {
        controls = new SomethingControls();
        RequireForUpdate<PlayerMovementInput>();
        RequireForUpdate<PlayerShootInput>();
        //EntityManager.AddComponent<PlayerInput>(SystemHandle);
    }

    private void StartShooting(InputAction.CallbackContext _)
    {
        state = !state;
        
       SystemAPI.SetSingleton(new PlayerShootInput{IsShooting = state});
    }

    protected override void OnStartRunning()
    {
        controls.Enable();
        controls.Cringe.Shoot.performed += StartShooting;
        
    }
    protected override void OnUpdate()
    {
        //System updates all ents every frame...?
        float curMoveInput = controls.Cringe.Move.ReadValue<Vector2>().x;
        SystemAPI.SetSingleton(new PlayerMovementInput{RotationDirection = curMoveInput});
        
    }

    protected override void OnStopRunning()
    {
        controls.Disable();
        controls.Cringe.Shoot.performed -= StartShooting;
    }

}

public struct PlayerMovementInput : IComponentData
{
    public float RotationDirection;
}

public struct PlayerShootInput : IComponentData
{
    public bool IsShooting;
}
