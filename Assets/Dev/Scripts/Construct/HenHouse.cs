using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenHouse : MonoBehaviour
{
    #region Exposed

    [Header("Poule Modele")]
    [SerializeField]
    private GameObject _henPrefab;
    #endregion


    #region Unity API

    private void Awake()
    {
       GameObject hen =  Instantiate(_henPrefab, transform.position, transform.rotation);
        FindObjectOfType<GameManager>().SetHen(hen.GetComponent<Animal>());
    }
    #endregion
}
