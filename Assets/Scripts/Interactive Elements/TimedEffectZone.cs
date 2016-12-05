using UnityEngine;
using System.Collections;

public class TimedEffectZone : BaseTimedEffectZone
{
    [SerializeField] Behaviour[] componentsToTurnOff; 
    [SerializeField] GameObject[] gameObjectsToTurnOff;
    [SerializeField] ParticleSystem[] particlesToTurnOff;
        
    protected override void DeactivateElements()
    {
        foreach (var item in componentsToTurnOff)
        {
            item.enabled = false;
        }

        if (gameObjectsToTurnOff != null && gameObjectsToTurnOff.Length > 0)
        {
            foreach (var item in gameObjectsToTurnOff)
            {
                item.SetActive(false);
            }
        }

        if (particlesToTurnOff != null && particlesToTurnOff.Length > 0)
        {
            foreach (var item in particlesToTurnOff)
            {
                item.Stop(true);
            }
        }
    }

    protected override void ActivateElements()
    {
        foreach (var item in componentsToTurnOff)
        {
            item.enabled = true;
        }

        if (gameObjectsToTurnOff != null && gameObjectsToTurnOff.Length > 0)
        {
            foreach (var item in gameObjectsToTurnOff)
            {
                item.SetActive(true);
            }
        }

        if (particlesToTurnOff != null && particlesToTurnOff.Length > 0)
        {
            foreach (var item in particlesToTurnOff)
            {
                item.Play(true);
            }
        }
    }
}
