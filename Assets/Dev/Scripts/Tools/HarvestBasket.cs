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
    #region Unity API

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Plants" && other.GetComponentInParent<GrowPlants>())
        {
            GrowPlants gp = other.GetComponentInParent<GrowPlants>();
            Plants plant = other.GetComponentInParent<Plants>();
            if(gp.isFullingGrown()){
                if (gp.IsDestroyOnHarvest())
                {
                    //_score.Value -= other.GetComponentInParent<Plants>().getBonusMalus();
                    Destroy(other.transform.parent.transform.gameObject);
                }
                else
                {
                    int phase = gp.GetPhaseWhenHarvest();
                    gp.SetCurrentTier(phase);
                    gp.Harvest();
                    plant.resetSpawnTime();
                    plant.ResetMultiply();
                }

            }
           
        }
           Destroy(transform.parent.gameObject, 0.5f);

        //other.GetComponent<Plants>().AddTier();

    }

    #endregion
}
