using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextComplentarite : MonoBehaviour
{
    #region private

    CardScriptable _cs;
    bool isOn;

    #endregion


    #region Unity API

    private void OnMouseDown()
    {
        if (_cs)
        {
        Debug.Log(isOn);
        if (isOn)
        {
            isOn = false;
                HidePlante();
        }
        else
        {
            ShowPlante();
            isOn = true;
        }

        }
    }

    #endregion

    #region Public 

    public void SetCardScriptable(CardScriptable cs)
    {
        _cs = cs;
    }


    #endregion

    #region private methodes

    private void ShowPlante()
    {
        List<GameObject> plantsOnMap = FindObjectOfType<PlantsManager>().GetPlantsOnTheMaps();

        for (int i = 0; i < plantsOnMap.Count; i++)
        {
            if (plantsOnMap[i].GetComponent<Plants>().GetCard()._CardName.Equals(_cs._CardName))
            {
                plantsOnMap[i].GetComponent<Plants>().SetisCard(true);
                plantsOnMap[i].GetComponent<Plants>().ActivateFx();
            }
        }
    }

    public void HidePlante()
    {
        List<GameObject> plantsOnMap = FindObjectOfType<PlantsManager>().GetPlantsOnTheMaps();
        if (_cs)
        {
            for (int i = 0; i < plantsOnMap.Count; i++)
            {
            
                if (plantsOnMap[i].GetComponent<Plants>().GetCard()._CardName.Equals(_cs._CardName))
                {
                    plantsOnMap[i].GetComponent<Plants>().SetisCard(false);
                    plantsOnMap[i].GetComponent<Plants>().DisableVFX();
                }
            }

        }
    }

    #endregion
}
