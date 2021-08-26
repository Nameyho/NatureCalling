using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Meteos/Nuages", order = 1)]
public class CloudScriptable : ScriptableObject
{

    public GameObject cloudGameObject;

    public float chanceToRain;
}
