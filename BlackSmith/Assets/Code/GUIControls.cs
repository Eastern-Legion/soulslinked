using UnityEngine;
using System.Collections;

public class GUIControls : MonoBehaviour{
	CharacterController CharacterController;
	[HideInInspector]
	public bool blockGui;
	bool blockToggle;
	public bool useNavAgent;

	void Start(){
		CharacterController = GetComponent<CharacterController>();
	}

	public void EndClimbing(){
		CharacterController.CharacterState = CharacterState.DEFAULT;
		CharacterController.gravity = CharacterController.gravityTemp;
		CharacterController.rb.useGravity = true;
		CharacterController.animator.applyRootMotion = false;
		CharacterController.canMove = true;
		CharacterController.isClimbing = false;
	}

	void OnGUI(){
		//Set blocking in controller
		if(blockGui){
			CharacterController.isBlocking = true;
		}
		else{
			CharacterController.isBlocking = false;
		}
		if(!CharacterController.isDead){
			if(CharacterController.weapon == Weapon.RELAX || CharacterController.weapon != Weapon.UNARMED){
				if(GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed")){
					CharacterController.animator.SetBool("Relax", false);
					CharacterController.isRelax = false;
					StartCoroutine(CharacterController._SwitchWeapon(0));
					CharacterController.weapon = Weapon.UNARMED;
					CharacterController.canAction = true;
					CharacterController.animator.SetTrigger("RelaxTrigger");
				}
				if(!CharacterController.isSitting && !CharacterController.isMoving && CharacterController.weapon == Weapon.RELAX){
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Sit")){
						CharacterController.canAction = false;
						CharacterController.isSitting = true;
						CharacterController.canMove = false;
						CharacterController.animator.SetInteger("Idle", 1);
						CharacterController.animator.SetTrigger("IdleTrigger");
					}
					if(GUI.Button(new Rect(1115, 380, 100, 30), "Sleep")){
						CharacterController.canAction = false;
						CharacterController.isSitting = true;
						CharacterController.canMove = false;
						CharacterController.animator.SetInteger("Idle", 2);
						CharacterController.animator.SetTrigger("IdleTrigger");
					}
				}
				if(CharacterController.isSitting && !CharacterController.isMoving && CharacterController.weapon == Weapon.RELAX){
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Stand")){
						CharacterController.canAction = false;
						CharacterController.isSitting = false;
						CharacterController.animator.SetInteger("Idle", 0);
						CharacterController.animator.SetTrigger("IdleTrigger");
						CharacterController.canMove = true;
					}
				}
			}
			if(CharacterController.canAction && !CharacterController.isRelax){
				if(CharacterController.isGrounded){
					//crossbow can't block
					if(CharacterController.weapon != Weapon.TWOHANDCROSSBOW){
						//if character is not blocking
						blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
						if(blockGui){
							CharacterController.isBlocking = true;
							CharacterController.animator.SetBool("Blocking", true);
							if(blockToggle == false){
								CharacterController.animator.SetTrigger("BlockTrigger");
								blockToggle = true;
							}
						}
						else{
							CharacterController.isBlocking = false;
							CharacterController.animator.SetBool("Blocking", false);
							blockToggle = false;
						}

					}
					//get hit
					if(blockGui){
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							StartCoroutine(CharacterController._BlockHitReact());
						}
						if(GUI.Button(new Rect(30, 270, 100, 30), "Block Break")){
							StartCoroutine(CharacterController._BlockBreak());
						}
					}
					else if(!CharacterController.isBlocking){
						//Rolling
						if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward")){
							CharacterController.targetDashDirection = transform.forward;
							StartCoroutine(CharacterController._Roll(1));
						}
						if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward")){
							CharacterController.targetDashDirection = -transform.forward;
							StartCoroutine(CharacterController._Roll(3));
						}
						if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left")){
							CharacterController.targetDashDirection = -transform.right;
							StartCoroutine(CharacterController._Roll(4));
						}
						if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right")){
							CharacterController.targetDashDirection = transform.right;
							StartCoroutine(CharacterController._Roll(2));
						}
						//Dodging
						if(GUI.Button(new Rect(235, 15, 100, 30), "Dodge Left")){
							StartCoroutine(CharacterController._Dodge(1));
						}
						if(GUI.Button(new Rect(235, 45, 100, 30), "Dodge Right")){
							StartCoroutine(CharacterController._Dodge(2));
						}
						//Turning
						if(GUI.Button(new Rect(340, 15, 100, 30), "Turn Left")){
							StartCoroutine(CharacterController._Turning(1));
						}
						if(GUI.Button(new Rect(340, 45, 100, 30), "Turn Right")){
							StartCoroutine(CharacterController._Turning(2));
						}
						//ATTACK LEFT
						if(CharacterController.weapon == Weapon.SHIELD || CharacterController.weapon == Weapon.RIFLE || CharacterController.weapon != Weapon.ARMED || (CharacterController.weapon == Weapon.ARMED && CharacterController.leftWeapon != 0) && CharacterController.leftWeapon != 7){
							if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L")){
								CharacterController.Attack(1);
							}
						}
						//ATTACK RIGHT
						if(CharacterController.weapon == Weapon.RIFLE || CharacterController.weapon != Weapon.ARMED || (CharacterController.weapon == Weapon.ARMED && CharacterController.rightWeapon != 0) || CharacterController.weapon == Weapon.ARMEDSHIELD){
							if(CharacterController.weapon != Weapon.SHIELD){
								if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R")){
									CharacterController.Attack(2);
								}
							}
						}
						//ATTACK DUAL
						if(CharacterController.leftWeapon > 7 && CharacterController.rightWeapon > 7 && CharacterController.leftWeapon != 14){
							if(CharacterController.rightWeapon != 15){
								if((CharacterController.leftWeapon != 16 && CharacterController.rightWeapon != 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										CharacterController.Attack(3);
									}
								}
								else if((CharacterController.leftWeapon == 16 && CharacterController.rightWeapon == 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										CharacterController.Attack(3);
									}
								}
							}
						}
						//Kicking
						if(GUI.Button(new Rect(25, 115, 100, 30), "Left Kick")){
							CharacterController.AttackKick(1);
						}
						if(GUI.Button(new Rect(130, 115, 100, 30), "Right Kick")){
							CharacterController.AttackKick(2);
						}
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							
						}
						//Weapon Switching
						if(CharacterController.weapon == Weapon.UNARMED && !CharacterController.isMoving){
							if(GUI.Button(new Rect(1115, 310, 100, 30), "Relax")){
								CharacterController.animator.SetBool("Relax", true);
								CharacterController.isRelax = true;
								CharacterController.weapon = Weapon.RELAX;
								CharacterController.canAction = false;
								CharacterController.animator.SetTrigger("RelaxTrigger");
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDSWORD){
							if(GUI.Button(new Rect(1115, 350, 100, 30), "2 Hand Sword")){
								StartCoroutine(CharacterController._SwitchWeapon(1));
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDCLUB){
							if(GUI.Button(new Rect(1000, 350, 100, 30), "2 Hand Club")){
								StartCoroutine(CharacterController._SwitchWeapon(20));
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDSPEAR){
							if(GUI.Button(new Rect(1115, 380, 100, 30), "2 Hand Spear")){
								StartCoroutine(CharacterController._SwitchWeapon(2));
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDAXE){
							if(GUI.Button(new Rect(1115, 410, 100, 30), "2 Hand Axe")){
								StartCoroutine(CharacterController._SwitchWeapon(3));
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDBOW){
							if(GUI.Button(new Rect(1115, 440, 100, 30), "2 Hand Bow")){
								StartCoroutine(CharacterController._SwitchWeapon(4));
							}
						}
						if(CharacterController.weapon != Weapon.TWOHANDCROSSBOW){
							if(GUI.Button(new Rect(1115, 470, 100, 30), "Crossbow")){
								StartCoroutine(CharacterController._SwitchWeapon(5));
							}
						}
						if(CharacterController.weapon != Weapon.RIFLE){
							if(GUI.Button(new Rect(1000, 470, 100, 30), "Rifle")){
								StartCoroutine(CharacterController._SwitchWeapon(18));
							}
						}
						if(CharacterController.weapon != Weapon.STAFF){
							if(GUI.Button(new Rect(1115, 500, 100, 30), "Staff")){
								StartCoroutine(CharacterController._SwitchWeapon(6));
							}
						}
						if(CharacterController.leftWeapon != 7){
							if(GUI.Button(new Rect(1115, 700, 100, 30), "Shield")){
								StartCoroutine(CharacterController._SwitchWeapon(7));
							}
						}
						if(CharacterController.leftWeapon != 8){
							if(GUI.Button(new Rect(1065, 540, 100, 30), "Left Sword")){
								StartCoroutine(CharacterController._SwitchWeapon(8));
							}
						}
						if(CharacterController.rightWeapon != 9){
							if(GUI.Button(new Rect(1165, 540, 100, 30), "Right Sword")){
								StartCoroutine(CharacterController._SwitchWeapon(9));
							}
						}
						if(CharacterController.leftWeapon != 10){
							if(GUI.Button(new Rect(1065, 570, 100, 30), "Left Mace")){
								StartCoroutine(CharacterController._SwitchWeapon(10));
							}
						}
						if(CharacterController.rightWeapon != 11){
							if(GUI.Button(new Rect(1165, 570, 100, 30), "Right Mace")){
								StartCoroutine(CharacterController._SwitchWeapon(11));
							}
						}
						if(CharacterController.leftWeapon != 12){
							if(GUI.Button(new Rect(1065, 600, 100, 30), "Left Dagger")){
								StartCoroutine(CharacterController._SwitchWeapon(12));
							}
						}
						if(CharacterController.leftWeapon != 13){
							if(GUI.Button(new Rect(1165, 600, 100, 30), "Right Dagger")){
								StartCoroutine(CharacterController._SwitchWeapon(13));
							}
						}
						if(CharacterController.leftWeapon != 14){
							if(GUI.Button(new Rect(1065, 630, 100, 30), "Left Item")){
								StartCoroutine(CharacterController._SwitchWeapon(14));
							}
						}
						if(CharacterController.leftWeapon != 15){
							if(GUI.Button(new Rect(1165, 630, 100, 30), "Right Item")){
								StartCoroutine(CharacterController._SwitchWeapon(15));
							}
						}
						if(CharacterController.leftWeapon != 16){
							if(GUI.Button(new Rect(1065, 660, 100, 30), "Left Pistol")){
								StartCoroutine(CharacterController._SwitchWeapon(16));
							}
						}
						if(CharacterController.leftWeapon != 17){
							if(GUI.Button(new Rect(1165, 660, 100, 30), "Right Pistol")){
								StartCoroutine(CharacterController._SwitchWeapon(17));
							}
						}
						if(CharacterController.rightWeapon != 19){
							if(GUI.Button(new Rect(1000, 380, 100, 30), "1 Hand Spear")){
								StartCoroutine(CharacterController._SwitchWeapon(19));
							}
						}
						
					}
				}
				if(CharacterController.canJump){
					if(CharacterController.isGrounded){
						if(GUI.Button(new Rect(25, 165, 100, 30), "Jump")){
							if(CharacterController.canJump){
								StartCoroutine(CharacterController._Jump());
							}
							if(GUI.Button(new Rect(175, 165, 100, 30), "PickupTrigger")){
								CharacterController.Pickup();
							}
						}
					}
				}
				if(!blockGui && !CharacterController.isBlocking && CharacterController.isGrounded){
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death")){
						StartCoroutine(CharacterController._Death());
					}
					if(CharacterController.weapon != Weapon.ARMED){
						if(GUI.Button(new Rect(130, 165, 100, 30), "Pickup")){
							CharacterController.Pickup();
						}
						if(GUI.Button(new Rect(235, 165, 100, 30), "Activate")){
							CharacterController.Activate();
						}
					}
					else if(CharacterController.weapon == Weapon.ARMED){
						if(CharacterController.leftWeapon != 0 && CharacterController.rightWeapon != 0){
						}
						else{
							if(GUI.Button(new Rect(130, 165, 100, 30), "Pickup")){
								CharacterController.Pickup();
							}
							if(GUI.Button(new Rect(235, 165, 100, 30), "Activate")){
								CharacterController.Activate();
							}
						}
					}
				}
				//Climbing
				if(!blockGui && !CharacterController.isBlocking && CharacterController.isGrounded && CharacterController.CharacterState != CharacterState.CLIMBING && CharacterController.isNearLadder){
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb")){
						CharacterController.gravityTemp = CharacterController.gravity;
						CharacterController.gravity = 0;
						CharacterController.rb.useGravity = false;
						CharacterController.animator.applyRootMotion = true;
						CharacterController.animator.SetTrigger("Climb-On-BottomTrigger");
						//Get the direction of the ladder, and snap the character to the correct position and facing
						Vector3 newVector = Vector3.Cross(CharacterController.ladder.transform.forward, CharacterController.ladder.transform.right);
						Debug.DrawRay(CharacterController.ladder.transform.position, newVector, Color.red, 2f);
						Vector3 newSpot = CharacterController.ladder.transform.position + (newVector.normalized * 0.71f);
						transform.position = new Vector3(newSpot.x, 0, newSpot.z);
						transform.rotation = Quaternion.Euler(transform.rotation.x, CharacterController.ladder.transform.rotation.eulerAngles.y, transform.rotation.z);
						CharacterController.canMove = false;
						CharacterController.Invoke("Climbing", 1.05f);
					}
				}
				if(CharacterController.CharacterState == CharacterState.CLIMBING){
					if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top")){
						CharacterController.animator.applyRootMotion = true;
						CharacterController.animator.SetTrigger("Climb-Off-TopTrigger");
						Invoke("EndClimbing", 2.6f);
					}
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up")){
						CharacterController.animator.applyRootMotion = true;
						CharacterController.animator.SetTrigger("Climb-UpTrigger");
					}
					if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down")){
						CharacterController.animator.applyRootMotion = true;
						CharacterController.animator.SetTrigger("Climb-DownTrigger");
					}
				}
			}
			//Special attack
			if(!CharacterController.isRelax && CharacterController.isGrounded){
				if(CharacterController.weapon == Weapon.TWOHANDSWORD){
					if(GUI.Button(new Rect(235, 85, 100, 30), "Special Attack1")){
						CharacterController.Special(1);
					}
				}
				//Casting Armed and Staff
				if(CharacterController.weapon == Weapon.ARMED || CharacterController.weapon == Weapon.STAFF || CharacterController.weapon == Weapon.UNARMED){
					if(GUI.Button(new Rect(25, 330, 100, 30), "Cast Atk Left")){
						if(!CharacterController.isCasting){
							CharacterController.Cast(1, "attack");
						}
						else{
							CharacterController.Cast(0, "attack");
						}
					}
					if(CharacterController.weapon != Weapon.STAFF){
						if(GUI.Button(new Rect(130, 330, 100, 30), "Cast Atk Right")){
							if(!CharacterController.isCasting){
								CharacterController.Cast(2, "attack");
							}
							else{
								CharacterController.Cast(0, "attack");
							}
						}
						if(GUI.Button(new Rect(80, 365, 100, 30), "Cast Atk Dual")){
							if(!CharacterController.isCasting){
								CharacterController.Cast(3, "attack");
							}
							else{
								CharacterController.Cast(0, "attack");
							}
						}
					}
					if(GUI.Button(new Rect(25, 425, 100, 30), "Cast AOE")){
						if(!CharacterController.isCasting){
							CharacterController.Cast(4, "AOE");
						}
						else{
							CharacterController.Cast(0, "AOE");
						}
					}
					if(GUI.Button(new Rect(25, 400, 100, 30), "Cast Buff")){
						if(!CharacterController.isCasting){
							CharacterController.Cast(4, "buff");
						}
						else{
							CharacterController.Cast(0, "buff");
						}
					}
					if(GUI.Button(new Rect(25, 450, 100, 30), "Cast Summon")){
						if(!CharacterController.isCasting){
							CharacterController.Cast(4, "summon");
						}
						else{
							CharacterController.Cast(0, "summon");
						}
					}
				}
			}
			//Climbing while Relaxed
			if(!blockGui && !CharacterController.isBlocking && CharacterController.isGrounded && CharacterController.CharacterState != CharacterState.CLIMBING && CharacterController.isNearLadder){
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb")){
					CharacterController.gravityTemp = CharacterController.gravity;
					CharacterController.gravity = 0;
					CharacterController.rb.useGravity = false;
					CharacterController.animator.applyRootMotion = true;
					CharacterController.animator.SetTrigger("Climb-On-BottomTrigger");
					Invoke("Climbing", 1.05f);
				}
			}
			if(CharacterController.CharacterState == CharacterState.CLIMBING){
				if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top")){
					CharacterController.animator.applyRootMotion = true;
					CharacterController.animator.SetTrigger("Climb-Off-TopTrigger");
					Invoke("EndClimbing", 2.6f);
				}
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up")){
					CharacterController.animator.applyRootMotion = true;
					CharacterController.animator.SetTrigger("Climb-UpTrigger");
				}
				if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down")){
					CharacterController.animator.applyRootMotion = true;
					CharacterController.animator.SetTrigger("Climb-DownTrigger");
				}
			}
		}
		if(CharacterController.isDead){
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive")){
				StartCoroutine(CharacterController._Revive());
			}
		}
		//Use NavMesh
		useNavAgent = GUI.Toggle(new Rect(25, 500, 100, 30), useNavAgent, "Use NavAgent");
		if(useNavAgent){
			CharacterController.useMeshNav = true;
			CharacterController.navMeshAgent.enabled = true;
		}
		else{
			CharacterController.useMeshNav = false;
			CharacterController.navMeshAgent.enabled = false;
		}
	}
}