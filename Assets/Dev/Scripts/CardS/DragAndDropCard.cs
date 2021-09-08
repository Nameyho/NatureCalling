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
    private Collider effectcollider;
    private float _lastTimeUnbuild;
    private float _lastTimeEffect;
    private Transform _Hand;
	private float _lastTimeClose;
	private bool _isFirstTimePlayed = true;


    #endregion
    #region Exposed

    [SerializeField]
    private CurrentSpawnerLocationScritpable _currentSpawnerLocation;


    [Header("camera")]
    public LayerMask IgnoreMe;

	[Header("camera")]
	public LayerMask IgnoreMeSpade;

	[Header("Score")]
    [SerializeField]
    private IntVariable _score;
    private HandManager _hm;

	[Header("Rotation Speed")]
	[SerializeField]
	private float _RotationSpeed = 1000;

	
    #endregion



    #region Unity API
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        cam = Camera.main;
        _Hand = this.gameObject.transform.parent.transform.parent.transform.parent.transform;
        _hm = FindObjectOfType<HandManager>();
    }

    private void LateUpdate()
    {
        if (_isDragable)
        {
            Drag();
        }
        if (Input.GetMouseButtonDown(1) )
        {

			
            if (_Hand.gameObject.activeSelf)
            {
				_Hand.gameObject.SetActive(false);
            }
            else
            {
				reset();

            }

		}

        if (_isGhost)
        {
			RotateObject();

        }
    }
    void OnMouseDown()
    {
        
        if (!(_isBusy))
        {
            
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            _isDragable = true;
            _isBusy = true;
            GetComponent<Seeding>().SetIsSelected(true);
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
			CardScriptable cs = GetComponent<Cards>().GetCardScriptable();

			Seeding seed = GetComponent<Seeding>();

			if (Physics.Raycast(ray, out hit))
			{
				
				
				if (hit.transform.tag == "CardsBackground")
				{
					_transform.position = _transform.parent.position;
				}
				else if (hit.transform.tag == "UnBuild" || hit.transform.tag == "Plants")
				{
					seed.UpdateRenderer(2);

					this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
					_lastTimeUnbuild = Time.time;


				}
				else if (hit.transform.tag == "EffectZone")
				{
					seed.UpdateRenderer(1);
					effectcollider = hit.collider;
					this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);

				}
				else if (hit.transform.tag == "Cards")
				{
                    if (_isFirstTimePlayed)
                    {
						this.transform.position = new Vector3(0, 0, 0);
						_isFirstTimePlayed = false;
					}
					//this.transform.rotation = Quaternion.Euler(0, 0, 0);
					
					RotateObject();
				}

				else
				{
					seed.UpdateRenderer(0);
					this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
					
				}


			}

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
			{

				GrowPlants gp = hit.transform.GetComponentInParent<GrowPlants>();
				if ((hit.transform.tag == "Plants" || (hit.transform.tag == "EffectZone")) && Vector3.Distance(hit.point, transform.position) < 0.5f)
				{

					if (cs._isWaterCan && Input.GetMouseButtonDown(0))
					{

						GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
						go.transform.Rotate(0, _transform.rotation.eulerAngles.y, 0);
					}
					if (cs._IsBasket)
					{

						seed.SetIsBuidable(false);
						if (Input.GetMouseButtonDown(0))
						{
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
						
							go.transform.Rotate(0, _transform.rotation.eulerAngles.y, 0);
						}

					}
					if (cs._isInsectPollinator && (gp.GetCurrentTier() < gp.GetMaxTier()))
					{
						seed.SetIsBuidable(false);
						if (Input.GetMouseButtonDown(0))
						{
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
							go.transform.Rotate(0, _transform.rotation.eulerAngles.y, 0);
						}
					}

				}
				if (hit.transform.GetComponentInParent<Building>() && cs._isBuilding)
				{
					seed.SetIsBuidable(false);
					if (Input.GetMouseButtonDown(0))
					{
						_score.Value -= hit.transform.GetComponentInParent<Building>().GetBonusDiversity();
						Destroy(hit.transform.parent.gameObject);
					}

				}
				if (hit.transform.tag == "Unbuild" && cs._isPlant)
				{
					seed.SetIsBuidable(false);
					return;
				}

			}
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMeSpade))
			{
				
				if (cs._isShovel)
				{
					
					if (hit.transform.GetComponentInParent<Plants>() || hit.transform.GetComponentInParent<Building>())
					{
						Plants p = hit.transform.GetComponentInParent<Plants>();
						seed.SetIsBuidable(false);
						if (Input.GetMouseButtonDown(0))
						{
							
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
							_score.Value -=p.getBonusMalus();
							p.GetGroundLayering().DeletePlants();
							p.NoticeOtherAboutDestruction();
							FindObjectOfType<PlantsManager>().DeletePlantInMapList(hit.transform.parent.gameObject);
							Destroy(hit.transform.parent.gameObject);
							seed.AddLimitation();
							Destroy(go,5f);
						}

					}
			
					if ((hit.transform.parent.GetComponent<GroundLayering>()))
                    {
				
                        if (!hit.transform.parent.GetComponent<GroundLayering>().IsPlantsOn() && Input.GetMouseButtonDown(0))
                        {
							
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
							Destroy(hit.transform.gameObject);
							Destroy(go, 5f);

                        }
					}

				}
				
			}
		}

    }

    public void reset()
    {
        if (_isGhost)
        {

            _Hand.gameObject.SetActive(true);
            
            Destroy(_transform.gameObject);
            _hm.ChangeHand();
            
        }
        
    }

	private void Close()
    {
		_Hand.gameObject.SetActive(true);

		Destroy(_transform.gameObject);
		_hm.ChangeHand();
	}

    public void SetGhost(bool b)
    {
        _isGhost = b;
    }

	public void RotateObject()
	{

		if (Input.GetKey(KeyCode.T))
		{

			_transform.Rotate(Vector3.up * Time.deltaTime *_RotationSpeed ,Space.World);
		}
		if (Input.GetKey(KeyCode.R))
		{
			_transform.Rotate(Vector3.down * Time.deltaTime * _RotationSpeed, Space.World); ;
		}

	
	}

	public Quaternion GetRotation()
    {




		return Quaternion.Euler(_transform.rotation.eulerAngles);
    }
    #endregion

}