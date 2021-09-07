using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsManager : MonoBehaviour
{
    #region Private

    private List<GameObject> _plantOnTheMap = new List<GameObject>();

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




}
