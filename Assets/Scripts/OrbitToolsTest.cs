using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zeptomoby.OrbitTools;

public class OrbitToolsTest : MonoBehaviour
{
    [SerializeField] private GameObject satellitePrefab;

    private List<GameObject> _satellitesObjects = new ();
    private List<Satellite> _satellites = new ();
    private int count = 0;

    void Start()
    {
        // Load TLE data from the file
        string tleDataPath = "Assets/Resources/3le.txt"; // Update the path as needed
        string[] tleLines = File.ReadAllLines(tleDataPath);

        for (int i = 0; i < tleLines.Length - 2; i += 3)
        {
            // get first 3 lines
            string line0 = tleLines[i];
            string line1 = tleLines[i + 1];
            string line2 = tleLines[i + 2];
            TwoLineElements tle = new TwoLineElements(line0, line1, line2);
            Satellite satellite = new Satellite(tle);
            
            GameObject satelliteObject = Instantiate(satellitePrefab, Vector3.zero, Quaternion.identity);
            SpaceObject spaceObject = satelliteObject.GetComponent<SpaceObject>();
            spaceObject.Init(satellite);
            
            _satellitesObjects.Add(satelliteObject);
            _satellites.Add(satellite);
            count++;
        }
        Debug.Log(count);
    }

    // [SerializeField] private GameObject satelliteGameObject;
    // [SerializeField] private float speed = 1.0f;
    //
    // private Satellite satellite;
    //
    // void Start()
    // {
    //     // random TLE
    //     string line1 = "1 75964C 23042Z   23085.04423611  .00220051  00000-0  86866-3 0   854";
    //     string line2 = "2 75964  43.0044 186.4429 0030833 349.6785 143.4678 15.84804581    19";
    //     TwoLineElements tle = new TwoLineElements("Test", line1, line2);
    //     
    //     // instantiate new satellite
    //     satellite = new Satellite(tle);
    // }
    //
    // private void FixedUpdate()
    // {
    //     // Eci object contains position and velocity vectors
    //     Eci eci = satellite.PositionEci(Time.time * speed);
    //     
    //     // TODO: don't do this
    //     // update position and ignore velocity cause lazy
    //     Vector3 position = new Vector3((float)eci.Position.X, (float)eci.Position.Y, (float)eci.Position.Z);
    //     satelliteGameObject.transform.position = position / 1000; // div 1000 for km to m
    // }
}