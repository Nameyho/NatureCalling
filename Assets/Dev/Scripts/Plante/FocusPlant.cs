using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPlant : MonoBehaviour
{
    #region Public

    public void AddFocusPoints()
    {
        FindObjectOfType<GameManager>().AddFocusPlants();
    }

    #endregion
}
