using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class CardBackgroundManager : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private GameObject _hand;

    [SerializeField]
    private CurrentSpawnerLocationScritpable _currentSpawnerLocation;
    #endregion


    #region Unity API
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Cards>())
        {
            Debug.Log("je suis sorti");
          other.GetComponent<Cards>().TransformIntoGhostModel();
            other.GetComponent<DragAndDropCard>().SetGhost(true);
            Cards cards = other.GetComponent<Cards>();
            
            _currentSpawnerLocation._SpawnerTransform = cards.transform.parent; 
            cards.transform.SetParent(cards.transform.parent.transform.parent.transform.parent.transform.parent.transform);
             _hand.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<Cards>())
        {
            other.GetComponent<DragAndDropCard>().SetGhost(false);
            other.GetComponent<Cards>().TransformIntoCard();
        }
    }

    #endregion
}
