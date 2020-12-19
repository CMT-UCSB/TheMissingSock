using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{

    public Slider volumeSlider;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private static float masterVolume = 0.5f;

    private void Start()
    {
        AudioListener.volume = masterVolume;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        masterVolume = volumeSlider.value;
    }
}
