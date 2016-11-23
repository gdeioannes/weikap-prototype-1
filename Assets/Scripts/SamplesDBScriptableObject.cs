using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewSamplesDB", menuName = "Weikap/DB/Samples")]
public class SamplesDBScriptableObject : ScriptableObject {

    [System.Serializable]
    public struct Sample
    {
        public string Name;
        public string Description;

        public Sprite Icon;
        public Color IconColor;
        public Texture2D Image;
        public Color ImageColor;
    }

    public Sample[] samples;
}
