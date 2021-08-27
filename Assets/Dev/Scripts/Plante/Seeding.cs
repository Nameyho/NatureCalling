using ScriptableObjectArchitecture;
using UnityEngine;

public class Seeding : MonoBehaviour
{
    #region Exposed

    [Header("Materials")]
    [SerializeField]
    private Material _Basic;

    [SerializeField]
    private Material _Effect;

    [SerializeField]
    private Material _Unbuildable;

    [SerializeField]
    private GameObject _ghostModel;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject _plantsPrefabs;
    

    [Header("camera")]
    public LayerMask IgnoreMe;
   

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    #endregion

    #region Private

    private Renderer _myRend;
    private bool _isBuildable = false;
    private Camera cam;
    private bool _isSelected;


    #endregion

    #region Unity API


    private void Update()
    {
		
		onClick();
    }



    //private void OnTriggerStay(Collider other)
    //{

    //    if (other.CompareTag("UnBuild"))
    //    {
    //        Debug.Log("unbuild");
    //        _myRend.material = _Unbuildable;
    //        _isBuildable = false;
    //    }


    //}
    //private void OnTriggerExit(Collider other)
    //{

    //    Debug.Log("unbuild");
    //    if (other.CompareTag("UnBuild"))
    //    {
    //        _myRend.material = _Effect;
    //        _isBuildable = true;
    //    }
    //    else
    //    {
    //        Debug.Log("basic");
    //        _myRend.material = _Basic;
    //        _isBuildable = true;
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("EffectZone"))

    //    {
    //        Debug.Log("Effectzone");
    //        _myRend.material = _Effect;
    //        _isBuildable = true;
    //    }
    //}

    public void UpdateRenderer(int i)
    {
        switch (i)
        {
            case 1:
                _myRend.material = _Effect;
                _isBuildable = true;
                break;


            case 2:
                _myRend.material = _Unbuildable;
                _isBuildable = false;
                break;
            case 0:
                _myRend.material = _Basic;
                _isBuildable = true;
                break;
        }
            

    }



    private void Awake()
    {
        _myRend = _ghostModel.GetComponent<Renderer>();
        cam = Camera.main;
    }

    #endregion


    #region Methods

    private void onClick()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        CardScriptable cs = GetComponent<Cards>().GetCardScriptable();

        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {

            Debug.Log(hit.transform.tag);

            if (Input.GetMouseButtonDown(0)&& _isBuildable && _isSelected)
            {

                if (hit.transform.tag == "Plants" &&cs._isPlant)
                {
                     Instantiate(_plantsPrefabs,hit.point, Quaternion.identity);
                    _gameManager.AddProgression(cs._bonusBioDiversity);

                }
                if (hit.transform.tag == "AquaticPlants" && cs._isAquaticPlant)
                {
                    Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    _gameManager.AddProgression(cs._bonusBioDiversity);

                }
                if( !cs._isAquaticPlant && !cs._isPlant && hit.transform.tag=="BuildingZone")
                {
                    Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    _gameManager.AddProgression(cs._bonusBioDiversity);
                }

                //GetComponent<Cards>().PlayThisCard();


            }
        }

    }
    #endregion

    #region public

    public void SetIsSelected(bool f)
    {
        _isSelected = f;
    }

    #endregion
}
