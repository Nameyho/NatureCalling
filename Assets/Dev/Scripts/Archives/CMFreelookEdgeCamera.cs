using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CMFreelookEdgeCamera : MonoBehaviour
{
    public int boundary = 50; // distance from edge scrolling starts
    public float speed = 1f;
    int screenWidth;
    int screenHeight;

    [Header("Zoom")]

    [SerializeField]
    private float _SpeedZoom = 3f;

    [SerializeField]
    private float ZoomInMax = 5f;

    [SerializeField]
    private float ZoomOutMax = 20f;

    [SerializeField]
    [Range(1,500)]
    private float _zoomMultiply = 10f;


    [Header("Pan")]

    [SerializeField]
    private float _panBoundsUp;

    [SerializeField]
    private float _panBoundsDown;

    [SerializeField]
    private float _panBoundsLeft;

    [SerializeField]
    private float _panBoundsRight;

    [SerializeField]
    private float _panSpeed;


    private CinemachineFreeLook m_virtualCamera;


    private void Awake()
    {
        m_virtualCamera = GetComponent<CinemachineFreeLook>();

    }
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
    private void Update()
    {
        float up =  Input.GetAxis("Mouse ScrollWheel");
        Cursor.lockState = CursorLockMode.Confined;
        
        ZoomScreen(_zoomMultiply *up);
        Pan();


    }

    public void ZoomScreen(float increment)
    {
        float radius=  m_virtualCamera.m_Orbits[0].m_Radius;

        m_virtualCamera.m_Orbits[1].m_Radius = radius-increment *Time.deltaTime;
        m_virtualCamera.m_Orbits[1].m_Radius = Mathf.Clamp(m_virtualCamera.m_Orbits[1].m_Radius, ZoomInMax, ZoomOutMax);
        m_virtualCamera.m_Orbits[0].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius ;
        m_virtualCamera.m_Orbits[2].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius ;


        //if( increment<0) // -10 = dezoom
        //{
        //     radius = m_virtualCamera.m_Orbits[0].m_Radius;
        //     target = Mathf.Clamp(radius-increment, ZoomInMax, ZoomOutMax);

        //    m_virtualCamera.m_Orbits[1].m_Radius = Mathf.Lerp(target, radius, _SpeedZoom * Time.deltaTime);
        //    m_virtualCamera.m_Orbits[0].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 5;
        //    m_virtualCamera.m_Orbits[2].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 10;


        //}


    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.mousePosition.x > screenWidth - boundary)
            {
                return speed;
            }
            if (Input.mousePosition.x < boundary)
            {
                return -speed;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.mousePosition.y > screenHeight - boundary)
            {
                return speed;
            }
            if (Input.mousePosition.y < boundary)
            {
                return -speed;
            }
        }
      
     
        return 0f;
    }

    public void Pan()
    {

        //démarrage du jeu 
        if(m_virtualCamera.m_XAxis.Value>- 45 && m_virtualCamera.m_XAxis.Value < 45)
        {

            Vector3 startPosition = m_virtualCamera.m_LookAt.transform.position;

            if (Input.GetKey(KeyCode.D)&& startPosition.x < _panBoundsUp)
            {
            
           
                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsUp, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition,(_panSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.Q) && startPosition.x > _panBoundsDown)
            {
         

                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsDown, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.S) && startPosition.z > _panBoundsLeft)
            {


                Vector3 targetPosition = new Vector3(startPosition.x , startPosition.y, startPosition.z  +_panBoundsLeft);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.Z) && startPosition.z < _panBoundsRight)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y , startPosition.z  +_panBoundsRight);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
        }

        //caméra sur la gauche
        if (m_virtualCamera.m_XAxis.Value > 45 && m_virtualCamera.m_XAxis.Value < 135)
        {

            Vector3 startPosition = m_virtualCamera.m_LookAt.transform.position;

            if (Input.GetKey(KeyCode.Z) && startPosition.x < _panBoundsUp)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsUp, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.S) && startPosition.x > _panBoundsDown)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsDown, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.D) && startPosition.z > _panBoundsLeft)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsLeft);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.Q) && startPosition.z < _panBoundsRight)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsRight);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
        }

        //caméra sur la droite
        if (m_virtualCamera.m_XAxis.Value > -135 && m_virtualCamera.m_XAxis.Value < -45)
        {

            Vector3 startPosition = m_virtualCamera.m_LookAt.transform.position;

            if (Input.GetKey(KeyCode.S) && startPosition.x < _panBoundsUp)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsUp, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.Z) && startPosition.x > _panBoundsDown)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsDown, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.Q) && startPosition.z > _panBoundsLeft)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsLeft);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.D) && startPosition.z < _panBoundsRight)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsRight);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
        }


        //caméra à l'arrière


        if (m_virtualCamera.m_XAxis.Value < -135 || m_virtualCamera.m_XAxis.Value > 135)
        {

            Vector3 startPosition = m_virtualCamera.m_LookAt.transform.position;

            if (Input.GetKey(KeyCode.Q) && startPosition.x < _panBoundsUp)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsUp, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.D) && startPosition.x > _panBoundsDown)
            {


                Vector3 targetPosition = new Vector3(startPosition.x + _panBoundsDown, startPosition.y, startPosition.z);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.Z) && startPosition.z > _panBoundsLeft)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsLeft);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }


            if (Input.GetKey(KeyCode.S) && startPosition.z < _panBoundsRight)
            {


                Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + _panBoundsRight);


                m_virtualCamera.m_LookAt.position = Vector3.MoveTowards(startPosition, targetPosition, (_panSpeed * Time.deltaTime));
            }
        }
    }
}
