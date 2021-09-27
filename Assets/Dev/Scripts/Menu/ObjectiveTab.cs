using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectiveTab : MonoBehaviour, IPointerExitHandler
{
    [SerializeField]
    private ObjectiveButton objectivebuton;

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!objectivebuton.getLocked())
        {
      
        this.gameObject.SetActive(false);

        }
    }


  
}
