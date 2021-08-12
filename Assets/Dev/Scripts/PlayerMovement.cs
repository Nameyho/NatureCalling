using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Exposed

    [Header("Camera")]
    [SerializeField]
    private Camera cam;

    [Header("Movement Configuration")]
    [SerializeField]
    private float _speedMovement;

    #endregion;

    #region Private
    private Transform _transform;
    private Vector3 target;

    #endregion

    #region Unity API
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
    }

    #endregion

    #region Privates Methods
    private void Move()
    {
    
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
       
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                target = hit.point;
            
            }

        }
        _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(target.x,1,target.z), _speedMovement*Time.deltaTime);
    }

    #endregion
}
