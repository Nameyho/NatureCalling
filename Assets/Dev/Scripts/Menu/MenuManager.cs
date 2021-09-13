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

    public void OnClickMainMenu(GameObject go)
    {
        _MainMenu.SetActive(true);
        go.SetActive(false);
    }

    public void LoadScene(int scene)
    {

          SceneManager.LoadScene(_scenesList[scene].SceneName);
    }
    #endregion
}
