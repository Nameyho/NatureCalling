using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Beehive : MonoBehaviour
{
    [SerializeField]
    private IntVariable _hiveNumber;
    
    private void OnDestroy()
    {
        if (_hiveNumber >= 0)
        {
        _hiveNumber.Value++;

        }
    }
}
