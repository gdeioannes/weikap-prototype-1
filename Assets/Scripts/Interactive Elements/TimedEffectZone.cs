using UnityEngine;
using System.Collections;

public class TimedEffectZone : MonoBehaviour
{
    [SerializeField] Behaviour[] componentsToTurnOff;
    [SerializeField] GameObject[] gameObjectsToTurnOff;
    [SerializeField] ParticleSystem[] particlesToTurnOff;
    [SerializeField] float waitInterval;
    [SerializeField] float activeInterval;

    IEnumerator Start()
    {
        while (true)
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

            yield return new WaitForSeconds(waitInterval);

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

            yield return new WaitForSeconds(activeInterval);
        }
    }
}
