using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButton : MonoBehaviour
{

    #region

    [SerializeField]
    private int step;


    [SerializeField]
    private HandManager _hand;
    #endregion


    #region Unity Methods

    private void OnMouseDown()
    {
        _hand.Navigate(step);  
    }

    #endregion
}
