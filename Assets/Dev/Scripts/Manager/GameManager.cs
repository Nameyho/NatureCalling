using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
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


    [SerializeField]
    private float[] _layeringMax;


    [Header("Menu de pause")]
    [SerializeField]
    private GameObject _menuOptionPause;

    #endregion
    #region Private
    private float[] _lastTime;

    private float[] _privateLayeringMax;

    private bool _isPaused = false;
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

    public void SetGameOnPause()
    {
        if (_isPaused)
        {
        Time.timeScale = 1;
            _isPaused = false;
            _menuOptionPause.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _isPaused = true;
            _menuOptionPause.SetActive(true);
        }
    }


    public void ReloadScene()
    {
        string scene = SceneManager.GetActiveScene().name;
        Time.timeScale = 1;
       
        SceneManager.UnloadSceneAsync(scene,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.LoadScene(scene,LoadSceneMode.Single);
        
        
    }
    #region Unity API

    private void Awake()
    {
        _currentScore.Value = 0;
        if (cardScriptables.Count == cardsIntvariable.Count)
        {
            for (int i = 0; i < cardScriptables.Count; i++)
            {
                cardsIntvariable[i].Value = cardScriptables[i]._numberCardsAtStart;
            }
        }
        else
        {
            Debug.Log("pas le même nombre de cartes");
        }

        if ((_layeringCard.Length == _layeringTime.Length) && (_layeringMax.Length == _layeringTime.Length))
        {
            _lastTime = new float[_layeringTime.Length];

            _privateLayeringMax = _layeringMax;

            for (int i = 0; i < _privateLayeringMax.Length; i++)
            {
                _privateLayeringMax[i] -= _layeringCard[i].Value;
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
        for (int i = 0; i < _layeringTime.Length; i++)
        {
            if (Time.time - _lastTime[i] > _layeringTime[i] && (_privateLayeringMax[i] > 0))
            {

                _layeringCard[i].Value++;
                _lastTime[i] = Time.time;
                _privateLayeringMax[i]--;

            }
        }
    }

    #endregion

}
