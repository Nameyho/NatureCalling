using ScriptableObjectArchitecture;
using UnityEngine;
using TMPro;

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

    #endregion

    #region Private

    private MeshRenderer _myRend;
    private bool _isBuildable = false;
    private Camera cam;
    private bool _isSelected;
    private DragAndDropCard _DaD;
    private float _lastLayeringPlant;
    private bool _isLayeringNotAlreadyPlant = true;

    #endregion

    #region Unity API


    private void Update()
    {
		
		onClick();
        UpdateTextLimitation();
    }


    public void UpdateRenderer(int i)
    {
        switch (i)
        {
            case 1:

                _myRend.materials = _Effect;
                _isBuildable = true;
                break;


            case 2:
                // _myRend.material = _Unbuildable;

                _myRend.materials = _Unbuildable;
                _isBuildable = false;
                break;
            case 0:
                //  _myRend.material = _Basic;

                _myRend.materials = _Basic;
                _isBuildable = true;
                break;
        }
            

    }



    private void Awake()
    {
        _myRend = _ghostModel.GetComponent<MeshRenderer>();
        cam = Camera.main;
        _DaD = GetComponent<DragAndDropCard>();
        
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

                    if (hit.transform.tag == "Layering" && cs._isPlant)
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

                            }

                        }


                    }
                    if (hit.transform.tag == "AquaticPlants" && cs._isAquaticPlant)
                    {
                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        _gameManager.AddProgression(cs._bonusBioDiversity);

                    }
                    if (!cs._isAquaticPlant && !cs._isPlant && !cs._isBuilding && !cs._isShovel && !cs._isLayering && hit.transform.tag == "BuildingZone")
                    {
                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        _remainingCards.Value--;
                        _gameManager.AddProgression(cs._bonusBioDiversity);

                    }
                    if (cs._isWaterCan && hit.transform.tag == "Plants")
                    {
                        WaterCan watercan = GetComponent<WaterCan>();

                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        //Destroy(go);
                    }
                    if (cs._isBuilding)
                    {

                        GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                        go.transform.Rotate(0, _DaD.GetRotationY(), 0);
                        _remainingCards.Value--;
                        _gameManager.AddProgression(cs._bonusBioDiversity);
                    }

                    if ((cs._isLayering & Time.time - _lastLayeringPlant > _CoolddownLayering) || (_isLayeringNotAlreadyPlant && cs._isLayering))
                    {
                        if (hit.transform.tag == "BuildingZone")
                        {
                            GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
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
