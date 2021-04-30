using UnityEngine;

[CreateAssetMenu(fileName = "ColourSettings", menuName = "Planets/ColorSettings", order = 0)]
public class ColorSettings : ScriptableObject {
    public Gradient gradient;
    public BiomeColourSettings biomeColourSettings;
    public Gradient oceanColor;

    
    public Material planetMaterial;


[System.Serializable]
    public class BiomeColourSettings{

        
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0,1)]
        public float blendAmount;
        public Biome[] biomes;
        [System.Serializable]
        
        public class Biome{
            public Gradient gradient;
            public Color tint;
            [Range(0,1)] 
            public float tintPercent;
            [Range(0,1)]
            public float startHeight;

        }
    }
}