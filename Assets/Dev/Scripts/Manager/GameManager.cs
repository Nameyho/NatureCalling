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

    [Header("Valeur de départ à insérer dans le même ordre dans chaque liste")]
    //[SerializeField]
    //private List<CardScriptable> cardScriptables = new List<CardScriptable>();

    [SerializeField]
    private int[] _startValue;

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

    [SerializeField]
    private GameObject _mainMenu;

    [SerializeField]
    private GameObject _restartMenu;

    [SerializeField]
    private GameObject _QuitMenu;

    [SerializeField]
    private GameObject _BackToMenu;

    [SerializeField]
    private GameObject _optionMenu;

    [SerializeField]
    private SceneVariable _MenuScene;


    [Header("Conditions de victoire")]
    [SerializeField]
    private int _focusPlantMax;

    [SerializeField]
    private int _nombreArrosage;

    [SerializeField]
    private int _scoreToChangeScene;

    [SerializeField]
    private int _harvestAquaticPlantsToReach;

    [SerializeField]
    private int _plantToHeal;

    [SerializeField]
    private int _NombreDePlantARecolter;

    [SerializeField]
    private BoolVariable[] _animals;



    #endregion
    #region Private
    private float[] _lastTime ;

    private float[] _privateLayeringMax;

    private bool _isPaused = false;

    private int _focusPlants;

    private int _wateringTime;

    private int _harvestAquaticPlants;

    private int _currentPlantHealed;

    private int _totalHarvestedPlant;
    #endregion


    #region Unity API

    private void Start()
    {
        Reset();
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneUnloaded;
    }
    private void Update()
    {
        AddLayering();
        CheckIfVictory();
    }




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
            ResumeGame();
        }
        else
        {
            Time.timeScale = 0;
            _isPaused = true;
            _menuOptionPause.SetActive(true);
            
        }
    }


    public void ReloadButton()
    {
        _mainMenu.SetActive(false);
        _restartMenu.SetActive(true);
    }

    public void QuitButton()
    {
        _mainMenu.SetActive(false);
        _QuitMenu.SetActive(true);
    }

    public void MenuButton()
    {
        _mainMenu.SetActive(false);
        _BackToMenu.SetActive(true);
        
    }

    public void OptionsButton()
    {
        _mainMenu.SetActive(false);
        _optionMenu.SetActive(true);
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    public void CloseOptionMenu()
    {
        _BackToMenu.SetActive(false);
        _optionMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void CloseBackToMenu()
    {
        _BackToMenu.SetActive(false);
        _restartMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void CloseQuitMenu()
    {
        _QuitMenu.SetActive(false);
        _restartMenu.SetActive(false);
        _mainMenu.SetActive(true);
        ResumeGame();
    }
    public void CloseReloadMenu()
    {
        _mainMenu.SetActive(true);
        _restartMenu.SetActive(false);
        ResumeGame();
        
    }
    public void ReloadScene()
    {
        string scene = SceneManager.GetActiveScene().name;
        Time.timeScale = 1;
        //Reset();

        //SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);


    }

    public void BackToMenu()
    {
       // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.LoadScene(_MenuScene.Value.SceneName, LoadSceneMode.Single);
    }


    public void Reset()
    {
        _currentScore.Value = 0;
        if (_startValue.Length == cardsIntvariable.Count)
        {
            for (int i = 0; i < _startValue.Length; i++)
            {
                cardsIntvariable[i].Value = _startValue[i];

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



    public IntVariable GetCurrentScore()
    {
        return _currentScore;
    }

    public void AddFocusPlants()
    {
        _focusPlants++;
    }

    public void AddWateringTime()
    {
        _wateringTime++;
    }

    public void AddCurrentAquaticPlant()
    {
        _harvestAquaticPlants++;
    }

    public void AddCurrentHealedPlant()
    {
        _currentPlantHealed++;
    }

    public void AddHarvestedPlant()
    {
        _totalHarvestedPlant++;
    }


    private void OnSceneUnloaded(Scene scene, LoadSceneMode mod)
    {
        
        //Reset();
    }
    
    #endregion




    #region privates methods

    private void AddLayering()
    {
        for (int i = 0; i < _layeringTime.Length; i++)
        {
            if (Time.timeSinceLevelLoad - _lastTime[i] > _layeringTime[i] && (_privateLayeringMax[i] > 0))
            {

                _layeringCard[i].Value++;
                _lastTime[i] = Time.timeSinceLevelLoad;
                _privateLayeringMax[i]--;

            }
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        _isPaused = false;
        _menuOptionPause.SetActive(false);
    }

    private void CheckIfVictory()
    {


        bool vivace = _focusPlants >= _focusPlantMax;
        bool arrosage = _wateringTime >= _nombreArrosage;
        bool points = _currentScore >= _scoreToChangeScene;
        bool aquaticPlants = _harvestAquaticPlants >= _harvestAquaticPlantsToReach;
        bool healedPlants = _currentPlantHealed >= _plantToHeal;
        bool recoltedplant = _totalHarvestedPlant >= _NombreDePlantARecolter;
        bool a = true;
        for (int i = 0; i < _animals.Length; i++)
        {
            if (!_animals[i])
            {
                a = false;
            }
        }


        if(vivace && arrosage && points && aquaticPlants && recoltedplant & a)
        {
            Debug.Log("win");
        }
    }
    #endregion

}
