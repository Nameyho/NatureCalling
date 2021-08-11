using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ButtonTest : MonoBehaviour
{


    public PlantPhase _phase;
    public IntVariable number ;
    public GameObject _plant;
    public void GrowPlant()
    {
       
        
        if (number < _phase.PhaseAmount){
            number.Value++;
            Debug.Log(number.Value);
        GrowPlants gp = _plant.GetComponent<GrowPlants>();
        gp.SetCurrentTier(number);

        }
    }

    public void ShrinkPlant()
    {
        if (number >= 1)
        {
            number.Value--;
            GrowPlants gp = _plant.GetComponent<GrowPlants>();
            gp.SetCurrentTier(number);

        }
    }

    private void Start()
    {
        number.Value = 0;
    }
}
