using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlantTab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    #region Exposed

    [SerializeField]
    private GameObject subMenu;

    public Sprite _tabIdle;
    public Sprite _tabHover;

    public Image _background;

    [SerializeField]
    private SubMenuScriptable subMenuScriptable;

    #endregion


    public void OnPointerClick(PointerEventData eventData)
    {
        subMenu.SetActive(true);
        List<CardScriptable> empty = new List<CardScriptable>(0);
        subMenuScriptable._listSubMenu = empty;
        FindObjectOfType<HandManager>().ChangeHand();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _background.sprite = _tabHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _background.sprite = _tabIdle;
    }

    public void CloseSubMenu()
    {
        subMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && subMenu.activeSelf)
        {

            FindObjectOfType<PlantsManager>().DisableAllvfx();
            subMenu.SetActive(false);
        
        }
    }


 
    
}
