using UnityEngine;
using System.Collections;
using ScriptableObjectArchitecture;

public class DragAndDropCard : MonoBehaviour
{
    #region Private

    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform _transform;
    private bool _isDragable;
    private Camera cam;
    private bool _isGhost = false;
    private bool _isBusy =false;
    private int nbr = 0;

    private Transform _Hand;

    [SerializeField]
    private CurrentSpawnerLocationScritpable _currentSpawnerLocation;

    #endregion



    #region Unity API
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        cam = Camera.main;
        _Hand = this.gameObject.transform.parent.transform.parent.transform.parent.transform;
    }

    private void LateUpdate()
    {
        if (_isDragable)
        {
            Drag();
        }
        if (Input.GetMouseButtonDown(1))
        {
            reset();
        }

    }
    void OnMouseDown()
    {
        
        if (!(_isBusy))
        {
            nbr++;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            _isDragable = true;
            _isBusy = true;
        }
    }

    #endregion


    #region Methods
    void Drag()
    {
        if (!_isGhost)
        {

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);


            transform.position = new Vector3(curPosition.x, curPosition.y, curPosition.z);

        }
        else
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "CardsBackground"){


                    _transform.position = _transform.parent.position;
                   }
                else
                {
                    this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z); 
                }
                
            }

        }
    }

    private void reset()
    {
        
        _Hand.gameObject.SetActive(true);
        _isBusy = false;
        _transform.SetParent(_currentSpawnerLocation._SpawnerTransform);
        transform.localPosition = Vector3.zero;
        _isDragable = false;
        
    }


    public void SetGhost(bool b)
    {
        _isGhost = b;
    }
    #endregion

}