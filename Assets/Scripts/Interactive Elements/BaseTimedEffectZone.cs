using UnityEngine;
using System.Collections;

public class BaseTimedEffectZone : MonoBehaviour {

    [SerializeField] float waitInterval;
    [SerializeField] float activeInterval;

    IEnumerator Start()
    {
        while (true)
        {
            DeactivateElements();

            yield return new WaitForSeconds(waitInterval);

            ActivateElements();

            yield return new WaitForSeconds(activeInterval);
        }
    }

    protected virtual void DeactivateElements() { }
    protected virtual void ActivateElements() { }
}
