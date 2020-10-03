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

    void Awake()
    {
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

    private void HungerManager()
    {
        
    }

    private void StaminaManager()
    {
        
    }

    private void ManaManager()
    {
        
    }

    private void HPmanager()
    {
        
    }
}
