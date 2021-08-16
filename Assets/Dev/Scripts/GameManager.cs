using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GameManager: MonoBehaviour
{

    #region Exposed

    [Header("Progression")]

    [SerializeField]
    private FloatVariable _currentScore;

    #endregion

    #region Public Methods

    public void AddProgression(float score)
    {
        _currentScore.Value += score;
    }

    public void DeleteProgression(float score)
    {
        _currentScore.Value -= score;
    }
    #endregion
}
