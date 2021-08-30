using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{

    #region Exposed

    #endregion

    #region private

    GrowPlants _gp;

    #endregion

    #region Unity API

    private void Awake()
    {
        _gp = GetComponent<GrowPlants>();
    }


    #endregion



    #region public


    public void AddTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() +1);
    }

    public void DeleteTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() - 1);
    }


    #endregion
}
