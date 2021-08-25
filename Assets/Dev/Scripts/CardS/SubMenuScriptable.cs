using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Cartes_Configuration/Sous_menu", order = 1)]

public class SubMenuScriptable : ScriptableObject
{

    public List<CardScriptable> _listSubMenu = new List<CardScriptable>();
}
