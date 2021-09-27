using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectiveButton : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{


    #region Exposed

    [SerializeField]
    private GameObject _objectives;
    #endregion

    #region private

    bool _lockedByClick;
    #endregion

    #region Unity API

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_objectives.activeSelf && _lockedByClick)
        {
            _objectives.SetActive(false);
            _lockedByClick = false;

        }
        else
        {
            _objectives.SetActive(true);
            _lockedByClick = true;

        }
    }

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!_lockedByClick)
    //    {
    //     _objectives.SetActive(false);

    //    }
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        _objectives.SetActive(true);
    }

    public bool getLocked()
    {
        return _lockedByClick;
    }

    #endregion
}
