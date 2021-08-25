using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AnimalEventManager : MonoBehaviour
{
    #region Exposed

    [Header("Conditions Apparitions")]
    [SerializeField]
    private List<AnimalsConditionScriptable> _AnimalsConditionsList = new List<AnimalsConditionScriptable>();

    [SerializeField]
    private IntVariable _currentScore;

    [Header("Refresh Rate")]
    [SerializeField]
    private float _delay;

    [Header("Locations")]
    private List<Transform> _animalsLocationsSpawner;

    #endregion

    #region Private
    private float _lastCheckedTime;

    private List<AnimalsConditionScriptable> _animalsWaitingToBeInstancied;
    #endregion 

    #region 

    private void Update()
    {
        if(Time.time> _lastCheckedTime - _delay)
        {
            CheckIfconditionsFilled();
            CheckIfConditionsStillFilled();
            SpawningAnimal();
            _lastCheckedTime = Time.time;
        }
        
    }
    #endregion

    #region 

    private void CheckIfconditionsFilled()
    {
        foreach (AnimalsConditionScriptable animals in _AnimalsConditionsList)
        {
            bool isPassed = true;
            for (int i = 0; i < animals.conditions.Length; i++)
            {
                if (!(animals.conditions[i]))
                {
                    isPassed = false; 
                }
            }

            if(isPassed && animals.ScoreToReach>= _currentScore)
            {
                _animalsWaitingToBeInstancied.Add(animals);
                _AnimalsConditionsList.Remove(animals);
            }
        }
    }

    private void CheckIfConditionsStillFilled()
    {
        foreach (AnimalsConditionScriptable animals in _animalsWaitingToBeInstancied)
        {
            bool isNotFilled = false;
            for (int i = 0; i < animals.conditions.Length; i++)
            {
                if (!(animals.conditions[i]))
                {
                    isNotFilled = true;
                }
            }

            if (isNotFilled || animals.ScoreToReach >= _currentScore)
            {
                _AnimalsConditionsList.Add(animals);
                _animalsWaitingToBeInstancied.Remove(animals);
            }
        }
    }

    private void SpawningAnimal()
    {
        int temp = Random.Range(0, _animalsWaitingToBeInstancied.Count);
        int percentage = Random.Range(0, 101);
        if(_animalsWaitingToBeInstancied[temp].PercentageToSpawn<= percentage)
        {
            int temp2 = Random.Range(0, _animalsLocationsSpawner.Count);
            Instantiate(_animalsWaitingToBeInstancied[temp].animalGameObject, _animalsLocationsSpawner[temp2]);
            
        }
    }
    #endregion
}
