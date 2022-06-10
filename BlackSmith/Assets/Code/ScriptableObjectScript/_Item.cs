using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum __ItemType
{
    _WEAPON,
    _POTION,
    _INGREDIENT,
    _ARMOR,
    _EQUIPMENT,
    _MISC
}

public abstract class _Item : ScriptableObject
{
    public new string name;
    public __ItemType Type;
    [TextArea(15,20)]
    public string description;
    public GameObject prefab;

    //use for inventory system later on
    public int item_Width = 1;
    public int item_Height = 1;
    public int goldprice = 100;

}
