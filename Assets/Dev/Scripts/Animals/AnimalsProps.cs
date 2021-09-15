using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AnimalsProps : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private BoolVariable[] _boolsVariables;

    #endregion


    #region UNITY API

    private void Awake()
    {
        for (int i = 0; i < _boolsVariables.Length; i++)
        {
            _boolsVariables[i].Value = true;
        }
        
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _boolsVariables.Length; i++)
        {
            _boolsVariables[i].Value = false;
        }
    }

    #endregion


}
