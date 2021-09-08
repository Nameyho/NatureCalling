using ScriptableObjectArchitecture;
using UnityEngine;
using TMPro;

public class Seeding : MonoBehaviour
{
    #region Exposed

    [Header("Materials")]
    [SerializeField]
    private Material _Basic;

    [SerializeField]
    private Material _Effect;

    [SerializeField]
    private Material _Unbuildable;

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
    private IntVariable _cardLimitation;


    [SerializeField]
    private TextMeshPro _textLimitation;

    #endregion

    #region Private

    private Renderer _myRend;
    private bool _isBuildable = false;
    private Camera cam;
    private bool _isSelected;
    private DragAndDropCard _DaD;


    #endregion

    #region Unity API


    private void Update()
    {
		
		onClick();
        UpdateTextLimitation();
    }



    //private void OnTriggerStay(Collider other)
    //{

    //    if (other.CompareTag("UnBuild"))
    //    {
    //        Debug.Log("unbuild");
    //        _myRend.material = _Unbuildable;
    //        _isBuildable = false;
    //    }


    //}
    //private void OnTriggerExit(Collider other)
    //{

    //    Debug.Log("unbuild");
    //    if (other.CompareTag("UnBuild"))
    //    {
    //        _myRend.material = _Effect;
    //        _isBuildable = true;
    //    }
    //    else
    //    {
    //        Debug.Log("basic");
    //        _myRend.material = _Basic;
    //        _isBuildable = true;
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("EffectZone"))

    //    {
    //        Debug.Log("Effectzone");
    //        _myRend.material = _Effect;
    //        _isBuildable = true;
    //    }
    //}

    public void UpdateRenderer(int i)
    {
        switch (i)
        {
            case 1:
                _myRend.material = _Effect;
                _isBuildable = true;
                break;


            case 2:
                _myRend.material = _Unbuildable;
                _isBuildable = false;
                break;
            case 0:
                _myRend.material = _Basic;
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

    #endregion


    #region Methods

    private void onClick()
    {


        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        CardScriptable cs = GetComponent<Cards>().GetCardScriptable();

        if(_cardLimitation.Value > 0)
        {

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
  
            if (Input.GetMouseButtonDown(0)&& _isBuildable && _isSelected)
            {

                if (hit.transform.tag == "Layering" &&cs._isPlant)
                {
                    GameObject plant =  Instantiate(_plantsPrefabs,hit.point, Quaternion.identity);
                        
                    plant.transform.Rotate(0, _DaD.GetRotation().eulerAngles.y, 0);
                    if (plant.GetComponent<Plants>())
                    {
                        //plant.GetComponent<Plants>().GetAllPlants();
                        plant.GetComponent<Plants>().SetGroundLayering(hit.transform.parent.GetComponent<GroundLayering>());
                        _gameManager.AddProgression(cs._bonusBioDiversity);
                        hit.transform.parent.GetComponent<GroundLayering>().AddPlants();
                            _cardLimitation.Value--;
                    }

                }
                if (hit.transform.tag == "AquaticPlants" && cs._isAquaticPlant)
                {
                    GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    go.transform.Rotate(0, _DaD.GetRotation().eulerAngles.y, 0);
                    _gameManager.AddProgression(cs._bonusBioDiversity);

                }
                if( !cs._isAquaticPlant && !cs._isPlant && !cs._isBuilding && !cs._isShovel && hit.transform.tag=="BuildingZone")
                {
                     


                    GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    go.transform.Rotate(0, _DaD.GetRotation().eulerAngles.y, 0);
                    _gameManager.AddProgression(cs._bonusBioDiversity);
                        _cardLimitation.Value--;
                }
                if(cs._isWaterCan && hit.transform.tag == "Plants")
                {
                    WaterCan watercan = GetComponent<WaterCan>();
                  
                    GameObject go = Instantiate(_plantsPrefabs, hit.point, Quaternion.identity);
                    go.transform.Rotate(0, _DaD.GetRotation().eulerAngles.y, 0);
                    //Destroy(go);
                }
                if(cs._isBuilding )
                {
                    _DaD.GetRotation();
                    GameObject go =Instantiate(_plantsPrefabs, hit.point,Quaternion.identity);
                    go.transform.Rotate(0,_DaD.GetRotation().eulerAngles.y,0);
                        _cardLimitation.Value--;
                        _gameManager.AddProgression(cs._bonusBioDiversity);
                }
                
             


                //GetComponent<Cards>().PlayThisCard();


            }

        }
    }

    #endregion

    #region public
    public void UpdateTextLimitation()
    {
     
        _textLimitation.text = _cardLimitation.Value.ToString();
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
        _cardLimitation.Value++;
    }

    public IntVariable GetLimitation()
    {
        return _cardLimitation;
    }
   
    #endregion
}
