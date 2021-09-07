using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class GrowPlants : MonoBehaviour
{
    #region Exposed

    [Header("Model")]
    public List<MeshRenderer> _growPlantMeshes;

    [Header("ScriptableObject")]
    public PlantPhase _plantTier;

    [Header("Growing Parameters ")]
    public float _timeToGrow = 5;
    public float _RefreshRate = 0.05f;
    [Range(0,1)]
    public float _minGrow = 0.2f;
    [Range(0, 1)]
    public float _maxGrow = 0.97f;


    [Header("Details")]
    public GameObject[] _detailsPrefabs;

    [Header("Model Plant sans mesh")]
    public GameObject _plantModel;
    #endregion

    #region Private

    private List<Material> growPlantsMaterials = new List<Material>();
    private bool fullyGrown;
    private int currentTier = 0 ;
    private int _maxTier ;
    private int _maxTierDetail;
    private Transform _transform;

    #endregion

    #region Unity API


    private void Start()
    {
        _maxTier = _plantTier.PhaseAmount;
        _maxTierDetail = _plantTier.PhaseTodetail;
        _transform = GetComponent<Transform>();
        if(_growPlantMeshes.Count> 0)
        {
            for (int i = 0; i < _growPlantMeshes.Count; i++)
            {
                for (int j = 0; j < _growPlantMeshes[i].materials.Length; j++)
                {
                    if (_growPlantMeshes[i].materials[j].HasProperty("Grow_"))
                    {
                        _growPlantMeshes[i].materials[j].SetFloat("Grow_", _minGrow);
                        growPlantsMaterials.Add(_growPlantMeshes[i].materials[j]);
					
                    }
                }
            }

        }

    }

    private void Update()
    {
        float currentfloat = (float)currentTier / (float)_maxTier;
        

        if (currentfloat >= 1)
        {

            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;

        }
    }


    #endregion
    #region Methods

    IEnumerator GrowPlantsFunction( Material mat)
    {
       
        float growValue = mat.GetFloat("Grow_");

        if (!fullyGrown)
        {
            if (growValue < (_maxGrow /_maxTier) * currentTier)
            {
                while (growValue < (_maxGrow/ _maxTier) *currentTier)
                {
                
                    growValue += 1 / (_timeToGrow / _RefreshRate);
                    mat.SetFloat("Grow_", growValue);

                    if(currentTier>=  _maxTierDetail)
                    {
                        Vector3 VecMax = ((currentTier- _maxTierDetail)* (Vector3.one/(_maxTier -  _maxTierDetail)));
                        if (VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                        {
                            for (int i = 0; i < _detailsPrefabs.Length; i++)
                            {
                                _detailsPrefabs[i].gameObject.transform.localScale = Vector3.zero;

                            }
                        }

                        for (int i = 0; i < _detailsPrefabs.Length; i++)
                        {
                             if(VecMax.sqrMagnitude>= _detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude)
                            {
                               
                                if(currentTier != _maxTierDetail)
                                {
                                    if (_detailsPrefabs[i].transform.localScale.sqrMagnitude < Vector3.one.sqrMagnitude)
                                    {
                                        _detailsPrefabs[i].gameObject.transform.localScale += Vector3.one * (1f / (currentTier - _maxTierDetail) * _RefreshRate);

                                    }
                                }
                                else
                                {
                                    _detailsPrefabs[i].gameObject.transform.localScale += Vector3.one * (1f  * _RefreshRate);
                                }


                       
                            }
                        }

                    }
                    yield return new WaitForSeconds(_RefreshRate);
                }  
            }
            else if (growValue > ((_maxGrow / _maxTier) * currentTier))
                {
                while (growValue > (_maxGrow / _maxTier) * currentTier)
                {

                    growValue -= 1 / (_timeToGrow / _RefreshRate);
                    mat.SetFloat("Grow_", growValue);

                    if (currentTier >=  _maxTierDetail)
                    {
                        Vector3 VecMax = ((currentTier -  _maxTierDetail) * (Vector3.one / (_maxTier -  _maxTierDetail)));

                        if(VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                        {
                            for (int i = 0; i < _detailsPrefabs.Length; i++)
                            {
                                _detailsPrefabs[i].gameObject.transform.localScale = Vector3.zero;

                            }
                        }
                        for (int i = 0; i < _detailsPrefabs.Length; i++)
                        {
                            if(VecMax.sqrMagnitude <= _detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude && (_detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude >Vector3.zero.sqrMagnitude))
                            {
                                if (_detailsPrefabs[i].transform.localScale.sqrMagnitude < Vector3.one.sqrMagnitude)
                                {
                                    _detailsPrefabs[i].gameObject.transform.localScale += Vector3.one * (1f / (currentTier - _maxTierDetail) * _RefreshRate);

                                }

                            }

                        }
                    }


                    yield return new WaitForSeconds(_RefreshRate);


                }
            }

        }
        else
        {
           
            while (growValue >= (_maxGrow / _maxTier) * currentTier)
            {
               
                growValue -= 1 / (_timeToGrow / _RefreshRate);
                mat.SetFloat("Grow_", growValue);

                if (currentTier >=  _maxTierDetail)
                {
                    Vector3 VecMax = ((currentTier -  _maxTierDetail) * (Vector3.one / (_maxTier -  _maxTierDetail)));

                    if (VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                    {
                        for (int i = 0; i < _detailsPrefabs.Length; i++)
                        {
                            _detailsPrefabs[i].gameObject.transform.localScale = Vector3.zero;
                        }
                    }
                    for (int i = 0; i < _detailsPrefabs.Length; i++)
                    {
                    
                        if (VecMax.sqrMagnitude <= _detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude && (_detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude > Vector3.zero.sqrMagnitude))
                        {

                            float maxScale = Mathf.Clamp((currentTier - _maxTierDetail), 0, 1);
                            _detailsPrefabs[i].gameObject.transform.localScale -= Vector3.one * (1f / maxScale * _RefreshRate);
                        }

                    }
                }

                yield return new WaitForSeconds(_RefreshRate);
            }
        }

        if (growValue >= _maxGrow)
        {

            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;

        }
        
    }


    IEnumerator GrowScaleFunction()
    {
        float currentfloat = (float)currentTier / (float)_maxTier;
   
        if (_plantModel.transform.localScale.sqrMagnitude <= Vector3.one.sqrMagnitude)
        {
         _plantModel.transform.localScale += Vector3.one * ((2f / currentfloat)* _RefreshRate);

        }

        if (currentfloat >= 1)
        {

            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;

        }
        yield return new WaitForSeconds(_RefreshRate);


    }

    public void SetCurrentTier(int tier)
    {
        if (_maxTier < tier)
        {
            return;
        }
        if (!(currentTier == tier))
        {


            currentTier = tier;
            if (growPlantsMaterials.Count > 0)
            {
                for (int i = 0; i < growPlantsMaterials.Count; i++)
                {
                    StartCoroutine(GrowPlantsFunction(growPlantsMaterials[i]));

                }

            }
            else
            {
                StartCoroutine(GrowScaleFunction());
            }

        }


    }

    public int GetCurrentTier()
    {
        return currentTier;
    }

    public int GetMaxTier()
    {
        return _plantTier.PhaseAmount;


    }



    public bool isFullingGrown()
    {
        return fullyGrown;
    }

    public int  GetPhaseWhenHarvest()
    {
        return _plantTier.PhaseWhenHarvested;
    }

    public bool IsDestroyOnHarvest()
    {
        return _plantTier._DestroyOnHarvest;
    }
    public void RemoveMaxTier(int del)
    {
        if(_maxTier > currentTier)
        {
            _maxTier  -=  del;
            
        }
        else
        {
            currentTier = _plantTier.PhaseAmount;
        }

        if( _maxTierDetail> currentTier)
        {
            _maxTierDetail -= del;
        }


  

        
    }

    public void Harvest()
    {
        for (int i = 0; i < _detailsPrefabs.Length; i++)
        {
            _detailsPrefabs[i].gameObject.transform.localScale = Vector3.zero;
        }
    }
    

    //public void Growing()
    //{
    //    //if(!(currentTier == tier))
    //    //{


    //    //    currentTier = tier;
    //    //        for (int i = 0; i < growPlantsMaterials.Count; i++)
    //    //            {
    //    //                  StartCoroutine(GrowPlantsFunction(growPlantsMaterials[i]));

    //    //            }

    //    //}
    //    if (GetComponent<Plants>().IsWatered())
    //    {

    //         Debug.Log("arrosé et je grandis");
           
           
    //        for (int i = 0; i < growPlantsMaterials.Count; i++)
    //        {
    //            StartCoroutine(GrowPlantsFunction(growPlantsMaterials[i]));

    //        }

    //    }

    //}


    #endregion

}
