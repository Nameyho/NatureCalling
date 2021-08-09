using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Exposed

    [Header("Scrolling")]
    [SerializeField]
    private float _SpeedScrolling = 2f;

    [SerializeField]
    private float _maxUpScrolling = 10f;

    [SerializeField]
    private float _minDownScrolling = -10f;

    [SerializeField]
    private float _maxRightScrolling = 10f;

    [SerializeField]
    private float _maxLeftScrolling = 10f;

    [Header("Zoom")]
    [SerializeField]
    private float _SpeedZoom = 3f;

    [SerializeField]
    private float ZoomInMax = 40f;

    [SerializeField]
    private float ZoomOutMax = 90f;


    #endregion


    #region private

    private CinemachineInputProvider m_inputProvider;
    private CinemachineVirtualCamera m_virtualCamera;
    private Transform m_cameratransform;

    #endregion

    #region Unity API

    private void Awake()
    {
        m_inputProvider = GetComponent<CinemachineInputProvider>();
        m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
        m_cameratransform = GetComponent<Transform>();
    }

    private void Update()
    {
        float x = m_inputProvider.GetAxisValue(0);
        float y = m_inputProvider.GetAxisValue(1);
        float z = m_inputProvider.GetAxisValue(2);
        if(x != 0 || y!= 0 )
        {
            PanScreen(x, y,z) ;
        }
        
        if(z != 0){
           
            ZoomScreen(z);
        }
    }

    #endregion




    #region Public Methods

    public void PanScreen(float x , float y,float z)
    {
        Vector3 direction = PanDirection(x, y,z);



        m_cameratransform.position = Vector3.Lerp(m_cameratransform.position, 
            
            new Vector3(Mathf.Clamp(m_cameratransform.position.x + direction.x,_maxLeftScrolling,_maxRightScrolling ),m_cameratransform.position.y, Mathf.Clamp(m_cameratransform.position.z + direction.z, _minDownScrolling, _maxUpScrolling))

            + direction * _SpeedScrolling, Time.deltaTime);

    }

    public Vector3 PanDirection(float x ,float y,float z)
    {
        Vector3 direction = Vector3.zero;
        if(y>= Screen.height * .95f)
        {
           direction.z += 1;
            
        }
        else if (y <= Screen.height * .05f)
        {
            direction.z -= 1;
        }

        else if( x>= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }
        return direction;
    }


    public void ZoomScreen( float increment)
    {
        float fov = m_virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, ZoomOutMax, ZoomInMax);
        m_virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, _SpeedZoom * Time.deltaTime);
    }
    #endregion
}
