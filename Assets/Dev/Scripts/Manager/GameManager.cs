using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;
using TMPro;
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

    [SerializeField]
    private GameObject _winMenu;


    [Header("Affichage")]
    [SerializeField]
    private TMP_Text[] objectifs;




    #endregion
    #region Private
    private bool _isBeeHive = false;
    private int _numberofBeehive = 0;

    private bool _isHenHouse = false;
    private int _numberHenHouse;

    private float[] _lastTime;

    private float[] _privateLayeringMax;

    private bool _isPaused = false;

    private int _focusPlants;

    private int _wateringTime;

    private int _harvestAquaticPlants;

    private int _currentPlantHealed;

    private int _totalHarvestedPlant;

    private bool _winmenu = true;

    private GameObject _button;

    private Animal _hen;

    private FlipCard _flippedCard;

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


    public int GetHenHouseNumber()
    {
        return _numberHenHouse;
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

    public void SetGameOnPause(GameObject go) 
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
            //go.SetActive(false);
            _button = go;
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
        _button.SetActive(true);
    }

    public void CloseBackToMenu()
    {
        _BackToMenu.SetActive(false);
        _restartMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _button.SetActive(true);
    }
    public void CloseQuitMenu()
    {
        _QuitMenu.SetActive(false);
        _restartMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _button.SetActive(true);
        
    }
    public void CloseReloadMenu()
    {
        _mainMenu.SetActive(true);
        _restartMenu.SetActive(false);
        _button.SetActive(true);
       

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

    public void CloseWinMenu()
    {
  
        _winMenu.SetActive(false);
        Time.timeScale = 1;
        _winmenu = false;
    }

    public void MoveToNextScene(string i)
    {
        SceneManager.LoadScene(i);
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

    public bool GetIsHive()
    {
        return _isBeeHive;
    }
    
    public void SetisHive(bool b)
    {

        if (!b)
        {
            _numberofBeehive--;
            if (_numberofBeehive>0)
            {
                _isBeeHive = true;
            }
            else
            {
                _isBeeHive = false;
            }
        }
        else
        {
                _isBeeHive = true;
                _numberofBeehive++;
            }
        
    }

    public bool GetisHenHouse()
    {
        return _isHenHouse;
    }

    public void SetisHenHouse(bool b)
    {

        if (!b)
        {
            _numberHenHouse--;
            if (_numberHenHouse > 0)
            {
                _isHenHouse = true;
            }
            else
            {
                _isHenHouse = false;
            }
        }
        else
        {
            _isHenHouse = true;
            _numberHenHouse++;
        }

    }

    public void SetHen(Animal hen)
    {
        _hen = hen;
    }

    public Animal getHen()
    {
        return _hen;
    }

    public void setFlippedCard(FlipCard fc)
    {
        _flippedCard = fc;
    }


    public FlipCard GetFlipped()
    {
        return _flippedCard;
    }
    #endregion




    #region privates methods
    private void OnSceneUnloaded(Scene scene, LoadSceneMode mod)
    {
        
        //Reset();
    }


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

        int currentIndex = 0;

        //focus plants
        if (_focusPlantMax > 0)
        {
            if (_focusPlants < _focusPlantMax)
            {
                objectifs[currentIndex].text = "Cucumber to harvest   " +  _focusPlants + " / " + _focusPlantMax ;

            }
            if(_focusPlants >= _focusPlantMax)
            {
                objectifs[currentIndex].text = "Cucumber to harvest   " + _focusPlantMax + " / " + _focusPlantMax;
                objectifs[currentIndex].color = new Color(0, 255, 0);
            }
            currentIndex++;
        }
        bool vivace = _focusPlants >= _focusPlantMax;


        //arrossage
        if (_nombreArrosage > 0)
        {
            if(_wateringTime< _nombreArrosage)
            {
                objectifs[currentIndex].text = "Plants to water   " +  _wateringTime + " / " + _nombreArrosage;
            }
            if (_wateringTime >= _nombreArrosage)
            {
                objectifs[currentIndex].text = "Plants to water   " + _nombreArrosage + " / " + _nombreArrosage;
                objectifs[currentIndex].color = new Color(0, 255, 0);
            }

            currentIndex++;
         }
        bool arrosage = _wateringTime >= _nombreArrosage;


        //score
        if (_scoreToChangeScene > 0)
        {
            if(_currentScore< _scoreToChangeScene)
            {
                objectifs[currentIndex].text = "Score to reach   " +  _currentScore + " / " + _scoreToChangeScene;
            }
            if (_currentScore >= _scoreToChangeScene)
            {
                objectifs[currentIndex].color = new Color(0, 255, 0);
                objectifs[currentIndex].text = "Score to reach   " + _scoreToChangeScene + " / " + _scoreToChangeScene;
            }
            currentIndex++;
        }
        bool points = _currentScore >= _scoreToChangeScene;


        //Aquatic
        if(_harvestAquaticPlantsToReach > 0)
        {
            if(_harvestAquaticPlants< _harvestAquaticPlantsToReach)
            {
                objectifs[currentIndex].text = "Aquatic plant to harvest   " + _harvestAquaticPlants + " / " + _harvestAquaticPlantsToReach;
            }

            if (_harvestAquaticPlants >= _harvestAquaticPlantsToReach)
            {
                objectifs[currentIndex].color = new Color(0, 255, 0);
                objectifs[currentIndex].text = "Aquatic plant to harvest   " + _harvestAquaticPlantsToReach + " / " + _harvestAquaticPlantsToReach;
            }
            currentIndex++;
        }
        bool aquaticPlants = _harvestAquaticPlants >= _harvestAquaticPlantsToReach;


        //plants guérie
        if(_plantToHeal> 0  && currentIndex <= 3)
        {
            if(_currentPlantHealed< _plantToHeal)
            {
                objectifs[currentIndex].text = "Infestation stopped   " + _currentPlantHealed + " / " + _plantToHeal;
            }
            if (_currentPlantHealed >= _plantToHeal)
            {
                objectifs[currentIndex].color = new Color(0, 255, 0);
                objectifs[currentIndex].text = "Infestation stopped   " + _plantToHeal + " / " + _plantToHeal;
            }

            currentIndex++;
        }
        bool healedPlants = _currentPlantHealed >= _plantToHeal;


        //plante a récolter
        if (_NombreDePlantARecolter > 0 && currentIndex<=3)
        {
            if(_totalHarvestedPlant < _NombreDePlantARecolter)
            {
                objectifs[currentIndex].text = "Plants to harvest   " + _totalHarvestedPlant + " / " + _NombreDePlantARecolter;
            }

            if (_totalHarvestedPlant >= _NombreDePlantARecolter)
            {
                objectifs[currentIndex].color = new Color(0, 255, 0);
                objectifs[currentIndex].text = "Plants to harvest   " + _NombreDePlantARecolter + " / " + _NombreDePlantARecolter;
            }
            currentIndex++;
        }
        bool recoltedplant = _totalHarvestedPlant >= _NombreDePlantARecolter;

        //animaux
        bool a = true;

        int spawn = 0;
        for (int i = 0; i < _animals.Length; i++)
        {
            spawn++;
            if (!_animals[i])
            {
                a = false;
                spawn--;
            }
        }

        if(_animals.Length>0 && currentIndex <= 3)
        {
            if(spawn< _animals.Length)
            {
            objectifs[currentIndex].text = "Wild animals to see   " + spawn + " / " + _animals.Length;

            }

            if (spawn>= _animals.Length)
            {
                objectifs[currentIndex].color = new Color(0, 255, 0);
                objectifs[currentIndex].text = "Wild animals to see   " + _animals.Length + " / " + _animals.Length;

            }
            currentIndex++;
        }

        if(vivace && arrosage && points && aquaticPlants && recoltedplant & a && _winmenu)
        {
            Time.timeScale = 0;
            if (!_winMenu.activeSelf)
            {
             _winMenu.SetActive(true);

            }
        }
    }
    #endregion

}
