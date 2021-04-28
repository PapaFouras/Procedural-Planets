using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShapeSettings", menuName = "Planets/ShapeSettings", order = 0)]
public class ShapeSettings : ScriptableObject {
    public float planetRadius = 1;

    public NoiseLayer[] noiseLayers;
    
    [System.Serializable]

    public class NoiseLayer{
        public bool enabled = true;
        public bool useFirstLayerAsAMask;
        public NoiseSettings noiseSettings;
    }
}
