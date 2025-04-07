using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;
    public TMP_Text valueText;

    void Awake()
    {
        source = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    void Start()
    {
        valueText.text = ((int)(slider.value * 100)).ToString() + "%";
    }

    public void ChangeVolume()
    {
        source.volume = slider.value;
        valueText.text = ((int)(slider.value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat("volume", slider.value);
        PlayerPrefs.Save();
    }
}
