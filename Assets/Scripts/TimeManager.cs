using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Systems;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    #region singleton
    private static TimeManager _instance;

    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("TimeManager");
                    _instance = go.AddComponent<TimeManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    
    private bool _isPlaying;
    public float TimeSpeed { get; private set; } = 1.0f; // 1.0 is normal speed
    
    [SerializeField] private Slider slider;
    private SatelliteOrbitSystem satelliteOrbitSystem; // Reference to your ECS system

    private bool _isUserInteracting = false;
    private IEnumerator _delayUnclicked = null;
    
    private void Start()
    {
        // Get the command line arguments
        string[] commandLineArgs = Environment.GetCommandLineArgs();

        if (commandLineArgs.Length > 1)
        {
            if (int.TryParse(commandLineArgs[1], out var output))
                slider.maxValue = output - 1;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        
        // Load TLE data from the file
        string tleDataPath = "Assets/Resources/3le.txt"; // Update the path as needed
        string[] tleLines = File.ReadAllLines(tleDataPath);
        string line1 = tleLines[1]; // first line
    }

    public void Update()
    {
        if (_isUserInteracting)
            return;
        
        slider.value += 1;
        if (slider.value >= slider.maxValue)
            slider.value = 0;
    }

    public void Play()
    {
        _isPlaying = true;
        Time.timeScale = TimeSpeed;
    }

    public void Pause()
    {
        _isPlaying = false;
        Time.timeScale = 0.0f; // Paused
    }

    public void SetTimeSpeed(float speed)
    {
        TimeSpeed = Mathf.Max(speed, 0.0f); // Ensure speed is not negative
        if (_isPlaying)
        {
            Time.timeScale = TimeSpeed;
        }
    }

    public int GetTime()
    {
        return (int)slider.value;
    }

    public void OnSliderClicked()
    {
        Debug.Log("Clicked");
        _isUserInteracting = true;
    }

    public void OnSliderUnclicked()
    {
        Debug.Log("Unclicked");
        if (_delayUnclicked != null)
            StopCoroutine(_delayUnclicked);
        StartCoroutine(_delayUnclicked = DelayUnclicked());
    }

    private IEnumerator DelayUnclicked()
    {
        yield return new WaitForSeconds(1.0f);
        _isUserInteracting = false;
    }
}