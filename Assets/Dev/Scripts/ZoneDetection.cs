
using UnityEngine;

public class ZoneDetection : MonoBehaviour
{
    #region Exposed

    [Header("Materials")]
    [SerializeField]
    private Material _Basic;

    [SerializeField]
    private Material _Effect;

    [SerializeField]
    private Material _Unbuildable;


    #endregion

    #region Private

    private Renderer _myRend;

    #endregion

    #region Unity API
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("UnBuild"))
        {
            Debug.Log("Je peux pas construire");
            _myRend.material = _Unbuildable;

        }


    }
    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("UnBuild"))
        {
            _myRend.material = _Effect;
        }
        else
        {
            _myRend.material = _Basic;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EffectZone"))
        {
            _myRend.material = _Effect;
        }
    }

    private void Awake()
    {
        _myRend = GetComponent<Renderer>();
    }

    #endregion  
}
