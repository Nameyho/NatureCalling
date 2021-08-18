using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;


[CreateAssetMenu(fileName = "Data", menuName = "Cartes_Configuration/cartes", order = 1)]

public class CardScriptable : ScriptableObject
{
    [Header("Carte en elle même")]
    public string _CardName;

    public Sprite  _CardImage;

    public int _cost;

    [Header("bonus que la carte va apporter")]

    public GameObject _prefabToSpawn;


}
