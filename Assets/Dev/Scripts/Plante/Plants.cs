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

    [Header("Temps gagné à chaque pollinisation")]
    [SerializeField]
    private float _PollinisationTime = 20f;

    [SerializeField]
    private float _pollinisationCooldown;

    [Header("points gagné à l'arrossage ou pollinisation")]
    [SerializeField]
    private int _pollinisationWateredPoint;

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


    [Header("Infestation")]
    [Range(0,1)]
    [SerializeField]
    private float _percentageInfestation;

    #endregion

    #region private

    GrowPlants _gp;
    float _phaseTime;
    float _spawnTime;

    int _MultiplyWatered = 0;
    float _timewhenLastwatered;

    int _multiplyPollinisation = 0;
    float _timeWhenLastPollinisation;

    float _completscore;
    Transform _transform;
    List<AlreadyUseCard> _usedCardsList = new List<AlreadyUseCard>();
   
    string _name;
    private GroundLayering _groundLayering;
    private bool _isInfested = false;
    private bool _canBeInfested = true;
    private int _repellentAround;
    private bool _isChecked = false;
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

    private void Update()
    {
        if (_repellentAround <= 0)
        {
            GetComponent<Plants>().SetcanBeInfested(true);
        }
        if (!_isChecked && (Time.time - _spawnTime > 1f))
        {
            CheckInfestation();
            _isChecked = true;
        }
        if (!_canBeInfested)
        {
            _isInfested = false;
        }
        if (_repellentAround > 0)
        {
            _canBeInfested = false;
        }

    }
    #endregion

    #region Methods

    private void GrowPlantWithTime()
    {
        // (temps global - le temps de la plante ) + ( le temps que gagne à l'arrosage * le nombre d'arrossage)  *
        // ( 1 * le nombre de plante complémentaire) > le temps d'une phase * le tiers actuel

        float basicTime = Time.time - _spawnTime;
        float wateredtime = _wateredTime * _MultiplyWatered;
        float pollinatorTime = _PollinisationTime * _multiplyPollinisation;
        float completTime = 1  +(1* (_complement * _completscore));
        float infested = 1;
        if (_isInfested)
        {
            infested = _percentageInfestation;
        }
        else
        {
            infested = 1f;
        }

            if ((((basicTime + wateredtime + pollinatorTime) * completTime) * infested)> (_phaseTime *( _gp.GetCurrentTier()+1)))
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
                Plants plante = (hits[i].GetComponentInParent<Plants>());
                if (hc.GetComponentInParent<Plants>() &&
                    !(cc.GetInstanceID().Equals(hc.GetInstanceID())) 
                    && (hc.GetInstanceID() != 0))
                {
                    if (CheckIsCompatible(plante))
                    {
                        _completscore++;
                    }

                    if (plante.GetCard()._isRepellent)
                    {
                        AddRepellentAround();
                    }
                }

               
            }

        }
    }

    private void CheckInfestation()
    {
        CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
        Collider[] hits = Physics.OverlapSphere(_transform.position, _radiusDetection);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].GetComponent<CapsuleCollider>())
            {
                CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
                Plants plante = (hits[i].GetComponentInParent<Plants>());
                if (hc.GetComponentInParent<Plants>() &&
                    !(cc.GetInstanceID().Equals(hc.GetInstanceID()))
                    && (hc.GetInstanceID() != 0))
                {

                    if (plante.GetInfested() && _card._isRepellent && _canBeInfested)
                    {
                        plante.setInfested(false);
                        plante.SetcanBeInfested(false);
                        plante.AddRepellentAround();
                    }
                    if (_card._isRepellent)
                    {
                        AddRepellentAround();
                       
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
            FindObjectOfType<GameManager>().AddWateringTime();
            _timewhenLastwatered = Time.time;
        }
        if (go.GetComponent<WaterCan>() && (Time.time - _timewhenLastwatered > _waterCanCooldown))
        {
 
            _MultiplyWatered++;
            _timewhenLastwatered = Time.time;
            FindObjectOfType<GameManager>().AddWateringTime();
        }
        if(go.GetComponent<Pollinator>() && (Time.time - _timeWhenLastPollinisation > _pollinisationCooldown))
        {
            _multiplyPollinisation++;
            _timeWhenLastPollinisation = Time.time;
        }
    }

    public void DeleteTier()
    {
        _gp.SetCurrentTier(_gp.GetCurrentTier() - _step);
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
                Plants plante = hc.GetComponentInParent<Plants>();
                if ( plante&&!(cc.GetInstanceID().Equals(hc.GetInstanceID()))&& (hc.GetInstanceID() != 0))
                {
                    plante.AddOnUsedCardList(_card);
                    if (plante.GetRepellentAround() > 0)
                    {
                        plante.DeleteRepellentAround();
                    }
                   
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

    #region Getter and Setter

    public string GetName()
    {
        return _name;
    }

    public float GetTotalGrowTime()
    {
        return _TotalGrowTime;
    }

    public int GetRepellentAround()
    {
        return _repellentAround;
    }

    public void AddRepellentAround()
    {
        _repellentAround++;
    }
    public void DeleteRepellentAround()
    {
        _repellentAround--;
    }

    public CardScriptable GetCard()
    {
        return _card;
    }

    public int getBonusMalus()
    {
        return _HarvestBonus;
    }

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

    public bool GetInfested()
    {
        return _isInfested;
    }

    public void setInfested(bool b)
    {
        if (!b)
        {
           FindObjectOfType<GameManager>().AddCurrentHealedPlant();
        }
        _isInfested = b;
    }

    public bool GetcanBeInfested()
    {
        return _canBeInfested;
    }

    public void SetcanBeInfested(bool b)
    {
        _canBeInfested = b;
    }

    public void AddBonusScore()
    {
        FindObjectOfType<GameManager>().GetCurrentScore().Value += _pollinisationWateredPoint;
    }
    public bool CanBeWatered()
    {
        if(_MultiplyWatered == 0)
        {
            return true;
        }
        return (Time.time - _timewhenLastwatered > _waterCanCooldown);
    }

    public bool CanBePollinisate()
    {
        if (_MultiplyWatered == 0)
        {
            return true;
        }
        return (Time.time - _timeWhenLastPollinisation > _pollinisationCooldown);
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
