using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cards : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private CardScriptable card;

    [SerializeField]
    private GameObject _cardGO;

    [SerializeField]
    private GameObject _GhostCard;

    [SerializeField]
    private TMP_Text _name;


    #endregion

    #region Private

    private Transform _transform;

    #endregion

    #region Utils
    public void SetCardScriptable(CardScriptable cs)
    {
        card = cs;
    }

    public CardScriptable GetCardScriptable()
    {
        return card;
    }

    //public void PlayThisCard()
    //{
    //    GetComponentInParent<HandManager>().PlayCard(card,this.gameObject);
    //}

    public void TransformIntoGhostModel()
    {
        _cardGO.SetActive(false);
        _GhostCard.SetActive(true);
        _GhostCard.transform.Rotate(-45, 0, 0);
        
    }

    public int GetBonusDiversity()
    {
        return card._bonusBioDiversity;
    }
    public void TransformIntoCard()
    {
        _cardGO.SetActive(true);
        _GhostCard.SetActive(false);
    }
    #endregion

    #region Unity API

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _name.text = card._CardName;
    }


    #endregion
}
