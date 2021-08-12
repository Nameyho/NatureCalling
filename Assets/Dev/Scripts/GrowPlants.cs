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
    public GameObject _detailsPrefab;
    #endregion

    #region Private

    private List<Material> growPlantsMaterials = new List<Material>();
    private bool fullyGrown;
    private int currentTier = 0 ;

    #endregion

    #region Unity API

    private void Awake()
    {
        
    }

    private void Start()
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

    private void Update()
    {


    }

    IEnumerator GrowPlantsFunction( Material mat)
    {
        float growValue = mat.GetFloat("Grow_");

        if (!fullyGrown)
        {
            if (growValue < (_maxGrow / _plantTier.PhaseAmount) * currentTier)
            {
                while (growValue < (_maxGrow/_plantTier.PhaseAmount)*currentTier)
                {
                
                    growValue += 1 / (_timeToGrow / _RefreshRate);
                    mat.SetFloat("Grow_", growValue);

                    if(currentTier>= _plantTier.PhaseTodetail)
                    {
                        Vector3 VecMax = ((currentTier-_plantTier.PhaseTodetail)* (Vector3.one/(_plantTier.PhaseAmount - _plantTier.PhaseTodetail)));
                        if (VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                        {
                            _detailsPrefab.gameObject.transform.localScale = Vector3.zero;
                        }
                        else if(VecMax.sqrMagnitude>= _detailsPrefab.gameObject.transform.localScale.sqrMagnitude)
                        {
                        _detailsPrefab.gameObject.transform.localScale += Vector3.one * (1f /( currentTier - _plantTier.PhaseTodetail)*_RefreshRate);
                        }
                    }
                    yield return new WaitForSeconds(_RefreshRate);
                }  
            }
            else if (growValue > ((_maxGrow / _plantTier.PhaseAmount) * currentTier))
                {
                while (growValue > (_maxGrow / _plantTier.PhaseAmount) * currentTier)
                {

                    growValue -= 1 / (_timeToGrow / _RefreshRate);
                    mat.SetFloat("Grow_", growValue);

                    if (currentTier >= _plantTier.PhaseTodetail)
                    {
                        Vector3 VecMax = ((currentTier - _plantTier.PhaseTodetail) * (Vector3.one / (_plantTier.PhaseAmount - _plantTier.PhaseTodetail)));

                        if(VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                        {
                            _detailsPrefab.gameObject.transform.localScale = Vector3.zero;
                        }else if (VecMax.sqrMagnitude <= _detailsPrefab.gameObject.transform.localScale.sqrMagnitude)
                        {
                           
                            _detailsPrefab.gameObject.transform.localScale -= Vector3.one * (1f / (currentTier - _plantTier.PhaseTodetail) * _RefreshRate);
                        }
                    }


                    yield return new WaitForSeconds(_RefreshRate);


                }
            }

        }
        else
        {
           
            while (growValue >= (_maxGrow / _plantTier.PhaseAmount) * currentTier)
            {
               
                growValue -= 1 / (_timeToGrow / _RefreshRate);
                mat.SetFloat("Grow_", growValue);

                if (currentTier >= _plantTier.PhaseTodetail)
                {
                    Vector3 VecMax = ((currentTier - _plantTier.PhaseTodetail) * (Vector3.one / (_plantTier.PhaseAmount - _plantTier.PhaseTodetail)));

                    if (VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                    {
                        _detailsPrefab.gameObject.transform.localScale = Vector3.zero;
                    }
                    else if (VecMax.sqrMagnitude <= _detailsPrefab.gameObject.transform.localScale.sqrMagnitude)
                    {
                        
                        _detailsPrefab.gameObject.transform.localScale -= Vector3.one * (1f / (currentTier - _plantTier.PhaseTodetail) * _RefreshRate);
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


    public void SetCurrentTier(int tier)
    {
        if(!(currentTier == tier))
        {
           

            currentTier = tier;
                for (int i = 0; i < growPlantsMaterials.Count; i++)
                    {
                          StartCoroutine(GrowPlantsFunction(growPlantsMaterials[i]));

                    }

        }
        
    }
    #endregion

}
