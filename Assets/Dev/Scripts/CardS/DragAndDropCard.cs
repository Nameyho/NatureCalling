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

    [SerializeField]
    private CurrentSpawnerLocationScritpable _currentSpawnerLocation;


    [Header("camera")]
    public LayerMask IgnoreMe;

    [Header("Score")]
    [SerializeField]
    private IntVariable _score;
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


                } else if (hit.transform.tag == "EffectZone")
                {
                    seed.UpdateRenderer(1);
                    effectcollider = hit.collider;
                    this.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);

                } else if (hit.transform.tag == "Cards")
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
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
                if ((hit.transform.tag == "Plants" || (hit.transform.tag == "EffectZone")) && Vector3.Distance(hit.point, transform.position) < 1)
                {
                    if (cs._isWaterCan && Input.GetMouseButtonDown(0))
                    {
                        Instantiate(cs._prefabToSpawn, hit.point, Quaternion.identity);

                    }
                    if (cs._IsBasket && (gp.GetCurrentTier() == gp.GetMaxTier()))
                    {
                        seed.SetIsBuidable(false);
                        if (Input.GetMouseButtonDown(0))
                        {
                            Instantiate(cs._prefabToSpawn, hit.point, Quaternion.identity);
                        }

                    }
                    if (cs._isInsectPollinator && (gp.GetCurrentTier() < gp.GetMaxTier()))
                    {
                        seed.SetIsBuidable(false);
                        if (Input.GetMouseButtonDown(0))
                        {
                            Instantiate(cs._prefabToSpawn, hit.point, Quaternion.identity);
                        }
                    }

                    if (cs._isShovel)
                    {
                       
                        if (hit.transform.GetComponentInParent<Plants>())
                        {
                            seed.SetIsBuidable(false);
                            if (Input.GetMouseButtonDown(0))
                            {
                                _score.Value -= hit.transform.GetComponentInParent<Plants>().getBonusMalus();
                                Destroy(hit.transform.parent.gameObject);
                            }

                        }

                    }
                }
                if (hit.transform.GetComponentInParent<Building>()&& cs._isShovel)
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

        }
    }

    private void reset()
    {
        if (_isGhost)
        {

            _Hand.gameObject.SetActive(true);
            _isBusy = false;
            _transform.SetParent(_currentSpawnerLocation._SpawnerTransform);
            transform.localPosition = Vector3.zero;
            _transform.localRotation = Quaternion.Euler(0, 0, 0);
            _transform.localScale = Vector3.one;
            _isDragable = false;
            GetComponent<Seeding>().SetIsSelected(false);
        }
        
    }


    public void SetGhost(bool b)
    {
        _isGhost = b;
    }
    #endregion

}