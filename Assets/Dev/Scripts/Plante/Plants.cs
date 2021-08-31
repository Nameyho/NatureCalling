using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{

    #region Exposed

    [SerializeField]
    private int _bonusMalus;

    [SerializeField]


    #endregion

    #region private

    GrowPlants _gp;
    Rigidbody _rb;
    SphereCollider sc;
   
 

    #endregion

    #region Unity API

    private void Awake()
    {
        _gp = GetComponent<GrowPlants>();
        _rb = GetComponent<Rigidbody>();
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag== "Plants"  )
    //    {
    //        Debug.Log(other.name);
    //        other.GetComponentInParent<Plants>().ApplyEffect();
    //    }

    //}

    #endregion



    #region public

    public void ApplyEffect()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() + _bonusMalus);
        Destroy(_rb);
    }

    public void AddTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() +_bonusMalus);
        _gp.RemoveMaxTier(2);
    }

    public void DeleteTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() - _bonusMalus);
    }

    public int getBonusMalus()
    {
        return _bonusMalus;
    }

    #endregion

    #region Privates

    public void GetAllPlants()
    {
        Plants[] Plants = FindObjectsOfType<Plants>();
        
        for (int i = 0; i < Plants.Length; i++)
        {
            Plants[i].ApplyEffect();
        }
    }

    #endregion
}
