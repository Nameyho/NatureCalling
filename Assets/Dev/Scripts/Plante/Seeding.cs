using ScriptableObjectArchitecture;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class Seeding : MonoBehaviour
{
    #region Exposed

    [Header("Materials")]
    [SerializeField]
    private Material[] _Basic = new Material[2];

    [SerializeField]
    private Material[] _Effect = new Material[2];

    [SerializeField]
    private Material[] _Unbuildable = new Material[2] ;

    [SerializeField]
    private GameObject _ghostModel;


    [SerializeField]
    private GameObject _visualPlantVFX;

    [SerializeField]
    private GameObject _GoodVFX;

    [SerializeField]
    private GameObject _BugHostelFVX;

    [SerializeField]
    private AudioSource _audioSource;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _plantsPrefabs;
    

    [Header("camera")]
    public LayerMask IgnoreMe;
   

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    [Header("Limitation")]

    [SerializeField]
    private IntVariable _remainingCards;


    [SerializeField]
    private TextMeshPro _textLimitation;

    [SerializeField]
    private float _CoolddownLayering;


    [SerializeField]
    private AudioClip[] _sound;
    #endregion

    #region Private

    private Renderer _myRend;
    private bool _isBuildable = false;
    private Camera cam;
    private bool _isSelected;
    private DragAndDropCard _DaD;
    private float _lastLayeringPlant;
    private bool _isLayeringNotAlreadyPlant = true;
    private GameObject VFX;


    GameObject go = null;

    bool _isBuildableVFX  = true;
    bool _isUnbuildableVFX = true;
    VisualEffect _vfxPlants;
    #endregion

    #region Unity API


    private void Update()
    {
       
        onClick();
        UpdateTextLimitation();
        if (go )
        {
            go.transform.position = transform.position;
        }
    }


    public void UpdateRenderer(int i)
    {
        if (!go && _visualPlantVFX)
        {
         go = Instantiate(_visualPlantVFX, transform.position, Quaternion.identity);
        _vfxPlants = go.GetComponent<VisualEffect>();

        }
        switch (i)
        {
            case 1:

                _myRend.materials = _Effect;
                Debug.Log(_myRend.materials[0]);
                _isBuildable = true;


              
                if (_isBuildableVFX && go)
                {
                    _isBuildableVFX = false;
                    _isUnbuildableVFX = true;
                    _vfxPlants.Reinit();
                    _vfxPlants.SendEvent("Plantable");
              

                }
                break;


            case 2:
                // _myRend.material = _Unbuildable;

                _myRend.materials = _Unbuildable;
                _isBuildable = false;

                if (_isUnbuildableVFX && go)
                {
                 
                    _isUnbuildableVFX = false;
                    _isBuildableVFX = true;
                    _vfxPlants.Reinit();
                    _vfxPlants.SendEvent("NotPlantable");
                }
                break;
            case 0:
                //  _myRend.material = _Basic;
                Debug.Log("basic");
                 _myRend.materials = _Basic;

                if (_isBuildableVFX && go)
                {
                    _isBuildableVFX = false;
                    _isUnbuildableVFX = true;
                    _vfxPlants.Reinit();
                    _vfxPlants.SendEvent("Plantable");
                }

                _isBuildable = true;
                break;
        }
       

    }



    private void Awake()
    {
        _myRend = _ghostModel.GetComponent<Renderer>();
        cam = Camera.main;
        _DaD = GetComponent<DragAndDropCard>();


    }


    private void OnDestroy()
    {
        if (go)
        {
            Destroy(go);
        }
    }
    #endregion


    #region Methods

    private void onClick()
    {


       
        if (_remainingCards)
        {

            if (_remainingCards.Value > 0)
            {

                SeedingObject();
             

            }
        }
        else
        {
            SeedingObject();
        }
    }


    private void SeedingObject()
    {

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        CardScriptable cs = GetComponent<Cards>().GetCardScriptable();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))


                if (Input.GetMouseButtonDown(0) && _isBuildable && _isSelected)
                {

                    if (hit.transform.tag == "Layering" && cs._isPlant )
                    {


                        if (hit.transform.parent.GetComponent<GroundLayering>().IsLayeringBuildable())
                        {
                            GameObject plant = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                            plant.transform.Rotate(0, _DaD.GetRotationY(), 0);
                            if (plant.GetComponent<Plants>())
                            {
                                //plant.GetComponent<Plants>().GetAllPlants();
                                plant.GetComponent<Plants>().SetGroundLayering(hit.transform.parent.GetComponent<GroundLayering>());
                                _gameManager.AddProgression(cs._bonusBioDiversity);
                                hit.transform.parent.GetComponent<GroundLayering>().AddPlants();

                            GameObject plantVFX = Instantiate(_visualPlantVFX, transform.position,Quaternion.identity);
                             plantVFX.GetComponent<VisualEffect>().SendEvent("Planted");
                             int rand = Random.Range(0, _sound.Length);
                             _audioSource.clip = _sound[rand];
                             _audioSource.Play();
                                Destroy(plantVFX, 5f);

                            }

                        
                    }
                   
                    }
                    if (hit.transform.tag == "AquaticPlants" && cs._isAquaticPlant)
                    {
                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        _gameManager.AddProgression(cs._bonusBioDiversity);

                    int rand = Random.Range(0, _sound.Length);
                    _audioSource.clip = _sound[rand];
                  

                    GameObject plantVFX = Instantiate(_visualPlantVFX, transform.position, Quaternion.identity);
                    plantVFX.GetComponent<VisualEffect>().SendEvent("Planted");
                    Destroy(plantVFX, 5f);
                    _audioSource.Play();
                    }
                    if (!cs._isAquaticPlant && !cs._isPlant && !cs._isBuilding &&
                    !cs._isShovel && !cs._isLayering  && !cs._isBugHostel && 
                    !cs._isHenHouse && !cs._isInsectPollinator &&  !cs._isHen
                    && !cs._IsBasket && !cs._isWaterCan &&
                    hit.transform.tag == "BuildingZone")
                    {
                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        GameObject vfx = Instantiate(_GoodVFX, hit.point, Quaternion.identity);
                        Destroy(vfx, 5f);
                    
                        _remainingCards.Value--;
          
                    _gameManager.AddProgression(cs._bonusBioDiversity);

                    }
                if (cs._isBugHostel && hit.transform.tag == "BuildingZone")
                {
                    GameObject vfx = Instantiate(_BugHostelFVX, hit.point, Quaternion.identity);
                    FindObjectOfType<GameManager>().SetisHive(true);
                    Destroy(vfx, 5f);
                }

               
                

                //if (cs._isWaterCan && hit.transform.tag == "Plants")
                //{
                //    GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                //    go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                //    //Destroy(go);
                //}

                if (cs._isInsectPollinator && hit.transform.tag == "Plants" && FindObjectOfType<GameManager>().GetIsHive())
                    {
                   
                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                         go.transform.Rotate(0, _DaD.GetRotationY(), 0);

                }

                if (cs._isHen && hit.transform.tag == "Plants" && FindObjectOfType<GameManager>().GetisHenHouse())
                {

                   // GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    go.transform.Rotate(0, _DaD.GetRotationY(), 0);

                }
                if (cs._isBuilding)
                    {
                    GameObject vfx = Instantiate(_GoodVFX, hit.point, Quaternion.identity);
                    Destroy(vfx, 5f);
                    GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        _remainingCards.Value--;
                        _gameManager.AddProgression(cs._bonusBioDiversity);
                    if (cs._isHenHouse )
                    {
                        FindObjectOfType<GameManager>().SetisHenHouse(true);
                    }

                    }

                    if ((cs._isLayering & Time.time - _lastLayeringPlant > _CoolddownLayering) || (_isLayeringNotAlreadyPlant && cs._isLayering))
                    {
                        if (hit.transform.tag == "BuildingZone")
                        {
                            GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                            GameObject vfx = Instantiate(_GoodVFX, hit.point, Quaternion.identity);
                             Destroy(vfx, 5f);
                            go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                            _remainingCards.Value--;

                  
                        _gameManager.AddProgression(cs._bonusBioDiversity);
                            _lastLayeringPlant = Time.time;
                            _isLayeringNotAlreadyPlant = false;

                        }

                    }



                    //GetComponent<Cards>().PlayThisCard();


                }

        
    }

    #endregion

    #region public
    public void UpdateTextLimitation()
    {
        if (_remainingCards)
        {
            DragAndDropCard dad = GetComponent<DragAndDropCard>();
        _textLimitation.text = _remainingCards.Value.ToString();
          

        }
        else
        {
            _textLimitation.text = "∞";
        }
    }

    public void SetIsSelected(bool f)
    {
        _isSelected = f;
    }


    public void SetIsBuidable(bool b)
    {
        _isBuildable = b;
    }

    public void AddLimitation()
    {
        _remainingCards.Value++;
    }

    public IntVariable GetLimitation()
    {
        return _remainingCards;
    }
  
    public void AddRemainingCards()
    {
        _remainingCards.Value++;
    }
    #endregion
}
