using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SoundsManager : MonoBehaviour
{
    #region Exposed

    [Header("Music Asset")]
    [SerializeField]
    private AudioClip _birdMusic;

    [Header("Events")]
    [SerializeField]
    private GameEvent _birdshasthere;

    #endregion

    #region Unity API

    private void Awake()
    {
        if (_audioSource == null) { _audioSource = GetComponent<AudioSource>(); }
        _maxVolume = _audioSource.volume;
    }

    private void Start()
    {
        _birdshasthere.AddListener(PlayMusic);
    }

    #endregion


    #region Main



    private void PlayMusic()
    {
        _audioSource.clip = _birdMusic;
        _audioSource.Play();
    }

    #endregion

    #region Private and Protected Members

    private AudioSource _audioSource;
    private float _lerpValue;
    private float _maxVolume;

    #endregion

}
