using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class HarvestBasket : MonoBehaviour
{

    #region Exposed
    [SerializeField]
    private IntVariable _score;

    #endregion

    #region Private

    #endregion

    #region public


    #endregion
    #region Unity API

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Plants" || other.tag== "HarvestAquatic" && other.GetComponentInParent<GrowPlants>())
        {
            GrowPlants gp = other.GetComponentInParent<GrowPlants>();
            Plants plant = other.GetComponentInParent<Plants>();
           
            if(gp.isFullingGrown()){
                if (gp.IsDestroyOnHarvest())
                {
                    //_score.Value -= other.GetComponentInParent<Plants>().getBonusMalus();
                    FindObjectOfType<PlantsManager>().DeletePlantInMapList(other.transform.parent.transform.gameObject);
                    Destroy(other.transform.parent.transform.gameObject);
                    plant.GetGroundLayering().DeletePlants();

                }
                else
                {
                    int phase = gp.GetPhaseWhenHarvest();
                    gp.SetCurrentTier(phase);
                    gp.Harvest();
                    plant.resetSpawnTime();
                    plant.ResetMultiply();
                }

                if (other.GetComponentInParent<FocusPlant>())
                {
                    other.GetComponentInParent<FocusPlant>().AddFocusPoints();

                }
                _score.Value += plant.getBonusMalus();
                if (plant.GetCard()._isAquaticPlant)
                {
                    FindObjectOfType<GameManager>().AddCurrentAquaticPlant();
                }

                if(plant.GetCard()._isAquaticPlant || plant.GetCard()._isPlant)
                {
                    FindObjectOfType<GameManager>().AddHarvestedPlant();
                }
            }



        }
           Destroy(transform.parent.gameObject, 0.5f);

        //other.GetComponent<Plants>().AddTier();

    }

    #endregion
}
