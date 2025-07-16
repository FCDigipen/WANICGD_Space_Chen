using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSliderHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private Slider slider;
    private string setting;
    void Start(){
        slider = GetComponentInChildren<Slider>(true);
        setting = GetComponent<SliderHandler>().setting;
        
        // add slider event handler
        slider.onValueChanged.AddListener(delegate {OnSliderChange();}); // delegate = function "object"
    }

    void OnSliderChange() {
        mixer.SetFloat(setting, slider.value);
    }
}
