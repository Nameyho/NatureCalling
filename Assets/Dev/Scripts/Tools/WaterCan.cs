using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCan : MonoBehaviour
{
    #region private

    private MeshCollider _meshCollider;

    #endregion


    #region Unity API
    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag== "Plants")
        {
            Debug.Log(other);
            //other.GetComponent<Plants>().AddTier();
        }
    }
    #endregion
}
