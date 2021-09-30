using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GroundLayering : MonoBehaviour
{

    #region Exposed


    [SerializeField]
    private int _NumberMaxOfPlants;

    [SerializeField]
    private IntVariable _remainingCard;


    #endregion


    #region 

    private int _numberOfPlants = 0;

    #endregion



    #region UnityAPI



    #endregion


    #region public


    public void AddPlants(){
        _numberOfPlants++;
    }

    public void DeletePlants()
    {
        _numberOfPlants--;
    }

    public bool IsPlantsOn()
    {
        if(_numberOfPlants > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsLayeringBuildable()
    {
        if(_numberOfPlants< _NumberMaxOfPlants)
        {
            return true;
        }
        return false;
    }

    public void AddRemaining()
    {
        if (_remainingCard >= 0)
        {
        _remainingCard.Value++;

        }
    
    }
    #endregion

    #region Privates Methods


    #endregion
}
