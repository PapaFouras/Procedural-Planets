using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        planet.GetComponent<Planet>().GeneratePlanet();
    }

    
}
