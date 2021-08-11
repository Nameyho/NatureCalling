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

    [Header("Prefabs")]
    [SerializeField]
    private ObjectVariable _plantsPrefabs;
    

    [Header("camera")]
    public Camera cam;
    public LayerMask IgnoreMe;
    
    public float _moveSpeed;

    #endregion

    #region Private

    private Renderer _myRend;
    private bool _isBuildable = false;
    private bool _isAlreadyMove = false;
    


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
        _myRend = GetComponent<Renderer>();
        
    }

    #endregion


    #region Methods

    private void onClick()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
       

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {
            
            if (Input.GetMouseButtonDown(0)&& _isBuildable)
            {
                
                Instantiate(_plantsPrefabs.Value,new Vector3( hit.point.x,0.5f ,hit.point.z), Quaternion.identity);
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
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
            {
               
                this.transform.position =new Vector3 (hit.point.x,0,hit.point.z);
            }

        }



    }
    #endregion
}
