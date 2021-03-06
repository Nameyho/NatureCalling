using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollinator : MonoBehaviour
{


    #region Unity API

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Plants" && other.GetComponentInParent<Plants>())
        {

            other.GetComponentInParent<Plants>().AddTier(this.gameObject);
            other.GetComponentInParent<Plants>().AddBonusScore();
            FindObjectOfType<GameManager>().AddPollinisedtotal();
            Destroy(transform.parent.gameObject);
        }

        //other.GetComponent<Plants>().AddTier();

    }

    #endregion
}