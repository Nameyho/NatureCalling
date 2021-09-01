using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollinator : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private int _durability;
    #endregion

    #region Unity API

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Plants" && other.GetComponentInParent<Plants>())
        {

            other.GetComponentInParent<Plants>().AddTier(_durability,this.gameObject);
            Destroy(transform.parent.gameObject, 0.5f);
        }

        //other.GetComponent<Plants>().AddTier();

    }

    #endregion
}