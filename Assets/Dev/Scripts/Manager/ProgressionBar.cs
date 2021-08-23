using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class ProgressionBar : MonoBehaviour
{

    #region Exposed

    [Header("Image bar remplie")]
    [SerializeField]
    private Image _image;

    [Header("Data")]
    [SerializeField]
    private FloatVariable _currentScore;

    [SerializeField]
    private FloatVariable _maximumScore;

    #endregion

    #region Unity API

    private void Update()
    {
        UpdatingProgressionBar();
    }

    #endregion

    #region Methods

    public void UpdatingProgressionBar()
    {
        _image.fillAmount= _currentScore.Value / _maximumScore.Value;
    }

    #endregion
}
