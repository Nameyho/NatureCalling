using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine.VFX;

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
	private float AxeY;
	private float lastvfx;
	private bool firsttimewatercan =true;
	private GameObject go;
	private VisualEffect ve;
	private bool PlantNeededToBewateredAround = false;
	private bool _isLockedByFlip = false;
	private bool PlantNeedToBePollen = false;
	private float _lastrestvfx;

	public List<Plants> _plantsCompatibleInCurrentrange = new List<Plants>();

	public List<Plants> _PlantToActivateFX = new List<Plants>();


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
	[Range(0,1)]
	private float _RotationSpeed = 1000;


	[Header("VFX AND SFX")]

	public AudioSource _audioSource;

	public GameObject _vfxWaterCan;
	public AudioClip[] _WatercanSounds;

	public GameObject _vfxBasket;
	public AudioClip[] _BasketSounds;

	public GameObject _vfxPollinator;
	public AudioClip[] _pollinatorSounds;

	public GameObject _vfxHen;

	public GameObject _badFVX;
	
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
				FindObjectOfType<TabGroup>().SetIdle();
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

    private void OnDestroy()
    {
		Destroy(go);
    }
    void OnMouseDown()
    {
            if (!_isLockedByFlip)
            {
			  _isDragable = true;

            }
        if (!(_isBusy))
        {
	
            
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
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
            if (FindObjectOfType<GameManager>().GetFlipped()){
				CardScriptable cs = GetComponent<Cards>().GetCardScriptable();

			

                if (!(GetComponentInChildren<FlipCard>().Equals(FindObjectOfType<GameManager>().GetFlipped()))){
					FindObjectOfType<GameManager>().GetFlipped().UnflipComplete();

				}
			}
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

				//arrosoir
				if (cs._isWaterCan)
				{
					if (firsttimewatercan && !PlantNeededToBewateredAround)
					{
						go = Instantiate(_vfxWaterCan, _transform.position, Quaternion.identity);
						firsttimewatercan = false;
						ve = go.GetComponent<VisualEffect>();
						ve.SendEvent("NotWaterable");
					
					}

					go.transform.position = _transform.position;
					CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
					Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					int around = 0;
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].GetComponent<CapsuleCollider>())
						{
						CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
						Plants plantLocal = hc.GetComponentInParent<Plants>();
                            if (plantLocal)
                            {
								if (plantLocal.CanBeWatered())
								{
									around++;
								
								}

                            }
							
						}
					}
					if (around > 0)
					{
						PlantNeededToBewateredAround = true;
                    }
                    else
                    {
						PlantNeededToBewateredAround = false;
                    }

				}

				if (cs._isWaterCan && PlantNeededToBewateredAround)
				{
					
					ve.SendEvent("Waterable");
					if (Input.GetMouseButtonDown(0))
					{
						GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);

						go.transform.Rotate(0, AxeY, 0);


						CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
						Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					
						for (int i = 0; i < hits.Length; i++)
						{
							if (hits[i].GetComponent<CapsuleCollider>())
							{
								CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
								Plants plantLocal = hc.GetComponentInParent<Plants>();

								if (plantLocal)
								{
									if (plantLocal.CanBeWatered())
									{
										GameObject wt = Instantiate(_vfxWaterCan, plantLocal.transform.position, Quaternion.identity);
										wt.GetComponent<VisualEffect>().SendEvent("Watering");
										int random = Random.Range(0, _WatercanSounds.Length);

										_audioSource.clip = _WatercanSounds[random];
										_audioSource.Play();
										Destroy(wt, 5f);

									}

								}

							}
						}



					}
					lastvfx = Time.time;




				}


				//panier
                if (cs._IsBasket)
                {
					if (firsttimewatercan && !PlantNeededToBewateredAround)
					{
						go = Instantiate(_vfxBasket, _transform.position, Quaternion.identity);
						firsttimewatercan = false;
						ve = go.GetComponent<VisualEffect>();
						ve.SendEvent("NotRecoltable");

					}

					go.transform.position = _transform.position;
					CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
					Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					int around = 0;
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].GetComponent<CapsuleCollider>())
						{
							CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
							Plants plantLocal = hc.GetComponentInParent<Plants>();
							if (plantLocal)
							{
								if (plantLocal.GetComponentInParent<GrowPlants>().isFullingGrown())
								{
									around++;

								}

							}

						}
					}
					if (around > 0)
					{
						PlantNeededToBewateredAround = true;
					}
					else
					{
						PlantNeededToBewateredAround = false;
					}
				}

				if (cs._IsBasket && PlantNeededToBewateredAround)
				{

					ve.SendEvent("Recoltable");
					if (Input.GetMouseButtonDown(0))
					{
						GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);

						go.transform.Rotate(0, AxeY, 0);


						CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
						Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					
						for (int i = 0; i < hits.Length; i++)
						{
							if (hits[i].GetComponent<CapsuleCollider>())
							{
								CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
								Plants plantLocal = hc.GetComponentInParent<Plants>();

								if (plantLocal)
								{
									if (plantLocal.GetComponentInParent<GrowPlants>().isFullingGrown())
									{
										GameObject wt = Instantiate(_vfxBasket, plantLocal.transform.position, Quaternion.identity);
										wt.GetComponent<VisualEffect>().SendEvent("Recolted");
										int random = Random.Range(0, _BasketSounds.Length);

										_audioSource.clip =_BasketSounds[random];
										_audioSource.Play();
										Destroy(wt, 5f);

									}

								}

							}
						}



					}
					lastvfx = Time.time;




				}



				//polleniser

				if (cs._isInsectPollinator && FindObjectOfType<GameManager>().GetIsHive())
				{
					if (firsttimewatercan && !PlantNeedToBePollen )
					{
						go = Instantiate(_vfxPollinator, _transform.position, Quaternion.identity);
						firsttimewatercan = false;
						ve = go.GetComponent<VisualEffect>();
						ve.SendEvent("NotPollinisable");

					}

					go.transform.position = _transform.position;
					CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
					Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					int around = 0;
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].GetComponent<CapsuleCollider>())
						{
							CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
							Plants plantLocal = hc.GetComponentInParent<Plants>();
							if (plantLocal)
							{
								if (plantLocal.CanBePollinisate())
								{
									around++;

								}

							}

						}
					}
					if (around > 0)
					{
						PlantNeedToBePollen = true;
					}
					else
					{
						PlantNeedToBePollen = false;
					}
				}

				if (cs._isInsectPollinator && PlantNeedToBePollen   && FindObjectOfType<GameManager>().GetIsHive())
				{
				
					ve.SendEvent("Pollinisable");
					if (Input.GetMouseButtonDown(0))
					{
						GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);

						go.transform.Rotate(0, AxeY, 0);


						CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
						Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
						
						for (int i = 0; i < hits.Length; i++)
						{
							if (hits[i].GetComponent<CapsuleCollider>())
							{
								CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
								Plants plantLocal = hc.GetComponentInParent<Plants>();

								if (plantLocal)
								{
									if (plantLocal.CanBePollinisate())
									{
										GameObject wt = Instantiate(_vfxPollinator, plantLocal.transform.position, Quaternion.identity);
										wt.GetComponent<VisualEffect>().SendEvent("Pollinised");
										int random = Random.Range(0, _pollinatorSounds.Length);

										_audioSource.clip = _pollinatorSounds[random];
										_audioSource.Play();
										Destroy(wt, 5f);

									}

								}

							}
						}



					}
					lastvfx = Time.time;




				}


				//desinfester

				if (cs._isHen && FindObjectOfType<GameManager>().GetisHenHouse())
				{
					if (firsttimewatercan && !PlantNeedToBePollen)
					{
						go = Instantiate(_vfxHen, _transform.position, Quaternion.identity);
						firsttimewatercan = false;
						ve = go.GetComponent<VisualEffect>();
						ve.SendEvent("NotDeBuggable");

					}

					go.transform.position = _transform.position;
					CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
					Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
					int around = 0;
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].GetComponent<CapsuleCollider>())
						{
							CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
							Plants plantLocal = hc.GetComponentInParent<Plants>();
							if (plantLocal)
							{
								if (plantLocal.GetInfested())
								{
									around++;

								}

							}

						}
					}
					if (around > 0)
					{
						PlantNeedToBePollen = true;
					}
					else
					{
						PlantNeedToBePollen = false;
					}
				}

				if (cs._isHen && PlantNeedToBePollen && FindObjectOfType<GameManager>().GetisHenHouse())
				{
                   
						ve.SendEvent("DeBuggable");
					
                    
					if (Input.GetMouseButtonDown(0))
					{
						//GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);

						//go.transform.Rotate(0, AxeY, 0);


						CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
						Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);
				
						for (int i = 0; i < hits.Length; i++)
						{
							if (hits[i].GetComponent<CapsuleCollider>())
							{
								CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
								Plants plantLocal = hc.GetComponentInParent<Plants>();

								if (plantLocal)
								{
									if (plantLocal.GetInfested())
									{
										FindObjectOfType<GameManager>().getHen().AddDestination(plantLocal);




										//int random = Random.Range(0, _pollinatorSounds.Length);

										//_audioSource.clip = _pollinatorSounds[random];
										//_audioSource.Play();

									}

								}

							}
						}



					}
					lastvfx = Time.time;




				}



				//Detection compatibilié

				if (cs._isPlant || cs._isAquaticPlant)
				{
				   List<CardScriptable> cartecompatible =  cs._prefabToSpawn.GetComponent<Plants>().GetCompatibilite();

					_plantsCompatibleInCurrentrange = new List<Plants>();
					CapsuleCollider cc = GetComponentInChildren<CapsuleCollider>();
					Collider[] hits = Physics.OverlapSphere(_transform.position, 1.24f);

                    for (int i = 0; i < hits.Length; i++)
                    {
						if (hits[i].GetComponent<CapsuleCollider>())
						{
							CapsuleCollider hc = hits[i].GetComponent<CapsuleCollider>();
							Plants plantLocal = hc.GetComponentInParent<Plants>();

							if (!_plantsCompatibleInCurrentrange.Contains(plantLocal) &&plantLocal && cartecompatible.Count>0 )
							{
                                for (int j = 0; j < cartecompatible.Count;j++)
                                {
                                    if (cartecompatible[j]._CardName.Equals(plantLocal.GetCard()._CardName))
                                    {
										_plantsCompatibleInCurrentrange.Add(plantLocal);
										_PlantToActivateFX.Add(plantLocal);
                                    }
                                }
							}
						}
                    }
                    for (int i = 0; i < _PlantToActivateFX.Count; i++)
                    {
                        if (_plantsCompatibleInCurrentrange.Contains(_PlantToActivateFX[i]))
                        {
							_PlantToActivateFX[i].ActivateFx();
                        }
                        else
                        {
							_PlantToActivateFX[i].DisableVFX();                        }
                    }

				}



				_transform.rotation = hit.transform.rotation;

				Quaternion target = Quaternion.Euler(0, AxeY, 0);
				_transform.GetChild(1).localRotation = Quaternion.Slerp(transform.rotation,target,/*Time.deltaTime **/_RotationSpeed) ;

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
				Plants plante = hit.transform.GetComponentInParent<Plants>();

				if ((hit.transform.tag == "Plants" || (hit.transform.tag == "EffectZone")) || (hit.transform.tag == "HarvestAquatic") && Vector3.Distance(hit.point, transform.position) < 0.5f)
				{
					
					
					if (cs._isInsectPollinator && (gp.GetCurrentTier() < gp.GetMaxTier()) && plante.CanBePollinisate() && FindObjectOfType<GameManager>().GetIsHive())
					{
						seed.SetIsBuidable(false);
						if (Input.GetMouseButtonDown(0))
						{
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
							go.transform.Rotate(0, AxeY, 0);
						}
					}

                    if (cs._isHen && plante.GetInfested() && FindObjectOfType<GameManager>().GetisHenHouse() && Input.GetMouseButtonDown(0))
                    {

                       // FindObjectOfType<GameManager>().getHen().AddDestination(plante);
                    }
                }

                // ?????
                //if (hit.transform.GetComponentInParent<Building>() && cs._isBuilding)
                //{
                //	seed.SetIsBuidable(false);
                //	if (Input.GetMouseButtonDown(0))
                //	{
                //		_score.Value -= hit.transform.GetComponentInParent<Building>().GetBonusDiversity();
                //		Destroy(hit.transform.parent.gameObject);
                //	}

                //}
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
                            if (hit.transform.GetComponentInParent<Beehive>())
                            {
								FindObjectOfType<GameManager>().SetisHive(false);
                            }
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
                            if (p )
                            {
							p.NoticeOtherAboutDestruction();
							FindObjectOfType<PlantsManager>().DeletePlantInMapList(hit.transform.parent.gameObject);
							_score.Value -= hit.transform.parent.GetComponent<Plants>().GetCard()._bonusBioDiversity;

                            }
                            if (p)
                            {
								if (p.GetCard()._isPlant && !  p.GetCard()._isAquaticPlant)
								{
								p.GetGroundLayering().DeletePlants();

								}

                            }
							Destroy(hit.transform.parent.gameObject);
							GameObject vfx = Instantiate(_badFVX, hit.point, Quaternion.identity);
							Destroy(vfx, 1f);
							Destroy(go,1f);
						}

					}
			
					if ((hit.transform.parent.GetComponent<GroundLayering>()))
                    {
				
                        if (!hit.transform.parent.GetComponent<GroundLayering>().IsPlantsOn() && Input.GetMouseButtonDown(0))
                        {
							
							GameObject go = Instantiate(cs._prefabToSpawn, hit.point, _transform.rotation);
							hit.transform.parent.GetComponentInParent<GroundLayering>().AddRemaining();
							Destroy(hit.transform.gameObject);
							GameObject vfx = Instantiate(_badFVX, hit.point, Quaternion.identity);
							Destroy(vfx, 1f);
							Destroy(go, 1f);

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

    public void SetGhost(bool b)
    {
        _isGhost = b;
    }

	public void RotateObject()
	{

		if (Input.GetKey(KeyCode.T))
		{
			AxeY++;
			//_transform.Rotate(Vector3.up * Time.deltaTime *_RotationSpeed ,Space.World);

		}
		if (Input.GetKey(KeyCode.R))
		{
			AxeY--;
			//_transform.Rotate(Vector3.down * Time.deltaTime * _RotationSpeed, Space.World); 
		}

	
	}

	public float GetRotationY()
    {
		return AxeY;
    }

	public void SetIsLockedByFlip(bool b)
    {
		_isLockedByFlip = b;
    }
	private void Update()
	{

		if (ve)
        {
		
            if ((Time.time> _lastrestvfx) && (lastvfx + 0.2f > Time.time) )
            {
			
				ve.SendEvent("NotWaterable");
				ve.SendEvent("NotRecoltable");
				ve.SendEvent("NotPollinisable");
				ve.SendEvent("NotDeBuggable");
            }

		}
    }
    #endregion

}