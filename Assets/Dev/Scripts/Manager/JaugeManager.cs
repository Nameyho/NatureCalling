using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class JaugeManager : MonoBehaviour
{
    [SerializeField]
    IntVariable currentScore;

    private int _maxTier;
    private int _maxTierDetail;
    private int currentTier = 0;
    public  float _RefreshRate;

    [Header("Details")]
    public GameObject[] _detailsPrefabs;

    [Header("Model")]
    public List<MeshRenderer> _growPlantMeshes;

    private List<Material> growPlantsMaterials = new List<Material>();

    [Range(0, 1)]
    public float _minGrow = 0.2f;

    [Header("ScriptableObject")]
    public PlantPhase _plantTier;

    private void Update()
    {

        
            SetCurrentTier(currentScore.Value);
          
        
    }

    private void Start()
    {

        _maxTier = _plantTier.PhaseAmount;
        _maxTierDetail = _plantTier.PhaseTodetail;
        if (_growPlantMeshes.Count > 0)
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

        public void SetCurrentTier(int tier)
        {
        if (_maxTier < tier)
        {
            currentTier = _maxTier;
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
        }
        }

        IEnumerator GrowPlantsFunction(Material mat)
        {
        float growValue = mat.GetFloat("Grow_");

        float test = ((float)currentScore.Value/(float) _maxTier);

        while (growValue <test)
        {
           
            Debug.Log(test);
            mat.SetFloat("Grow_", test);

            if (currentTier >= _maxTierDetail)
            {
                Vector3 VecMax = ((currentTier - _maxTierDetail) * (Vector3.one / (_maxTier - _maxTierDetail)));
                if (VecMax.sqrMagnitude == Vector3.zero.sqrMagnitude)
                {
                    for (int i = 0; i < _detailsPrefabs.Length; i++)
                    {
                        _detailsPrefabs[i].gameObject.transform.localScale = Vector3.zero;

                    }
                }

                for (int i = 0; i < _detailsPrefabs.Length; i++)
                {
                    if (VecMax.sqrMagnitude >= _detailsPrefabs[i].gameObject.transform.localScale.sqrMagnitude)
                    {

                        if (currentTier != _maxTierDetail)
                        {
                            if (_detailsPrefabs[i].transform.localScale.sqrMagnitude < Vector3.one.sqrMagnitude)
                            {
                                _detailsPrefabs[i].gameObject.transform.localScale += Vector3.one * (1f / (currentTier - _maxTierDetail) * _RefreshRate);

                            }
                        }
                        else
                        {
                            _detailsPrefabs[i].gameObject.transform.localScale += Vector3.one * (1f * _RefreshRate);
                        }



                    }
                }

            }
            yield return new WaitForSeconds(_RefreshRate);
        }


    }
    }
