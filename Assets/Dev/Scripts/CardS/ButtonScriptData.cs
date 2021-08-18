using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScriptData : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private CardScriptable card;


    public void SetCardScriptable(CardScriptable cs)
    {
        card = cs;
    }

    public CardScriptable GetCardScriptable()
    {
        return card;
    }

    public void PlayThisCard()
    {
        GetComponentInParent<DeckManager>().PlayCard(card,this.gameObject);
    }

    #endregion
}
