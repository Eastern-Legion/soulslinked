using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    _WEAPON,
    _POTION,
    _INGREDIENT,
    _ARMOR,
    _EQUIPMENT,
    _MISC
}

public abstract class ItemManager : ScriptableObject
{
    public new string name;
    public ItemType Type;
    [TextArea(15,20)]
    public string description;
    public GameObject prefab;
}
