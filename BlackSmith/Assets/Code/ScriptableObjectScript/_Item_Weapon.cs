using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Weapon_Num
{
    	//function to switch weapons
	UNARMED = 0,
	TWOH_SWORD = 1,
	TWOH_SPEAR = 2,
	TWOH_AXE = 3,
	TWOH_BOW = 4,
	TWOH_CROSSBOW = 5,
	STAFF = 6,
	SHIELD = 7,
	L_SWORD = 8,
	R_SWORD = 9,
	L_Mace = 10,
	R_Mace = 11,
	L_Dagger =12,
	R_Dagger =13,
	L_Item = 14,
	R_Item =15,
	L_Pistol = 16,
	R_Pistol=17,
	Rifle= 18,
	R_Spear = 19,
	TWOH_Club =20
}

[CreateAssetMenu(fileName = "WeaponObject", menuName = "SoulLinked/Item/Weapon")]
public class _Item_Weapon : _Item
{
    public int damage;
    public Weapon_Num Weapon_Num;

    public Vector3 HandleRotation;
    public Vector3 HandleLocation;

    void Awake()
    {
        Type = __ItemType._WEAPON;
    }
}
