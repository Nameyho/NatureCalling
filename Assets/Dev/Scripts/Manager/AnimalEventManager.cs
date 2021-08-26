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
    [SerializeField]
    private List<Transform> _animalsLocationsSpawner;

    #endregion

    #region Private
    private float _nextCheckTime;

    private List<AnimalsConditionScriptable> _animalsWaitingToBeInstancied = new List<AnimalsConditionScriptable>();
    #endregion 

    #region 

    private void Update()
    {
        if(Time.time>= _nextCheckTime)
        {
            CheckIfconditionsFilled();
            CheckIfConditionsStillFilled();
            SpawningAnimal();
            _nextCheckTime = Time.time + _delay;
        }
        
    }
    #endregion

    #region 

    private void CheckIfconditionsFilled()
    {
       
        for (int i = 0; i <_AnimalsConditionsList.Count ; i++)
        {
            AnimalsConditionScriptable animals = _AnimalsConditionsList[i];
            bool isPassed = true;
                for (int j = 0; j < animals.conditions.Length; j++)
                {
                    if (!(animals.conditions[j]))
                    {
                        isPassed = false; 
                    }
                }
            

                if(isPassed &&  _currentScore >= animals.ScoreToReach)
                {
                _animalsWaitingToBeInstancied.Add(_AnimalsConditionsList[i]);
               
                _AnimalsConditionsList.Remove(_AnimalsConditionsList[i]);
                }

        } 
        
    }

    private void CheckIfConditionsStillFilled()
    {
       
       
        for (int i = 0; i < _animalsWaitingToBeInstancied.Count; i++)
        {
            AnimalsConditionScriptable animals = _animalsWaitingToBeInstancied[i];
            bool isNotFilled = false;
            for (int j = 0; j < animals.conditions.Length; j++)
            {
                if (!(animals.conditions[j]))
                {
                    isNotFilled = true;
                }
            }

            if (isNotFilled || animals.ScoreToReach > _currentScore)
            {
                Debug.Log("Add instanciatie");
                _AnimalsConditionsList.Add(_animalsWaitingToBeInstancied[i]);
                _animalsWaitingToBeInstancied.Remove(_animalsWaitingToBeInstancied[i]);
            }
        }
    }

    private void SpawningAnimal()
    {
        if (_animalsWaitingToBeInstancied.Count > 0)
        {
                int temp = Random.Range(0, _animalsWaitingToBeInstancied.Count);
                int percentage = Random.Range(0, 101);
               
                if(_animalsWaitingToBeInstancied[temp].PercentageToSpawn>= percentage)
                {
                    int temp2 = Random.Range(0, _animalsLocationsSpawner.Count);
                    Instantiate(_animalsWaitingToBeInstancied[temp].animalGameObject, _animalsLocationsSpawner[temp2]);
                _animalsWaitingToBeInstancied.Remove(_animalsWaitingToBeInstancied[temp]);
              
                }

        }
    }

    public void AddToWaitinglist(AnimalsConditionScriptable acs)
    {
        _AnimalsConditionsList.Add(acs);
    }
    #endregion
}
