using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
  ShapeSettings shapeSettings;

 
  INoiseFilter[] noiseFilters;

  public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;

        noiseFilters = new INoiseFilter[shapeSettings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(shapeSettings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere){
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0){
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(shapeSettings.noiseLayers[0].enabled){
                elevation = firstLayerValue;
            }
        }
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if(shapeSettings.noiseLayers[i].enabled){

                float mask = shapeSettings.noiseLayers[i].useFirstLayerAsAMask ? firstLayerValue : 1;
            elevation += noiseFilters[i].Evaluate(pointOnUnitSphere);

            }
        }
        elevationMinMax.AddValue(elevation);
        return  elevation;
    }
    public float GetScaledElevation(float unscaledElevation){
        float elevation = Mathf.Max(0,unscaledElevation);
        elevation = shapeSettings.planetRadius * (1+elevation);
        return elevation;
    }
}
