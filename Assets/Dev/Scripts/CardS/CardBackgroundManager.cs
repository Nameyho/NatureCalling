using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackgroundManager : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ButtonScriptData>())
        {
          other.GetComponent<ButtonScriptData>().TransformIntoGhostModel();
            other.GetComponent<DragAndDropCard>().SetGhost(true);  
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ButtonScriptData>())
        {
            other.GetComponent<ButtonScriptData>().TransformIntoCard();
            other.GetComponent<DragAndDropCard>().SetGhost(false);
        }
    }
}
