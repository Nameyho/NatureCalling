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
    private IntVariable[] _numbersOfCardAtBeginning;

    [SerializeField]
    private float[] _layeringAddEveryXSecond;


    [SerializeField]
    private float[] _MaxCardLayering;

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

    private float[] _privateLayeringMax;

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

        if ((_numbersOfCardAtBeginning.Length == _layeringAddEveryXSecond.Length)  && (_MaxCardLayering.Length == _layeringAddEveryXSecond.Length))
        {
            _lastTime = new float[_layeringAddEveryXSecond.Length];
        
            _privateLayeringMax = _MaxCardLayering;

            for (int i = 0; i < _privateLayeringMax.Length; i++)
            {
                _privateLayeringMax[i] -= _numbersOfCardAtBeginning[i].Value;
            }
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
        for (int i = 0; i < _layeringAddEveryXSecond.Length; i++)
        {
            if(Time.time - _lastTime[i]> _layeringAddEveryXSecond[i] && (_privateLayeringMax[i]>0))
            {
                Debug.Log(_privateLayeringMax[i]);
                _numbersOfCardAtBeginning[i].Value++;
                _lastTime[i] = Time.time;
                _privateLayeringMax[i]--;
                
            }
        }
    }

    #endregion

}
