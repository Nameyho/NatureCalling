using ScriptableObjectArchitecture;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{
    #region Exposed

    [Header("Prefab")]
    [SerializeField]
    private GameObject _plantPrefab;

    [Header("Scriptable Object")]
    [SerializeField]
    private ObjectVariable _plantSelectectedScriptable;

    #endregion

    #region Methodes

    public void OnClick()
    {
        _plantSelectectedScriptable.Value = _plantPrefab;
    }

    #endregion
}
