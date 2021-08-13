using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Exposed

    [Header("Scrolling")]
    [SerializeField]
    private float _SpeedScrolling = 2f;

    //[SerializeField]
    //private float _maxUpScrolling = 10f;

    //[SerializeField]
    //private float _minDownScrolling = -10f;

    //[SerializeField]
    //private float _maxRightScrolling = 10f;

    //[SerializeField]
    //private float _maxLeftScrolling = 10f;

    [Header("Zoom")]
    [SerializeField]
    private float _SpeedZoom = 3f;

    [SerializeField]
    private float ZoomInMax = 40f;

    [SerializeField]
    private float ZoomOutMax = 90f;

    [SerializeField]
    private float _minScrollSpeed = 0f;

    [SerializeField]
    private float _maxScrollSpeed = 200f;


    #endregion


    #region private

    private CinemachineInputProvider m_inputProvider;
    private CinemachineFreeLook m_virtualCamera;
    private Transform m_cameratransform;
    private float lastFov;

    #endregion

    #region Unity API

    private void Awake()
    {
        m_inputProvider = GetComponent<CinemachineInputProvider>();
        m_virtualCamera = GetComponent<CinemachineFreeLook>();
        m_cameratransform = GetComponent<Transform>();
        
    }

    private void Update()
    {
        float z = m_inputProvider.GetAxisValue(2);
        Cursor.lockState = CursorLockMode.Confined;
        
        if(z != 0){
           
            ZoomScreen(z);
        }

	}

	#endregion


	#region Public Methods
    public void ZoomScreen( float increment)
    {
        float fov = m_virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, ZoomOutMax, ZoomInMax);
        m_virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, _SpeedZoom * Time.deltaTime);

        if(lastFov< fov &&(ZoomOutMax >= fov))
        {
             _SpeedScrolling += (_SpeedScrolling / (ZoomOutMax - ZoomInMax) *fov);
        }    

        else if (lastFov >fov && (ZoomInMax <= fov))
        {
            _SpeedScrolling -= (_SpeedScrolling / (ZoomOutMax - ZoomInMax) * fov );
        }
        lastFov = fov;
        _SpeedScrolling = Mathf.Clamp(_SpeedScrolling, _minScrollSpeed, _maxScrollSpeed);

    }
    #endregion
}
