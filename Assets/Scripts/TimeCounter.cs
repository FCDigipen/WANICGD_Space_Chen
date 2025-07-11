using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // update time text
    private TextMeshProUGUI text;
    void Start(){text = GetComponent<TextMeshProUGUI>();}

    // Update is called once per frame
    void Update()
    {
        TimeSpan t = TimeSpan.FromSeconds(Time.timeAsDouble);
        text.text = $"time={t.Minutes:00}:{t.Seconds:00}:{t.Milliseconds:000};";
    }
}
