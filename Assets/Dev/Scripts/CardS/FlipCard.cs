using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FlipCard : MonoBehaviour
{
    #region Exposed
    [SerializeField]
    private float _timeToFlip;

    private float steptotal;

    [SerializeField]
    private GameObject _modelMask;

    #endregion


    #region private

    float step;
    bool _mustflip = false;
    Transform _transform;

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
            _modelMask.transform.localRotation = Quaternion.Slerp(transform.localRotation, target, 1f);
        yield return new WaitForSeconds(1f);

        }
    }

    IEnumerator UnFlip()
    {
        while (steptotal > 0 && ! _mustflip)
        {
            steptotal -= step;

            Quaternion target = Quaternion.Euler(0, steptotal, 0);
            _modelMask.transform.localRotation = Quaternion.Slerp(transform.localRotation, target, 1f);
            yield return new WaitForSeconds(1f);

        }

        
    }


    private void OnMouseDown()
    {
        if (!_mustflip)
        {
            _mustflip = true;
            StartCoroutine(Flip());

        }
        else
        {
            _mustflip = false;
            StartCoroutine(UnFlip());
        }
        

    }

    private void Update()
    {

    }

    #endregion
}
