using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Plants : MonoBehaviour
{

    #region Exposed

    [Header("Pousse")]


    [SerializeField]
    private float _TotalGrowTime;

    [Header("Gain à la recolte")]
    [SerializeField]
    private int _HarvestBonus;

    [Header("Step")]
    [SerializeField]
    private int _step;

    [Header("Temps gagné à chaque arrosage")]
    [SerializeField]
    private float _wateredTime = 20f;

    [SerializeField]
    private float _waterCanCooldown; 

    [Header("Compatibilité")]
    [SerializeField]
    private List<CardScriptable> _compatibilytPlants = new List<CardScriptable>();

    [SerializeField]
    private CardScriptable _card;

    [SerializeField]
    private float _radiusDetection;

    [SerializeField]
    private float _complement;

    [Header("Limitation du nombre de plante")]
    [SerializeField]
    private IntVariable _limitation;

    #endregion

    #region private

    GrowPlants _gp;
    float _phaseTime;
    float _spawnTime;
    int _MultiplyWatered = 0;
    float _timewhenLastwatered;
    float _completscore;
    Transform _transform;
    List<AlreadyUseCard> _usedCardsList = new List<AlreadyUseCard>();
    int _PollinatorDurability;
    string _name;
    private GroundLayering _groundLayering;

    #endregion

    #region Unity API

    private void Awake()
    {
        _gp = GetComponentInParent<GrowPlants>();
        _phaseTime = _TotalGrowTime / _gp.GetMaxTier();
        _spawnTime = Time.time;
        FindObjectOfType<PlantsManager>().AddPlantInMapList(this.gameObject);
        _name = _card._CardName;
        _transform = GetComponent<Transform>();
        GetAllPlantHitted();

    }

    private void FixedUpdate()
    {
        GrowPlantWithTime();

    }
    #endregion

    #region Methods

    private void GrowPlantWithTime()
    {
        // (temps global - le temps de la plante ) + ( le temps que gagne à l'arrosage * le nombre d'arrossage)  *
        // ( 1 * le nombre de plante complémentaire) > le temps d'une phase * le tiers actuel

        float basicTime = Time.time - _spawnTime;
        float wateredtime = _wateredTime * _MultiplyWatered;
        float completTime = 1  +(1* (_complement * _completscore));

            if (((basicTime + wateredtime) * completTime)> (_phaseTime *( _gp.GetCurrentTier()+1)))
            {
            
                _gp.SetCurrentTier(_gp.GetCurrentTier() + _step);
            

        }

        
     
    }

    private void GetAllPlantHitted()
    {
        CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
        Collider[] hits = Physics.OverlapSphere(_transform.position, _radiusDetection);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].GetComponent<CapsuleCollider>())
            {
                CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
                if (hc.GetComponentInParent<Plants>() &&
                    !(cc.GetInstanceID().Equals(hc.GetInstanceID())) 
                    && (hc.GetInstanceID() != 0))
                {
                    if (CheckIsCompatible(hits[i].GetComponentInParent<Plants>()))
                    {
                        _completscore++;
                    }
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_transform.position, _radiusDetection);
    }
    #endregion

    #region public


    public void AddTier( GameObject go)
    {
        if(_MultiplyWatered == 0)
        {
            _MultiplyWatered++;
            _timewhenLastwatered = Time.time;
        }
        if (go.GetComponent<WaterCan>() && (Time.time - _timewhenLastwatered > _waterCanCooldown))
        {
 
            _MultiplyWatered++;
            _timewhenLastwatered = Time.time;
        }

    }

    public void DeleteTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() - _step);
    }

    public int getBonusMalus()
    {
        return _HarvestBonus;
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

    public int GetPlantHashcode()
    {
        return _name.GetHashCode();
    }

    #endregion
    #region main

    private void OnDestroy()
    {
        _limitation.Value++;
     


    }

    public CardScriptable GetCard()
    {
        return _card;
    }

    public void NoticeOtherAboutDestruction()
    {
        CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
        Collider[] hits = Physics.OverlapSphere(_transform.position, _radiusDetection);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].GetComponent<CapsuleCollider>())
            {
                CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
                if (hc.GetComponentInParent<Plants>() &&!(cc.GetInstanceID().Equals(hc.GetInstanceID()))&& (hc.GetInstanceID() != 0))
                {
                   
                    hc.GetComponentInParent<Plants>().AddOnUsedCardList(_card);
                }
            }

        }
    }

    public void AddOnUsedCardList(CardScriptable cs)
    {
        for (int i = 0; i < _usedCardsList.Count; i++)
        {

            if (_usedCardsList[i]._cardAlreadyUse._CardName.Equals(cs._CardName))
            {

                if(_usedCardsList[i]._around== 0)
                {
                    _usedCardsList.Remove(_usedCardsList[i]);
                    _compatibilytPlants.Add(cs);
                    _completscore--;
                }else if (_usedCardsList[i]._around >0)
                {
                    _usedCardsList[i]._around--;
                }

            }
        }
    }

    public string GetName()
    {
        return _name;
    }

    public float GetTotalGrowTime()
    {
        return _TotalGrowTime;
    }

    public bool CheckIsCompatible(Plants plantIn)
    {
        bool _isIn = false;
        foreach (CardScriptable plant in _compatibilytPlants) { 

            if (plant._CardName.Equals(plantIn.GetName()))
            {
                  _isIn = true;
                _compatibilytPlants.Remove(plantIn._card);
                AlreadyUseCard temp = new AlreadyUseCard(plantIn._card, 1);
                _usedCardsList.Add(temp);
                plantIn.AddCompletScore(_card);
                return _isIn;
            }
        }

        if (!_isIn)
        {
            for (int i = 0; i < _usedCardsList.Count; i++)
            {
                if (_usedCardsList[i]._cardAlreadyUse._CardName.Equals(plantIn.GetName()))
                {
                    _usedCardsList[i]._around++;
                    Debug.Log(_usedCardsList[i]._around);
                }

            }
        }

        return _isIn;
    }

    public void AddCompletScore(CardScriptable card)
    {
        if (_compatibilytPlants.Contains(card))
        {
            _completscore++;
            _compatibilytPlants.Remove(card);
            AlreadyUseCard temp = new AlreadyUseCard(card, 1);
            _usedCardsList.Add(temp);

        }

    }




    #endregion

    #region External Class
    public class  AlreadyUseCard 
        {
        public CardScriptable _cardAlreadyUse;
        public int _around;

            public AlreadyUseCard(CardScriptable cs, int around)
            {
            _cardAlreadyUse = cs;
            _around = around;

            }
       
        }

    #endregion
}
