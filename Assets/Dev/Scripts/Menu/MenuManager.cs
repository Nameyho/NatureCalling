using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class MenuManager : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private GameObject _MainMenu;

    [SerializeField]
    private GameObject _levelSelection;

    [SerializeField]
    private GameObject _credit;

    [SerializeField]
    private GameObject _leave;

    [SerializeField]
    private GameObject _option;

    [SerializeField]
    private SceneCollection _scenesList;

    #endregion


    #region Private

    private bool _isMenuCanBeOpen = true;
    #endregion

    #region Unity API

    private void Update()
    {
        if (Input.anyKeyDown & _isMenuCanBeOpen)
        {
            _MainMenu.SetActive(true);
        }
    }

    #endregion

    #region Public

    public void OnClickLevelSelection()
    {
        _MainMenu.SetActive(false);
        _levelSelection.SetActive(true);
        _isMenuCanBeOpen = false;
    }

    public void OnClickCredits()
    {
        _MainMenu.SetActive(false);
        _credit.SetActive(true);
        _isMenuCanBeOpen = false;
    }

    public void OnClickOptions()
    {
        _MainMenu.SetActive(false);
        _option.SetActive(true);
        _isMenuCanBeOpen = false;
    }

    public void OnClickMainMenu(GameObject go)
    {
        _MainMenu.SetActive(true);
        go.SetActive(false);
    }

    public void OnLeaveMenu()
    {
        _MainMenu.SetActive(false);
        _leave.SetActive(true);
        _isMenuCanBeOpen = false;
    }
    public void LoadScene(int scene)
    {
          SceneManager.LoadScene(_scenesList[scene].SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
