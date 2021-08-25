using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GameManager: MonoBehaviour
{

    #region Exposed

    [Header("Progression")]

    [SerializeField]
    private IntVariable _currentScore;

    [SerializeField]
    private int _scoreToChangeScene;

    #endregion

    #region Public Methods

    public void AddProgression(int score)
    {
        _currentScore.Value += score;
    }

    public void DeleteProgression(int score)
    {
        _currentScore.Value -= score;
    }
    #endregion

    public void CheckMinimalProgression()
    {
        if (_scoreToChangeScene < _currentScore.Value)
        {
        //bravo tu peux aller au niveau suivant si tu veux
        }
    }

    public void Awake()
    {
        _currentScore.Value = 0;
    }
}
