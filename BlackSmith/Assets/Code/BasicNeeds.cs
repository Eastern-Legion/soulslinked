using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNeeds : MonoBehaviour
{

    CharacterController CharacterController;


    [Header("Basic Needs")]
	public int Health = 100;
    public int Stamina = 50;
    public int Magika = 10;
    public int hunger = 10;
    int _WeapNum;
    CapsuleCollider capCollider;

    public bool isstruck; 

    public bool isEquipped = false;

    void Awake()
    {
        capCollider = GetComponent<CapsuleCollider>();
        CharacterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
      HPmanager();
      ManaManager();
      StaminaManager();
      HungerManager();
    }

    void OnTriggerEnter(Collider collide)
    {
        if(collide.transform.name.Contains("Hitbox") && !collide.transform.IsChildOf(this.gameObject.transform))
		{
			CharacterController.Struck();
            StartCoroutine(CharacterController._Staggered(0.5f));
		}
    }
    void attackmanager(int Weap)
    {
        Weap = _WeapNum;
        //
        {
            StartCoroutine(CharacterController._SwitchWeapon(Weap));
        }
        
    }

    private void HPmanager()
    {
        //regen
        //damaged
        if(capCollider )
        {
        }
    }

    private void StaminaManager()
    {
        
    }

    private void ManaManager()
    {
        
    }


        private void HungerManager()
    {
        
    }
}
