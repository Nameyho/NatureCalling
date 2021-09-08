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


    [Header("Layering bonus à insérer dans le même ordre dans chaque liste")]
    [SerializeField]
    private IntVariable[] _layeringCard;

    [SerializeField]
    private float[] _layeringTime;


    private float[] _lastTime;

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


    #region Unity API

    private void Awake()
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

        if(_layeringCard.Length == _layeringTime.Length)
        {
            _lastTime = new float[_layeringTime.Length];
        }
        else
        {
            Debug.Log("pas le même nombre de cartes");
        }
    }

    private void Update()
    {
        AddLayering();
    }



    #endregion



    #region privates methods

    private void AddLayering()
    {
        for (int i = 0; i < _layeringTime.Length; i++)
        {
            if(Time.time - _lastTime[i]> _layeringTime[i])
            {
                _layeringCard[i].Value++;
                _lastTime[i] = Time.time;
            }
        }
    }

    #endregion

}
