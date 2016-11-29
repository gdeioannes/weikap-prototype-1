using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SamplesInLevelController : MonoBehaviour {

    public SampleUIIconController sampleUIIconPrefab;
    public Transform iconsContainer;
    private Dictionary<int, Transform> iconContainers;

    public void FillSamplesInLevel(List<int> samples)
    {
        iconsContainer.DestroyChildren();
        iconContainers = new Dictionary<int, Transform>();
        HashSet<int> addedElements = new HashSet<int>();

        foreach (var item in samples)
        {
            if (!addedElements.Contains(item)) // only add unique elements
            {
                SampleUIIconController newSampleIcon = Object.Instantiate<SampleUIIconController>(sampleUIIconPrefab);
                newSampleIcon.transform.SetParent(iconsContainer, false);                
                newSampleIcon.Set(item);
                newSampleIcon.gameObject.SetActive(true);
                addedElements.Add(item);
                iconContainers[item] = newSampleIcon.transform;
            }
        }
    }

    public Transform GetSampleIconTransform(int sampleId)
    {
        if (iconContainers.ContainsKey(sampleId))
        {
            return iconContainers[sampleId];
        }
        return null;
    }
}
