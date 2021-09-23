using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPlant : MonoBehaviour
{
    #region Public

    [SerializeField]
    GameObject _nom;

    int harvesttime = 0 ;

    public void AddFocusPoints()
    {
        FindObjectOfType<GameManager>().AddFocusPlants();
        harvesttime++;

        if (harvesttime < 1)
        {
            _nom.SetActive(true);
        }
        else
        {
            _nom.SetActive(false);
        }
    }



    #endregion
}
