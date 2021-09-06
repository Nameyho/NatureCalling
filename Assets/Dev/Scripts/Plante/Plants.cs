using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{

    #region Exposed

    [SerializeField]
    private int _bonusMalus;

 


    #endregion

    #region private

    GrowPlants _gp;
    Rigidbody _rb;
    SphereCollider sc;

    int _Waterdurability;
    int _PollinatorDurability;

    private GroundLayering _groundLayering;

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
        if (_Waterdurability > 0)
        {
        _Waterdurability--;

        }
        if (_PollinatorDurability> 0)
        {
            _PollinatorDurability--;
        }
        Destroy(_rb);
    }

    public void AddTier(int durability,GameObject go)
    {
        if (_Waterdurability == 0 && durability> 0 && go.GetComponent<WaterCan>())
        {
            _Waterdurability = durability;
            _gp.SetCurrentTier(_gp.GetCurrentTier() +_bonusMalus);
            _gp.RemoveMaxTier(2);
        }
        if (_PollinatorDurability == 0 && durability > 0 && go.GetComponent<Pollinator>())
        {
            _PollinatorDurability = durability;
            _gp.SetCurrentTier(_gp.GetCurrentTier() + _bonusMalus);
            _gp.RemoveMaxTier(2);
        }
    }

    public void DeleteTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() - _bonusMalus);
    }

    public int getBonusMalus()
    {
        return _bonusMalus;
    }

    public void GetAllPlants()
    {
        Plants[] Plants = FindObjectsOfType<Plants>();
        
        for (int i = 0; i < Plants.Length; i++)
        {
            Plants[i].ApplyEffect();
        }
    }

    public void SetGroundLayering(GroundLayering gl) 
    {
        _groundLayering = gl;
    }

    public GroundLayering GetGroundLayering()
    {
        return _groundLayering;
    }

    #endregion
}
