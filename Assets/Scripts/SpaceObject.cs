using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeptomoby.OrbitTools;

public class SpaceObject : MonoBehaviour
{
    private TimeManager _timeManager;
    private Satellite _satellite;

    public void Init(Satellite satellite)
    {
        _satellite = satellite;
    }
    
    void Start()
    {
        _timeManager = TimeManager.Instance;
    }

    void Update()
    {
        if (_satellite == null)
            return;
        
        //DateTime utcTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        //DateTime dateTime = DateTime.Now - TimeSpan.FromDays(14);
        try
        {
            Eci eci = _satellite.PositionEci(DateTime.Now);
            Vector3 position = new Vector3((float)eci.Position.X, (float)eci.Position.Y, (float)eci.Position.Z);
            transform.position = position / 1000; // Div 1000 for km to m
        }
        catch (DecayException e)
        {

        }
        catch (PropagationException e)
        {
            
        }
    }
}
