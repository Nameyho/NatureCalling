using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    #region Exposed

    [Header("Maps détails in orders of apparition")]
    [SerializeField]
    private List<GameObject> _mapsParts = new List<GameObject>();

    #endregion

    #region private

    private int _indexOfLastPartSpawned;
    private Transform _transform;

    #endregion

    #region Methods

    public void SpawnNextPart()
    {
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();

        for (int i = 0; i < boxColliders.Length; i++)
        {
            Destroy(boxColliders[i]);
        }


        _indexOfLastPartSpawned++;
        if(_indexOfLastPartSpawned < _mapsParts.Count)
        {
            Instantiate(_mapsParts[_indexOfLastPartSpawned],_transform);
        }
    }
    #endregion
    #region Unity Api

    private void Start()
    {
        _indexOfLastPartSpawned = 0;
        _transform = GetComponent<Transform>();
    }

    #endregion


}
