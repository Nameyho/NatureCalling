using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPlants : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown()
    {
    Plants _plant = GetComponentInParent<Plants>();
 

        _plant.ShowComplementaity();
        _plant.showPlantsAround();
    }

    private void OnMouseExit()
    {
        Plants _plant = GetComponentInParent<Plants>();
        
        _plant.DisableVFX();
        _plant.HidePlantsAround();
    }
}

