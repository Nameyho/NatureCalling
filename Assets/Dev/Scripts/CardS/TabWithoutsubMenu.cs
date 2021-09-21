using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabWithoutsubMenu : MonoBehaviour,IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    #region Exposed

    [SerializeField]
    private GameObject subMenu;

    #endregion


    public void OnPointerClick(PointerEventData eventData)
    {
        subMenu.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }


}
