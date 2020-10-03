using UnityEngine;
using System.Collections;

public class GUIControls : MonoBehaviour{
	PlayerController PlayerController;
	[HideInInspector]
	public bool blockGui;
	bool blockToggle;
	public bool useNavAgent;

	void Start(){
		PlayerController = GetComponent<PlayerController>();
	}

	public void EndClimbing(){
		PlayerController.CharacterState = CharacterState.DEFAULT;
		PlayerController.gravity = PlayerController.gravityTemp;
		PlayerController.rb.useGravity = true;
		PlayerController.animator.applyRootMotion = false;
		PlayerController.canMove = true;
		PlayerController.isClimbing = false;
	}

	void OnGUI(){
		//Set blocking in controller
		if(blockGui){
			PlayerController.isBlocking = true;
		}
		else{
			PlayerController.isBlocking = false;
		}
		if(!PlayerController.isDead){
			if(PlayerController.weapon == Weapon.RELAX || PlayerController.weapon != Weapon.UNARMED){
				if(GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed")){
					PlayerController.animator.SetBool("Relax", false);
					PlayerController.isRelax = false;
					StartCoroutine(PlayerController._SwitchWeapon(0));
					PlayerController.weapon = Weapon.UNARMED;
					PlayerController.canAction = true;
					PlayerController.animator.SetTrigger("RelaxTrigger");
				}
				if(!PlayerController.isSitting && !PlayerController.isMoving && PlayerController.weapon == Weapon.RELAX){
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Sit")){
						PlayerController.canAction = false;
						PlayerController.isSitting = true;
						PlayerController.canMove = false;
						PlayerController.animator.SetInteger("Idle", 1);
						PlayerController.animator.SetTrigger("IdleTrigger");
					}
					if(GUI.Button(new Rect(1115, 380, 100, 30), "Sleep")){
						PlayerController.canAction = false;
						PlayerController.isSitting = true;
						PlayerController.canMove = false;
						PlayerController.animator.SetInteger("Idle", 2);
						PlayerController.animator.SetTrigger("IdleTrigger");
					}
				}
				if(PlayerController.isSitting && !PlayerController.isMoving && PlayerController.weapon == Weapon.RELAX){
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Stand")){
						PlayerController.canAction = false;
						PlayerController.isSitting = false;
						PlayerController.animator.SetInteger("Idle", 0);
						PlayerController.animator.SetTrigger("IdleTrigger");
						PlayerController.canMove = true;
					}
				}
			}
			if(PlayerController.canAction && !PlayerController.isRelax){
				if(PlayerController.isGrounded){
					//crossbow can't block
					if(PlayerController.weapon != Weapon.TWOHANDCROSSBOW){
						//if character is not blocking
						blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
						if(blockGui){
							PlayerController.isBlocking = true;
							PlayerController.animator.SetBool("Blocking", true);
							if(blockToggle == false){
								PlayerController.animator.SetTrigger("BlockTrigger");
								blockToggle = true;
							}
						}
						else{
							PlayerController.isBlocking = false;
							PlayerController.animator.SetBool("Blocking", false);
							blockToggle = false;
						}

					}
					//get hit
					if(blockGui){
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							StartCoroutine(PlayerController._BlockHitReact());
						}
						if(GUI.Button(new Rect(30, 270, 100, 30), "Block Break")){
							StartCoroutine(PlayerController._BlockBreak());
						}
					}
					else if(!PlayerController.isBlocking){
						//Rolling
						if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward")){
							PlayerController.targetDashDirection = transform.forward;
							StartCoroutine(PlayerController._Roll(1));
						}
						if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward")){
							PlayerController.targetDashDirection = -transform.forward;
							StartCoroutine(PlayerController._Roll(3));
						}
						if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left")){
							PlayerController.targetDashDirection = -transform.right;
							StartCoroutine(PlayerController._Roll(4));
						}
						if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right")){
							PlayerController.targetDashDirection = transform.right;
							StartCoroutine(PlayerController._Roll(2));
						}
						//Dodging
						if(GUI.Button(new Rect(235, 15, 100, 30), "Dodge Left")){
							StartCoroutine(PlayerController._Dodge(1));
						}
						if(GUI.Button(new Rect(235, 45, 100, 30), "Dodge Right")){
							StartCoroutine(PlayerController._Dodge(2));
						}
						//Turning
						if(GUI.Button(new Rect(340, 15, 100, 30), "Turn Left")){
							StartCoroutine(PlayerController._Turning(1));
						}
						if(GUI.Button(new Rect(340, 45, 100, 30), "Turn Right")){
							StartCoroutine(PlayerController._Turning(2));
						}
						//ATTACK LEFT
						if(PlayerController.weapon == Weapon.SHIELD || PlayerController.weapon == Weapon.RIFLE || PlayerController.weapon != Weapon.ARMED || (PlayerController.weapon == Weapon.ARMED && PlayerController.leftWeapon != 0) && PlayerController.leftWeapon != 7){
							if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L")){
								PlayerController.Attack(1);
							}
						}
						//ATTACK RIGHT
						if(PlayerController.weapon == Weapon.RIFLE || PlayerController.weapon != Weapon.ARMED || (PlayerController.weapon == Weapon.ARMED && PlayerController.rightWeapon != 0) || PlayerController.weapon == Weapon.ARMEDSHIELD){
							if(PlayerController.weapon != Weapon.SHIELD){
								if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R")){
									PlayerController.Attack(2);
								}
							}
						}
						//ATTACK DUAL
						if(PlayerController.leftWeapon > 7 && PlayerController.rightWeapon > 7 && PlayerController.leftWeapon != 14){
							if(PlayerController.rightWeapon != 15){
								if((PlayerController.leftWeapon != 16 && PlayerController.rightWeapon != 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										PlayerController.Attack(3);
									}
								}
								else if((PlayerController.leftWeapon == 16 && PlayerController.rightWeapon == 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										PlayerController.Attack(3);
									}
								}
							}
						}
						//Kicking
						if(GUI.Button(new Rect(25, 115, 100, 30), "Left Kick")){
							PlayerController.AttackKick(1);
						}
						if(GUI.Button(new Rect(130, 115, 100, 30), "Right Kick")){
							PlayerController.AttackKick(2);
						}
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							PlayerController.GetHit();
						}
						//Weapon Switching
						if(PlayerController.weapon == Weapon.UNARMED && !PlayerController.isMoving){
							if(GUI.Button(new Rect(1115, 310, 100, 30), "Relax")){
								PlayerController.animator.SetBool("Relax", true);
								PlayerController.isRelax = true;
								PlayerController.weapon = Weapon.RELAX;
								PlayerController.canAction = false;
								PlayerController.animator.SetTrigger("RelaxTrigger");
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDSWORD){
							if(GUI.Button(new Rect(1115, 350, 100, 30), "2 Hand Sword")){
								StartCoroutine(PlayerController._SwitchWeapon(1));
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDCLUB){
							if(GUI.Button(new Rect(1000, 350, 100, 30), "2 Hand Club")){
								StartCoroutine(PlayerController._SwitchWeapon(20));
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDSPEAR){
							if(GUI.Button(new Rect(1115, 380, 100, 30), "2 Hand Spear")){
								StartCoroutine(PlayerController._SwitchWeapon(2));
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDAXE){
							if(GUI.Button(new Rect(1115, 410, 100, 30), "2 Hand Axe")){
								StartCoroutine(PlayerController._SwitchWeapon(3));
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDBOW){
							if(GUI.Button(new Rect(1115, 440, 100, 30), "2 Hand Bow")){
								StartCoroutine(PlayerController._SwitchWeapon(4));
							}
						}
						if(PlayerController.weapon != Weapon.TWOHANDCROSSBOW){
							if(GUI.Button(new Rect(1115, 470, 100, 30), "Crossbow")){
								StartCoroutine(PlayerController._SwitchWeapon(5));
							}
						}
						if(PlayerController.weapon != Weapon.RIFLE){
							if(GUI.Button(new Rect(1000, 470, 100, 30), "Rifle")){
								StartCoroutine(PlayerController._SwitchWeapon(18));
							}
						}
						if(PlayerController.weapon != Weapon.STAFF){
							if(GUI.Button(new Rect(1115, 500, 100, 30), "Staff")){
								StartCoroutine(PlayerController._SwitchWeapon(6));
							}
						}
						if(PlayerController.leftWeapon != 7){
							if(GUI.Button(new Rect(1115, 700, 100, 30), "Shield")){
								StartCoroutine(PlayerController._SwitchWeapon(7));
							}
						}
						if(PlayerController.leftWeapon != 8){
							if(GUI.Button(new Rect(1065, 540, 100, 30), "Left Sword")){
								StartCoroutine(PlayerController._SwitchWeapon(8));
							}
						}
						if(PlayerController.rightWeapon != 9){
							if(GUI.Button(new Rect(1165, 540, 100, 30), "Right Sword")){
								StartCoroutine(PlayerController._SwitchWeapon(9));
							}
						}
						if(PlayerController.leftWeapon != 10){
							if(GUI.Button(new Rect(1065, 570, 100, 30), "Left Mace")){
								StartCoroutine(PlayerController._SwitchWeapon(10));
							}
						}
						if(PlayerController.rightWeapon != 11){
							if(GUI.Button(new Rect(1165, 570, 100, 30), "Right Mace")){
								StartCoroutine(PlayerController._SwitchWeapon(11));
							}
						}
						if(PlayerController.leftWeapon != 12){
							if(GUI.Button(new Rect(1065, 600, 100, 30), "Left Dagger")){
								StartCoroutine(PlayerController._SwitchWeapon(12));
							}
						}
						if(PlayerController.leftWeapon != 13){
							if(GUI.Button(new Rect(1165, 600, 100, 30), "Right Dagger")){
								StartCoroutine(PlayerController._SwitchWeapon(13));
							}
						}
						if(PlayerController.leftWeapon != 14){
							if(GUI.Button(new Rect(1065, 630, 100, 30), "Left Item")){
								StartCoroutine(PlayerController._SwitchWeapon(14));
							}
						}
						if(PlayerController.leftWeapon != 15){
							if(GUI.Button(new Rect(1165, 630, 100, 30), "Right Item")){
								StartCoroutine(PlayerController._SwitchWeapon(15));
							}
						}
						if(PlayerController.leftWeapon != 16){
							if(GUI.Button(new Rect(1065, 660, 100, 30), "Left Pistol")){
								StartCoroutine(PlayerController._SwitchWeapon(16));
							}
						}
						if(PlayerController.leftWeapon != 17){
							if(GUI.Button(new Rect(1165, 660, 100, 30), "Right Pistol")){
								StartCoroutine(PlayerController._SwitchWeapon(17));
							}
						}
						if(PlayerController.rightWeapon != 19){
							if(GUI.Button(new Rect(1000, 380, 100, 30), "1 Hand Spear")){
								StartCoroutine(PlayerController._SwitchWeapon(19));
							}
						}
						
					}
				}
				if(PlayerController.canJump){
					if(PlayerController.isGrounded){
						if(GUI.Button(new Rect(25, 165, 100, 30), "Jump")){
							if(PlayerController.canJump){
								StartCoroutine(PlayerController._Jump());
							}
							if(GUI.Button(new Rect(175, 165, 100, 30), "PickupTrigger")){
								PlayerController.Pickup();
							}
						}
					}
				}
				if(!blockGui && !PlayerController.isBlocking && PlayerController.isGrounded){
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death")){
						StartCoroutine(PlayerController._Death());
					}
					if(PlayerController.weapon != Weapon.ARMED){
						if(GUI.Button(new Rect(130, 165, 100, 30), "Pickup")){
							PlayerController.Pickup();
						}
						if(GUI.Button(new Rect(235, 165, 100, 30), "Activate")){
							PlayerController.Activate();
						}
					}
					else if(PlayerController.weapon == Weapon.ARMED){
						if(PlayerController.leftWeapon != 0 && PlayerController.rightWeapon != 0){
						}
						else{
							if(GUI.Button(new Rect(130, 165, 100, 30), "Pickup")){
								PlayerController.Pickup();
							}
							if(GUI.Button(new Rect(235, 165, 100, 30), "Activate")){
								PlayerController.Activate();
							}
						}
					}
				}
				//Climbing
				if(!blockGui && !PlayerController.isBlocking && PlayerController.isGrounded && PlayerController.CharacterState != CharacterState.CLIMBING && PlayerController.isNearLadder){
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb")){
						PlayerController.gravityTemp = PlayerController.gravity;
						PlayerController.gravity = 0;
						PlayerController.rb.useGravity = false;
						PlayerController.animator.applyRootMotion = true;
						PlayerController.animator.SetTrigger("Climb-On-BottomTrigger");
						//Get the direction of the ladder, and snap the character to the correct position and facing
						Vector3 newVector = Vector3.Cross(PlayerController.ladder.transform.forward, PlayerController.ladder.transform.right);
						Debug.DrawRay(PlayerController.ladder.transform.position, newVector, Color.red, 2f);
						Vector3 newSpot = PlayerController.ladder.transform.position + (newVector.normalized * 0.71f);
						transform.position = new Vector3(newSpot.x, 0, newSpot.z);
						transform.rotation = Quaternion.Euler(transform.rotation.x, PlayerController.ladder.transform.rotation.eulerAngles.y, transform.rotation.z);
						PlayerController.canMove = false;
						PlayerController.Invoke("Climbing", 1.05f);
					}
				}
				if(PlayerController.CharacterState == CharacterState.CLIMBING){
					if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top")){
						PlayerController.animator.applyRootMotion = true;
						PlayerController.animator.SetTrigger("Climb-Off-TopTrigger");
						Invoke("EndClimbing", 2.6f);
					}
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up")){
						PlayerController.animator.applyRootMotion = true;
						PlayerController.animator.SetTrigger("Climb-UpTrigger");
					}
					if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down")){
						PlayerController.animator.applyRootMotion = true;
						PlayerController.animator.SetTrigger("Climb-DownTrigger");
					}
				}
			}
			//Special attack
			if(!PlayerController.isRelax && PlayerController.isGrounded){
				if(PlayerController.weapon == Weapon.TWOHANDSWORD){
					if(GUI.Button(new Rect(235, 85, 100, 30), "Special Attack1")){
						PlayerController.Special(1);
					}
				}
				//Casting Armed and Staff
				if(PlayerController.weapon == Weapon.ARMED || PlayerController.weapon == Weapon.STAFF || PlayerController.weapon == Weapon.UNARMED){
					if(GUI.Button(new Rect(25, 330, 100, 30), "Cast Atk Left")){
						if(!PlayerController.isCasting){
							PlayerController.Cast(1, "attack");
						}
						else{
							PlayerController.Cast(0, "attack");
						}
					}
					if(PlayerController.weapon != Weapon.STAFF){
						if(GUI.Button(new Rect(130, 330, 100, 30), "Cast Atk Right")){
							if(!PlayerController.isCasting){
								PlayerController.Cast(2, "attack");
							}
							else{
								PlayerController.Cast(0, "attack");
							}
						}
						if(GUI.Button(new Rect(80, 365, 100, 30), "Cast Atk Dual")){
							if(!PlayerController.isCasting){
								PlayerController.Cast(3, "attack");
							}
							else{
								PlayerController.Cast(0, "attack");
							}
						}
					}
					if(GUI.Button(new Rect(25, 425, 100, 30), "Cast AOE")){
						if(!PlayerController.isCasting){
							PlayerController.Cast(4, "AOE");
						}
						else{
							PlayerController.Cast(0, "AOE");
						}
					}
					if(GUI.Button(new Rect(25, 400, 100, 30), "Cast Buff")){
						if(!PlayerController.isCasting){
							PlayerController.Cast(4, "buff");
						}
						else{
							PlayerController.Cast(0, "buff");
						}
					}
					if(GUI.Button(new Rect(25, 450, 100, 30), "Cast Summon")){
						if(!PlayerController.isCasting){
							PlayerController.Cast(4, "summon");
						}
						else{
							PlayerController.Cast(0, "summon");
						}
					}
				}
			}
			//Climbing while Relaxed
			if(!blockGui && !PlayerController.isBlocking && PlayerController.isGrounded && PlayerController.CharacterState != CharacterState.CLIMBING && PlayerController.isNearLadder){
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb")){
					PlayerController.gravityTemp = PlayerController.gravity;
					PlayerController.gravity = 0;
					PlayerController.rb.useGravity = false;
					PlayerController.animator.applyRootMotion = true;
					PlayerController.animator.SetTrigger("Climb-On-BottomTrigger");
					Invoke("Climbing", 1.05f);
				}
			}
			if(PlayerController.CharacterState == CharacterState.CLIMBING){
				if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top")){
					PlayerController.animator.applyRootMotion = true;
					PlayerController.animator.SetTrigger("Climb-Off-TopTrigger");
					Invoke("EndClimbing", 2.6f);
				}
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up")){
					PlayerController.animator.applyRootMotion = true;
					PlayerController.animator.SetTrigger("Climb-UpTrigger");
				}
				if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down")){
					PlayerController.animator.applyRootMotion = true;
					PlayerController.animator.SetTrigger("Climb-DownTrigger");
				}
			}
		}
		if(PlayerController.isDead){
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive")){
				StartCoroutine(PlayerController._Revive());
			}
		}
		//Use NavMesh
		useNavAgent = GUI.Toggle(new Rect(25, 500, 100, 30), useNavAgent, "Use NavAgent");
		if(useNavAgent){
			PlayerController.useMeshNav = true;
			PlayerController.navMeshAgent.enabled = true;
		}
		else{
			PlayerController.useMeshNav = false;
			PlayerController.navMeshAgent.enabled = false;
		}
	}
}