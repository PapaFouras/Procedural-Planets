using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColourGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if(texture == null ||texture.height != settings.biomeColourSettings.biomes.Length){
            texture = new Texture2D(textureResolution * 2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32,false);
        }
        biomeNoiseFilter= NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax){
        settings.planetMaterial.SetVector("_elevationMinMax",new Vector4(elevationMinMax.Min,elevationMinMax.Max));

    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere){
        float heigthPercent = (pointOnUnitSphere.y +1)/2f;
        heigthPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere)-settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColourSettings.biomes.Length;

        float blendRange = settings.biomeColourSettings.blendAmount / 2f + .001f;

        for (int i = 0; i < numBiomes; i++)
        {
           float dist = heigthPercent - settings.biomeColourSettings.biomes[i].startHeight;
           float weight = Mathf.InverseLerp(-blendRange,blendRange,dist);
           biomeIndex *= (1-weight);
           biomeIndex += i * weight;
        }

        return biomeIndex/Mathf.Max(1,(numBiomes-1));
    }

    public void UpdateColours(){
        Color[] colours = new Color[texture.width * texture.height];
        int colourIndex= 0;

        foreach (var biome in settings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientCol;
                if(i<textureResolution){
                    gradientCol =settings.oceanColor.Evaluate(i/(textureResolution-1f));
                }
                else{
                    gradientCol = biome.gradient.Evaluate((i-textureResolution)/(textureResolution-1f));
                }
                Color tintColor = biome.tint;
                colours[colourIndex] = gradientCol * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                colourIndex ++;
            }
        }

        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture",texture);
    }
}
