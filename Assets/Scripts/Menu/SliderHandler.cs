using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private string setting;
    [SerializeField] private float defaultValue;
    Slider slider;
    void Start(){
        slider = GetComponentInChildren<Slider>(true);
        slider.value = PlayerPrefs.GetFloat(setting, defaultValue);
        // add slider event handler
        slider.onValueChanged.AddListener(delegate {OnSliderChange();});
    }

    void OnSliderChange() {
        PlayerPrefs.SetFloat(setting, slider.value);
    }
}
