using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Building : MonoBehaviour
{
    #region

    [SerializeField]
    private CardScriptable _score;

    #endregion

    #region Mains

    public int GetBonusDiversity()
    {
        return _score._bonusBioDiversity;
    }
    #endregion

}
