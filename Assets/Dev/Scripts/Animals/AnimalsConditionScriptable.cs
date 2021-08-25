using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(fileName = "Data", menuName = "Animaux_Configuration/Conditions", order = 1)]
public class AnimalsConditionScriptable : ScriptableObject
{
    
    public GameObject animalGameObject;

    public BoolVariable[] conditions;

    public int ScoreToReach;

    public float PercentageToSpawn; 

}
