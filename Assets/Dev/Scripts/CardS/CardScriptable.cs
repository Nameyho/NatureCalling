using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.UI;


[CreateAssetMenu(fileName = "Data", menuName = "Cartes_Configuration/cartes", order = 1)]

public class CardScriptable : ScriptableObject
{
    [Header("Carte en elle même")]
    public string _CardName;

    public Sprite  _CardImage;

    public GameObject _cardModel;

    public bool _isPlant;

    public bool _isAquaticPlant;

    public bool _isWaterCan;

    public bool _IsBasket;

    public bool _isShovel;

    public bool _isBuilding;

    public bool _isInsectPollinator;

    [Header("bonus que la carte va apporter")]

    public GameObject _prefabToSpawn;

    public int _bonusBioDiversity;
}
