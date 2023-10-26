using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        
        // Load TLE data from the file
        string tleDataPath = "Assets/Resources/3le.txt"; // Update the path as needed
        string[] tleLines = File.ReadAllLines(tleDataPath);
        string line1 = tleLines[1]; // first line
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
}