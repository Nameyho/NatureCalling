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
    private ObjectVariable _currentPrefabSelected;

    [SerializeField]
    private IntVariable _currentEnergy;

    [SerializeField]
    private IntVariable _numbersCardsInHand;

    [SerializeField]
    private Transform[] _spawnLocation;

    [SerializeField]
    private TMP_Text _tmp;
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
        UpdateDeckText();
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


    public void DestoyCard(CardScriptable card, GameObject go)
    {
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
       

        

        for (int i = 0; i < _spawnLocation.Length; i++)
        {
            int rand = Random.Range(0, _DeckList.Count);
            if (_spawnLocation[i].transform.childCount <= 0)
            {
               GameObject go =  Instantiate(_DeckList[rand]._cardModel, _spawnLocation[i].transform);
               _numbersCardsInHand.Value++;
                go.GetComponentInChildren<ButtonScriptData>().SetCardScriptable(_DeckList[rand]);
                 _cardsinHands.Add(_DeckList[rand]);
                _DeckList.Remove(_DeckList[rand]);
            }

        }


        

    }


    public void UpdateDeckText()
    {
        _tmp.text = " Il reste " + _DeckList.Count + " dans le paquet";
    }
    #endregion


}
