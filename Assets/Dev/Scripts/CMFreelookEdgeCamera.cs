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

	//[Header("Zoom")]
	//[SerializeField]
	//private float _SpeedScrolling = 2f;

	//[SerializeField]
	//private float _SpeedZoom = 3f;

	//[SerializeField]
	//private float ZoomInMax = 40f;

	//[SerializeField]
	//private float ZoomOutMax = 90f;

	//[SerializeField]
	//private float _minScrollSpeed = 0f;

	//[SerializeField]
	//private float _maxScrollSpeed = 200f;

	//private CinemachineInputProvider m_inputProvider;
	//private CinemachineFreeLook m_virtualCamera;
	//private Transform m_cameratransform;
	//private float lastFov;

	//private void Awake()
	//{
	//	m_inputProvider = GetComponent<CinemachineInputProvider>();
	//m_virtualCamera = GetComponent<CinemachineFreeLook>();

	//}
	void Start()
	{
		CinemachineCore.GetInputAxis = GetAxisCustom;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
	private void Update()
	{
		//	float z = m_inputProvider.GetAxisValue(2);
		Cursor.lockState = CursorLockMode.Confined;

		//	if (z != 0)
		//	{
		//		ZoomScreen(z);
		//	}

	}

	//public void ZoomScreen(float increment)
	//{
	//	float fov = m_virtualCamera.m_Lens.FieldOfView;
	//	float target = Mathf.Clamp(fov + increment, ZoomOutMax, ZoomInMax);
	//	m_virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, _SpeedZoom * Time.deltaTime);

	//	if (lastFov < fov && (ZoomOutMax >= fov))
	//	{
	//		_SpeedScrolling += (_SpeedScrolling / (ZoomOutMax - ZoomInMax) * fov);
	//	}

	//	else if (lastFov > fov && (ZoomInMax <= fov))
	//	{
	//		_SpeedScrolling -= (_SpeedScrolling / (ZoomOutMax - ZoomInMax) * fov);
	//	}
	//	lastFov = fov;
	//	_SpeedScrolling = Mathf.Clamp(_SpeedScrolling, _minScrollSpeed, _maxScrollSpeed);

	//}

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
			//} else if (axisName == "Mouse ScrollWheel")
			//	{
			//		return Input.GetAxis(axisName);
			//	}
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
			return 0;
		}
		return 0;
	}
}
