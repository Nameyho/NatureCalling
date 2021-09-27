using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class HandManager : MonoBehaviour
{
    #region Exposed


    [SerializeField]
    private ObjectVariable _currentPrefabSelected;


    [SerializeField]
    private Transform[] _spawnLocation;

    [SerializeField]
    private SubMenuScriptable _subMenu;


    [SerializeField]
    private GameObject _previous;

    [SerializeField]
    private GameObject _forward;
    #endregion

    #region Private
    private int _minimum = 0;

	#endregion

	#region Unity API

	private void Update()
    {
        EnableNavigation();

    }
	private void Awake()
	{
		_subMenu._listSubMenu.Clear();
	}

	#endregion


	#region Public Methods 


	public void PlayCard(CardScriptable card, GameObject go)
    {

        if (card._prefabToSpawn)
        {
            _currentPrefabSelected.Value = card._prefabToSpawn;
        }
    }


    public void ChangeHand()
    {
        for (int i = 0; i < _spawnLocation.Length; i++)
        {
            if (_spawnLocation[i].childCount > 0)
            {
                Transform go = _spawnLocation[i].GetChild(0);
                Destroy(go.gameObject);

            }
        }

        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);

        }

        


        int shortestlistlength = 0;
        if (_spawnLocation.Length <= _subMenu._listSubMenu.Count)
        {
            shortestlistlength = _spawnLocation.Length;
        }
        else {
            shortestlistlength = _subMenu._listSubMenu.Count;
        }


        for (int i = 0 ; i < shortestlistlength; i++)
        {
            if (_subMenu._listSubMenu[i]._cardModel)
            {
                GameObject go = Instantiate(_subMenu._listSubMenu[i+_minimum]._cardModel, _spawnLocation[i]);

                go.transform.SetParent(_spawnLocation[i]);
            }
        }
    }

    public void Navigate(int step)
    {
        _minimum += step;
        ChangeHand();
    }

    public void EnableNavigation()
    {
        if(((_minimum +(_spawnLocation.Length- 1 )) -_spawnLocation.Length) >=0)
                                   {
            _previous.SetActive(true);
        }
        else
        {
            _previous.SetActive(false);
        }



        if(_subMenu._listSubMenu.Count - _spawnLocation.Length > _minimum)
        {
            _forward.SetActive(true);
        }
        else
        {
            _forward.SetActive(false);
        }

    }
public void resetMinimum()
    {
        _minimum = 0;
    }
    #endregion


}
