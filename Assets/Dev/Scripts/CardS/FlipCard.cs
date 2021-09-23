using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class FlipCard : MonoBehaviour
{
    #region Exposed
    [SerializeField]
    private float _timeToFlip;

    private float steptotal;

    [SerializeField]
    private GameObject _modelMask;

    [SerializeField]
    private GameObject _listeComptabiliteText;

    [SerializeField]
    private GameObject _prefabPlant;

    [SerializeField]
    private GameObject _backcardText;

    #endregion


    #region private

    float step;
    bool _mustflip = false;
    Transform _transform;
    TMP_Text[] _comps;

    #endregion

    #region public


    private void Awake()
    {
        step = 180 / _timeToFlip;
        _transform = transform.parent.transform.parent.transform;
       
    }

     IEnumerator Flip()
    {
        while (steptotal<180 && _mustflip )
        {
          steptotal += step;

            Quaternion target = Quaternion.Euler(0, steptotal,0) ;
            _modelMask.transform.localRotation = Quaternion.Lerp(transform.localRotation, target,1f );
        yield return new WaitForSeconds(1f);

        }
    }

    IEnumerator UnFlip()
    {
        while (steptotal > 0 && ! _mustflip)
        {
            steptotal -= step;

            Quaternion target = Quaternion.Euler(0, steptotal, 0);
            _modelMask.transform.localRotation = Quaternion.Lerp(transform.localRotation, target, 1f);
            yield return new WaitForSeconds(1f);

        }

        
    }

    private void ShowComptability()
    {
        _listeComptabiliteText.SetActive(true);
        _comps = _listeComptabiliteText.GetComponentsInChildren<TMP_Text>();
        if (_prefabPlant.GetComponent<Plants>())
        {
            List<CardScriptable> plantscomp=  _prefabPlant.GetComponent<Plants>().GetCompatibilite();
            if (plantscomp.Count>0)
            {
                for (int i = 0; i < plantscomp.Count; i++)
                {
                    _comps[i].text = plantscomp[i]._CardName;
                    _comps[i].GetComponent<TextComplentarite>().SetCardScriptable(plantscomp[i]);

                }

            }

        }
        else
        {
            _backcardText.SetActive(true);
        }
    }

    private void HideComptability()
    {
        _comps = _listeComptabiliteText.GetComponentsInChildren<TMP_Text>();
        if (_prefabPlant.GetComponent<Plants>())
        {
            List<CardScriptable> plantscomp = _prefabPlant.GetComponent<Plants>().GetCompatibilite();
            if (plantscomp.Count > 0)
            {
                for (int i = 0; i < plantscomp.Count; i++)
                {
                    _comps[i].text = "";
                    Debug.Log(plantscomp[i]._CardName);
                }
        }
        }
        else
        {
            _backcardText.SetActive(false);

        }
    }

    private void ShowAllPlantCards()
    {


        List<GameObject> plantsOnMap = FindObjectOfType<PlantsManager>().GetPlantsOnTheMaps();
        if (_prefabPlant.GetComponent<Plants>())
        {
            for (int i = 0; i < plantsOnMap.Count; i++)
            {

                if (plantsOnMap[i].GetComponent<Plants>().GetCard()._CardName.Equals(_prefabPlant.GetComponent<Plants>().GetCard()._CardName))
                {
                    plantsOnMap[i].GetComponent<Plants>().ActivateFx();
                    plantsOnMap[i].GetComponent<Plants>().SetisCard(true);
                }
            }

        }
    }

    private void HideAllPlantsCards()
    {
        List<GameObject> plantsOnMap = FindObjectOfType<PlantsManager>().GetPlantsOnTheMaps();
        if (_prefabPlant.GetComponent<Plants>())
        {
            for (int i = 0; i < plantsOnMap.Count; i++)
            {
                if (plantsOnMap[i].GetComponent<Plants>().GetCard()._CardName.Equals(_prefabPlant.GetComponent<Plants>().GetCard()._CardName))
                {
                    plantsOnMap[i].GetComponent<Plants>().SetisCard(false);
                    plantsOnMap[i].GetComponent<Plants>().DisableVFX();
                }
            }

        }
    }

    private void OnMouseDown()
    {
        if (!_mustflip)
        {
            _mustflip = true;

            _transform.GetComponentInParent<DragAndDropCard>().SetIsLockedByFlip(true);
            
            StartCoroutine(Flip());
            ShowComptability();
            ShowAllPlantCards();

        }
        else
        {
            _transform.GetComponentInParent<DragAndDropCard>().SetIsLockedByFlip(false);
            HideComptability();
            _mustflip = false;
            StartCoroutine(UnFlip());
            HideAllPlantsCards();

            for (int i = 0; i < _comps.Length; i++)
            {
                _comps[i].GetComponent<TextComplentarite>().HidePlante();
            }
        }
        

    }
    #endregion
}
