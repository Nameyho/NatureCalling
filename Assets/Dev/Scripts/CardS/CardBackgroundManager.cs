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

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ButtonScriptData>())
        {
            other.GetComponent<ButtonScriptData>().TransformIntoCard();

        }
    }
}
