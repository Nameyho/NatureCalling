using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public AudioMixer _MasterMixer;

    public void SetMasterVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Master", volume.value);
    }

    public void SetAnimalsVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Animal", volume.value);
    }

    public void SetVFXVolume(Slider volume)
    {
        _MasterMixer.SetFloat("VFX", volume.value);
    }

    public void SetMusicVolume(Slider volume)
    {
        Debug.Log(_MasterMixer.name);
        _MasterMixer.SetFloat("Music", volume.value);
    }

}
