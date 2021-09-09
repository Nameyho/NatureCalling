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
          
            other.GetComponentInParent<Plants>().AddTier(this.gameObject);
            Destroy(transform.parent.gameObject, 0.5f);
        }
       
            //other.GetComponent<Plants>().AddTier();
        
    }

    #endregion
}
