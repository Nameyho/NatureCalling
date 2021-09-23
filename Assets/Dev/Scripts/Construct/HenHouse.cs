using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class HenHouse : MonoBehaviour
{
    #region Exposed

    [Header("Poule Modele")]
    [SerializeField]
    private GameObject _henPrefab;

  
    [SerializeField]
    private IntVariable HenHouseCardLimitation;
    #endregion


    #region Unity API

    private void Awake()
    {
       GameObject hen =  Instantiate(_henPrefab, transform.position, transform.rotation);
        FindObjectOfType<GameManager>().SetHen(hen.GetComponent<Animal>());
    }


    private void OnDestroy()
    {
        HenHouseCardLimitation.Value++; 
    }
    #endregion
}
