using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{

    #region Exposed

    [SerializeField]
    private int _bonusMalus;

    [SerializeField]
    private float _TotalGrowTime;

    [SerializeField]
    private float _wateredTime = 20f;


    #endregion

    #region private

    GrowPlants _gp;
    Rigidbody _rb;
    float _phaseTime;
    float _spawnTime;
    int _MultiplyWatered = 0;
    float _timewhenLastwatered;
    int _Waterdurability;
    int _PollinatorDurability;

    private GroundLayering _groundLayering;

    #endregion

    #region Unity API

    private void Awake()
    {
        _gp = GetComponent<GrowPlants>();
        _rb = GetComponent<Rigidbody>();
        _phaseTime = _TotalGrowTime / _gp.GetMaxTier();
        _spawnTime = Time.time;
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag== "Plants"  )
    //    {
    //        Debug.Log(other.name);
    //        other.GetComponentInParent<Plants>().ApplyEffect();
    //    }

    //}


    private void Update()
    {
        GrowPlantWithTime();
    }
    #endregion

    #region Methods

    private void GrowPlantWithTime()
    {
        if((Time.time - _spawnTime) + (_wateredTime * _MultiplyWatered )  > _phaseTime * _gp.GetCurrentTier())
        {
            _gp.SetCurrentTier(_gp.GetCurrentTier() + _bonusMalus);
        }
    }



    #endregion

    #region public

    //public void ApplyEffect()
    //{
    //    _gp.SetCurrentTier(_gp.GetCurrentTier() + _bonusMalus);
    //    if (_Waterdurability > 0)
    //    {
    //    _Waterdurability--;

    //    }
    //    if (_PollinatorDurability> 0)
    //    {
    //        _PollinatorDurability--;
    //    }
    //    Destroy(_rb);
    //}

    public void AddTier(int durability,GameObject go)
    {
        if (go.GetComponent<WaterCan>()&& (Time.time -_timewhenLastwatered> _wateredTime))
        {
            //_Waterdurability = durability;
            _gp.SetCurrentTier(_gp.GetCurrentTier() +_bonusMalus);
            _MultiplyWatered++;
            _timewhenLastwatered = Time.time;
        }
        if (_PollinatorDurability == 0 && durability > 0 && go.GetComponent<Pollinator>())
        {
          //  _PollinatorDurability = durability;
            _gp.SetCurrentTier(_gp.GetCurrentTier() + _bonusMalus);
            
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

    //public void GetAllPlants()
    //{
    //    Plants[] Plants = FindObjectsOfType<Plants>();
        
    //    for (int i = 0; i < Plants.Length; i++)
    //    {
    //        Plants[i].ApplyEffect();
    //    }
    //}

    public void SetGroundLayering(GroundLayering gl) 
    {
        _groundLayering = gl;
    }

    public GroundLayering GetGroundLayering()
    {
        return _groundLayering;
    }

    public void resetSpawnTime()
    {
        _spawnTime = Time.time;
    }

    public void ResetMultiply()
    {
        _MultiplyWatered = 0;
    }
    #endregion
}
