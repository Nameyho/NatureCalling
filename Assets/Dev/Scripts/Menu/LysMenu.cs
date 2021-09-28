using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LysMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _winMenu;

    private void OnMouseDown()
    {
        Time.timeScale = 0;
        if (!_winMenu.activeSelf)
        {
            _winMenu.SetActive(true);

        }
    }
}

