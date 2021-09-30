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

    [SerializeField]
    private IntVariable _limitation;
 
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

        if(_limitation.Value >= 0)
        {
            _limitation.Value++;
        }
    }

    #endregion

}
