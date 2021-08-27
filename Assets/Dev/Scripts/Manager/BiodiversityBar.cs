using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class BiodiversityBar : MonoBehaviour
{

    #region Exposed

    private Image _image;

    [Header("Data")]
    [SerializeField]
    private IntVariable _currentScore;

    [SerializeField]
    private IntVariable _maximumScore;

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
        _image.fillAmount= (float)_currentScore.Value / (float) _maximumScore.Value;
    }

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    #endregion
}
