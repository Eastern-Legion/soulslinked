using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Weapon_T
{
	UNARMED = 0,
	TWOHANDSWORD = 1,
	TWOHANDSPEAR = 2,
	TWOHANDAXE = 3,
	TWOHANDBOW = 4,
	TWOHANDCROSSBOW = 5,
	STAFF = 6,
	ARMED = 7,
	RELAX = 8,
	RIFLE = 9,
	TWOHANDCLUB = 10,
	SHIELD = 11,
	ARMEDSHIELD = 12
}
public class ItemWeaponManager : ItemManager
{
    public int damage;
    public Weapon_T Weapon_T;

    public Vector3 HandleRotation;
    public Vector3 HandleLocation;


}
