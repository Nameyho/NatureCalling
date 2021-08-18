using ScriptableObjectArchitecture;
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

    [SerializeField]
    private GameObject _ghostModel;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject _plantsPrefabs;
    

    [Header("camera")]
    public LayerMask IgnoreMe;
    
    public float _moveSpeed;

    #endregion

    #region Private

    private Renderer _myRend;
    private bool _isBuildable = false;
    private bool _isAlreadyMove = false;
    private Camera cam;
    


    #endregion

    #region Unity API


    private void Update()
    {
		FollowMouse();
		onClick();
    }



    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("UnBuild"))
        {

            _myRend.material = _Unbuildable;
            _isBuildable = false;
        }


    }
    private void OnTriggerExit(Collider other)
    {
       

        if (other.CompareTag("UnBuild"))
        {
            _myRend.material = _Effect;
            _isBuildable = true;
        }
        else
        {
            _myRend.material = _Basic;
            _isBuildable = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EffectZone"))
        {
            _myRend.material = _Effect;
            _isBuildable = true;
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
       

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {
            Debug.Log(hit.transform.tag +  hit.transform.name);
            if (Input.GetMouseButtonDown(0)&& _isBuildable)
            {
                Debug.Log(hit.point);
                Instantiate(_plantsPrefabs,hit.point, Quaternion.identity);
            }
        }

    }

    private void FollowMouse()
    {
        if (Input.GetMouseButton(1))
        {
            if (!_isAlreadyMove)
            {
                _isAlreadyMove = true;
                _isBuildable = true;
            }
            //RaycastHit hit;
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
            //{
               
            //    this.transform.position =new Vector3 (hit.point.x,0,hit.point.z);
            //}

        }



    }
    #endregion
}
