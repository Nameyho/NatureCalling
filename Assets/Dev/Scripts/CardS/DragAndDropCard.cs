using UnityEngine;
using System.Collections;

public class DragAndDropCard : MonoBehaviour
{
    #region Private

    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform _transform;
    private bool _isDragable;
    private Camera cam;
    private bool _isGhost = false;

    #endregion


    #region Unity API
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (_isDragable)
        {
            Drag();
        }
        if (Input.GetMouseButtonDown(1))
        {
            reset();
        }
       // CardsRotation();
    }
    void OnMouseDown()
    {

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        _isDragable = true;
    }

    #endregion


    #region Methods
    void Drag()
    {
        if (!_isGhost)
        {

            Debug.Log("pas fantome");
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);


            transform.position = new Vector3(curPosition.x, curPosition.y, curPosition.z);

        }
        else
        {
            Debug.Log("fantome");
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "CardsBackground"){


                    _transform.position = _transform.parent.position;
                   }
                else
                {
                    this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z); ;
                }
                
            }

        }
    }

    private void reset()
    {
        transform.localPosition = Vector3.zero;
        _isDragable = false;
    }

    private void CardsRotation()
    {
        float mousePosition = Input.GetAxis("Horizontal");
        Debug.Log(mousePosition);

        RaycastHit hit;
     

    }

    public void SetGhost(bool b)
    {
        _isGhost = b;
    }
    #endregion

}