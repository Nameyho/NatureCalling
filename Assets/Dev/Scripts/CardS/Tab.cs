using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Tab : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{

    #region Exposed
    [SerializeField]
    private TabGroup _tabGroup;

    public Image background;

    [SerializeField]
    public List<CardScriptable> _tabCardsScriptable;
    #endregion

    #region Interfaces Methods
    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this,_tabCardsScriptable);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit(this);
    }
    #endregion

    #region Unity API

    private void Start()
    {
        background = GetComponent<Image>();
        _tabGroup.Subscribe(this);
    }
    #endregion
}
