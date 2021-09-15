using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCan : MonoBehaviour
{

    #region Exposed



    #endregion

    #region Unity API

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.tag == "Plants" && other.GetComponentInParent<Plants>())
        {
            Plants plante = other.GetComponentInParent<Plants>();
            plante.AddTier(this.gameObject);
            plante.AddBonusScore();
          
            Destroy(transform.parent.gameObject, 0.5f);
        }
       
            //other.GetComponent<Plants>().AddTier();
        
    }

    #endregion
}
