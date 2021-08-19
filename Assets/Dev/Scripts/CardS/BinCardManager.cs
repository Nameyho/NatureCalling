using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinCardManager : MonoBehaviour
{
    #region
        private void OnTriggerEnter(Collider other)
        {
        if (other.GetComponent<ButtonScriptData>())
        {
            other.GetComponent<ButtonScriptData>().DestroyThisCard();

        }
    }

    #endregion
}
