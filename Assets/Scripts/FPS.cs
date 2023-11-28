using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private TMP_Text fps;
    private float _fps;

    void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 1);
    }

    public void GetFPS()
    {
        _fps = (int)(1f / Time.unscaledDeltaTime);
        fps.text = _fps + " FPS";
    }
}
