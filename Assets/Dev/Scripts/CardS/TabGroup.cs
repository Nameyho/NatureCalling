using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class TabGroup : MonoBehaviour
{

    #region Exposed

    public List<Tab> _tabButtons;

    public Sprite _tabIdle;
    public Sprite _tabHover;
    public Sprite _tabActive;
    public Tab _selectedtab;

    [SerializeField]
    private SubMenuScriptable _currentCards;

    [SerializeField]
    private HandManager _handmanager;

    [SerializeField]
    private GameObject _hand;
    #endregion

    #region Public Methods
    public void Subscribe(Tab button)
    {
        if(_tabButtons == null)
        {
            _tabButtons = new List<Tab>();
        }

        _tabButtons.Add(button);
    }

    public void OnTabEnter (Tab button)
    {
        ResetTabs();
        if(_selectedtab == null || button != _selectedtab)
        {
            button.background.sprite = _tabHover;
        }
    }

    public void OnTabExit(Tab button)
    {
        ResetTabs();

    }

    public void OnTabSelected(Tab button, List<CardScriptable> cardScriptable )
    {
        _hand.SetActive(true);
        _selectedtab = button;
        ResetTabs();
        _handmanager.resetMinimum();
        button.background.sprite = _tabActive;
        _currentCards._listSubMenu = cardScriptable;
        //_handmanager.resetMinimum();
        _handmanager.ChangeHand();

    }

    public void ResetTabs()
    {
        foreach (Tab button in _tabButtons)
        {
            if(_selectedtab != null && button == _selectedtab )
            {
                continue;
            }
            button.background.sprite = _tabIdle;
        }
    }
    #endregion

}
