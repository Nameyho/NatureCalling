using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Building : MonoBehaviour
{
    #region

    [SerializeField]
    private CardScriptable _buildingCard;

    [SerializeField]
    private IntVariable _currentScore;

    #endregion

    #region Mains



    //private void Awake()
    //{
    //    _currentScore.Value += _buildingCard._bonusBioDiversity;
    //}

    private void OnDestroy()
    {
        if (_currentScore)
        {
        _currentScore.Value -= _buildingCard._bonusBioDiversity;

        }
    }

    #endregion

}
