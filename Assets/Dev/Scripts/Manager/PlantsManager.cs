using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsManager : MonoBehaviour
{
    #region Private

    private List<GameObject> _plantOnTheMap = new List<GameObject>();
    private float _lastTimeinfestation = 0 ;

    #endregion

    #region Exposed

    [SerializeField]
    private float _intervalBetweenInfestation;

    [SerializeField]
    private float _percentagePlantsToInfest;

    #endregion

    #region public

    public void AddPlantInMapList(GameObject plant)
    {
        _plantOnTheMap.Add(plant);
    }

    public void DeletePlantInMapList(GameObject plant)
    {
        _plantOnTheMap.Remove(plant);
    }


    public void HiglightSelectedType(Plants plant)
    {
        for (int i = 0; i < _plantOnTheMap.Count; i++)
        {
            if (_plantOnTheMap[i].Equals(plant))
            {
                //changer le materiel
            }
        }
    }


    #endregion

    #region private methods

    private void Infestation()
    {
        int t = 0;
        if (Time.time - _lastTimeinfestation > _intervalBetweenInfestation && _plantOnTheMap.Count>0)
        {
            float plantToInfest = _plantOnTheMap.Count * (_percentagePlantsToInfest/100);
            int rand= Random.Range(0, _plantOnTheMap.Count);
            if (!(_plantOnTheMap[rand].GetComponent<Plants>().GetInfested())&& (_plantOnTheMap[rand].GetComponent<Plants>().GetcanBeInfested()))
            {
                _plantOnTheMap[rand].GetComponent<Plants>().setInfested(true);
                t++;
            }
            if (t >= plantToInfest)
            {
                _lastTimeinfestation = Time.time;
            }
        }
    }

    #endregion

    #region Unity API

    private void Update()
    {
        Infestation();
    }

    #endregion



}
