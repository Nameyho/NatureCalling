using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class DeckManager : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private List<CardScriptable> _DeckList = new List<CardScriptable>();

    [SerializeField]
    private List<CardScriptable> _cardsinHands = new List<CardScriptable>();

    [SerializeField]
    private Button _buttonModel;

    [SerializeField]
    private ObjectVariable _currentPrefabSelected;

    [SerializeField]
    private IntVariable _currentEnergy;

    [SerializeField]
    private IntVariable _numbersCardsInHand;
    #endregion

    #region Private

    private Transform _transform;



    #endregion

    #region Unity API

    private void Awake()
    {
        _numbersCardsInHand.Value = 0;
        SpawnCardHand();
    }

    private void Update()
    {
        CompleteHand();
    }

    #endregion


    #region Public Methods 


    public void SpawnCardHand()
    {
        for (int i = 0; i < 5; i++)
        {
            AddACart();
        }
    }

    public void PlayCard(CardScriptable card,GameObject go) 
    {
        Debug.Log("je joue la carte " + card.name);

        if (card._prefabToSpawn)
        {
            _currentPrefabSelected.Value = card._prefabToSpawn;
        }
        else
        {
            Debug.Log("rien à instancier");
        }

        _currentEnergy.Value -= card._cost;

        _cardsinHands.Remove(card);

        Destroy(go);
        _numbersCardsInHand.Value--;


    }

    public void CompleteHand()
    {
        if((_numbersCardsInHand.Value < 5)&&(_DeckList.Count> 0))
        {

            AddACart();
        }
    }

    public void AddACart()
    {
        int rand = Random.Range(0, _DeckList.Count);

        Button but = Instantiate(_buttonModel, _transform);
        _numbersCardsInHand.Value++;
        but.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        but.GetComponent<Image>().sprite = _DeckList[rand]._CardImage;
        but.GetComponentInChildren<TMP_Text>().text = _DeckList[rand]._CardName;
        but.GetComponentInChildren<ButtonScriptData>().SetCardScriptable(_DeckList[rand]);

        _cardsinHands.Add(_DeckList[rand]);
        _DeckList.Remove(_DeckList[rand]);
    }

    #endregion


}
