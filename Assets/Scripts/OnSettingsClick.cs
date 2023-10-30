using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSettingsClick : MonoBehaviour
{
    [SerializeField] private EventObject deactivateTurret;
    [SerializeField] private EventObject activateTurret;

    [SerializeField] private StateLayerObject fsm;

    [SerializeField] private CachedObjectWrapper cachedObjects;

    public void OnEnter()
    {
        fsm.UpdateState(deactivateTurret, null, cachedObjects);
    }

    public void OnExit()
    {
        fsm.UpdateState(activateTurret, null, cachedObjects);
    }
}
