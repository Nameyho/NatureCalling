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



    private CinemachineInputProvider m_inputProvider;
    private CinemachineFreeLook m_virtualCamera;
    private Transform m_cameratransform;
    private float lastFov;

    private void Awake()
    {
        m_inputProvider = GetComponent<CinemachineInputProvider>();
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
        if (up != 0)
        {
            //ZoomScreen(up);
        }
        if(up > 0)
        {
            ZoomScreen(_zoomMultiply);
        }
        else if(up<0)
        {
            ZoomScreen(-_zoomMultiply);
        }

    }

    public void ZoomScreen(float increment)
    {

  
        if(increment> 0 ) // 10 = zoom
        {
            float radius=  m_virtualCamera.m_Orbits[0].m_Radius;
            float target = Mathf.Clamp(radius- increment, ZoomInMax, ZoomOutMax);

            Debug.Log(target);
     
                m_virtualCamera.m_Orbits[1].m_Radius = Mathf.Lerp(target, radius, _SpeedZoom * Time.deltaTime);
                m_virtualCamera.m_Orbits[0].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 5;
                m_virtualCamera.m_Orbits[2].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 10;


            
            
            

        }
        if( increment<0) // -10 = dezoom
        {
            float radius = m_virtualCamera.m_Orbits[0].m_Radius;
            float target = Mathf.Clamp(radius-increment, ZoomInMax, ZoomOutMax);




            m_virtualCamera.m_Orbits[1].m_Radius = Mathf.Lerp(target, radius, _SpeedZoom * Time.deltaTime);
            m_virtualCamera.m_Orbits[0].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 5;
            m_virtualCamera.m_Orbits[2].m_Radius = m_virtualCamera.m_Orbits[1].m_Radius - 10;




        }


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
}
