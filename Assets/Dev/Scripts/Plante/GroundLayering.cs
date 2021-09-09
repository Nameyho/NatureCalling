using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLayering : MonoBehaviour
{

    #region Exposed


    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private int _NumberMaxOfPlants;


    #endregion


    #region 

    private int _numberOfPlants = 0;

    #endregion



    #region UnityAPI


    private void Awake()
    {
        _audioSource.Play();
    }
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
    #endregion
}
