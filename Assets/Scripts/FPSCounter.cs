using System;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;

    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }

    void GetFPS()
    {
        fps = (int)(1f/Time.unscaledDeltaTime);
        FPSCounterText.text = $"{fps}";
    }
}