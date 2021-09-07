using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GameManager: MonoBehaviour
{

    #region Exposed

    [Header("Progression")]

    [SerializeField]
    private IntVariable _currentScore;

    [SerializeField]
    private int _scoreToChangeScene;

    [Header("Valeur de départ à insérer dans le même ordre dans chaque liste")]
    [SerializeField]
    private List<CardScriptable> cardScriptables = new List<CardScriptable>();

    [SerializeField]
    private List<IntVariable> cardsIntvariable = new List<IntVariable>();


    #endregion

    #region Public Methods

    public void AddProgression(int score)
    {
        _currentScore.Value += score;
    }

    public void DeleteProgression(int score)
    {
        _currentScore.Value -= score;
    }
    #endregion

    public void CheckMinimalProgression()
    {
        if (_scoreToChangeScene < _currentScore.Value)
        {
        //bravo tu peux aller au niveau suivant si tu veux
        }
    }

    public void Awake()
    {
        _currentScore.Value = 0;
        if(cardScriptables.Count == cardsIntvariable.Count)
        {
            for (int i = 0; i < cardScriptables.Count; i++)
            {
                cardsIntvariable[i].Value = cardScriptables[i]._usingLimitation;
            }
        }
        else
        {
            Debug.Log("pas le même nombre de cartes");
        }
    }

    


}
