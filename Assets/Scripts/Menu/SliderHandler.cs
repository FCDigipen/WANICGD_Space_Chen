using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] public string setting;
    [SerializeField] private float defaultValue;
    private Slider slider;
    void Start(){
        slider = GetComponentInChildren<Slider>(true);
        slider.value = PlayerPrefs.GetFloat(setting, defaultValue);
        // add slider event handler
        slider.onValueChanged.AddListener(delegate {OnSliderChange();}); // delegate = function "object"
    }

    void OnSliderChange() {
        PlayerPrefs.SetFloat(setting, slider.value);
    }
}
