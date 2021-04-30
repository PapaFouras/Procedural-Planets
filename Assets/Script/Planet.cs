using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2,256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask {All,Top,Bottom,Left,Right,Front,Back};
    public FaceRenderMask faceRenderMask;

[HideInInspector]
    public bool shapeSettingsFoldOut;
    [HideInInspector]
    public bool colourSettingsFoldOut;

    public ShapeSettings shapeSettings;
    public ColorSettings colourSettings;

    ShapeGenerator shapeGenerator = new ShapeGenerator();

    public ColourGenerator colourGenerator = new ColourGenerator();

    [SerializeField,HideInInspector]
   MeshFilter[] meshFilters;
   TerrainFace[] terrainFaces;

   public void Initialise(){
       shapeGenerator.UpdateSettings(shapeSettings);
       colourGenerator.UpdateSettings(colourSettings);
       if(meshFilters == null || meshFilters.Length == 0){
           meshFilters = new MeshFilter[6];
       }
       terrainFaces = new TerrainFace[6];

       Vector3[] directions = {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};
       for (int i = 0; i < 6; i++)
       {
           if(meshFilters[i]==null){
            GameObject meshObj = new GameObject("mesh"+i);
           meshObj.transform.parent = transform;

           meshObj.AddComponent<MeshRenderer>();
           meshFilters[i] = meshObj.AddComponent<MeshFilter>();
           meshFilters[i].sharedMesh = new Mesh();

           }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

           terrainFaces[i] = new TerrainFace(shapeGenerator,meshFilters[i].sharedMesh,resolution,directions[i]);
           bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask -1 == i;
           meshFilters[i].gameObject.SetActive(renderFace);
          
       }
   }

   public  void GenerateMesh(){

       for (int i = 0; i < 6; i++)
       {
           if(meshFilters[i].gameObject.activeSelf){
           terrainFaces[i].ConstructMesh();

           }
       }

       colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
      
   }

    public void GenerateColour(){
        colourGenerator.UpdateColours();
         for (int i = 0; i < 6; i++)
       {
           if(meshFilters[i].gameObject.activeSelf){
           terrainFaces[i].UpdateUvs(colourGenerator);

           }
       }

    }

    public void OnColourSettingsUpdated(){
        if(autoUpdate){
            Initialise();
            GenerateColour();
        }
        
    }

    public void OnShapeSettingsUpdated(){
        if(autoUpdate){
            Initialise();
            GenerateMesh();
        }

    }

    public void GeneratePlanet(){
        Initialise();
        GenerateMesh();
        GenerateColour();
    }


    private void Start() {
       StartCoroutine(GeneratePlanetAfter1s());
    }

    IEnumerator GeneratePlanetAfter1s(){
        yield return new WaitForSeconds(.05f);
        GeneratePlanet();
    }


}
