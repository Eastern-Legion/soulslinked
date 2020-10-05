using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputExtender;
public class PlayerController : CharacterController
{
	public  float InputScroll;
    public GameObject Weaponobject;
	
	public GameObject playergameObject;
	void _Modechange()
	{
		if (Key_Extender.IsLongKeyPress(KeyCode.E))
		{
				if (!isRelax)
				{
					animator.SetBool("Relax", true);
					isRelax = true;
					weapon = Weapon.RELAX;
					canAction = false;
					animator.SetTrigger("RelaxTrigger");
				}
				else
				{
					animator.SetBool("Relax", false);
					isRelax = false;
					StartCoroutine(_SwitchWeapon(0));
					weapon = Weapon.UNARMED;
					canAction = true;
					animator.SetTrigger("RelaxTrigger");
				}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			EquipWeapon1();
		}

	}

/* 	void UnEquip()
    {

        Weaponobject.transform.parent = null;
        Weaponobject.GetComponent<Rigidbody>().useGravity = true;
        Weaponobject.GetComponent<Rigidbody>().isKinematic = false;
        Weaponobject.GetComponent<BoxCollider>().isTrigger = false;
    } */
    void EquipWeapon1()
    {
        
        Weaponobject.GetComponent<Rigidbody>().useGravity = false;
        Weaponobject.GetComponent<Rigidbody>().isKinematic = true;
        Weaponobject.GetComponent<BoxCollider>().isTrigger = true;
		Weaponobject.SetActive(false);
        Weaponobject.transform.parent = playergameObject.transform;
		 Weaponobject.transform.position = playergameObject.transform.position;
        Weaponobject.transform.rotation = playergameObject.transform.rotation;
		EquippedWeapon = Weaponobject;
		StartCoroutine(_SwitchWeapon(9));
        
    }
    public override void Inputs()
    {
        //Input abstraction for easier asset updates using outside control schemes
        inputJump = Input.GetButtonDown("Jump");
        inputLightHit = Input.GetButtonDown("LightHit");
        inputDeath = Input.GetButtonDown("Death");
        inputUnarmed = Input.GetButtonDown("Unarmed");
        inputShield = Input.GetButtonDown("Shield");
        inputAttackL = Input.GetButtonDown("AttackL");
        inputAttackR = Input.GetButtonDown("AttackR");
        inputCastL = Input.GetButtonDown("CastL");
        inputCastR = Input.GetButtonDown("CastR");
        inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
        inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
        inputLshift = Input.GetKey(KeyCode.LeftShift);
        inputTargetBlock = Input.GetAxisRaw("TargetBlock");
        inputDashVertical = Input.GetAxisRaw("DashVertical");
        inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputAiming = Input.GetButtonDown("Aiming");
		InputScroll = Input.GetAxisRaw("Mouse Scroll");
		/*
		inputDiUp = Input.GetButtonDown("DirectUp");
		inputDiDown = Input.GetButtonDown("DirectDown");
		inputDiLeft= Input.GetButtonDown("Directleft");
		inputDiRight = Input.GetButtonDown("DirectRight");
		inputFire = Input.GetButtonDown("Fire");
		*/
		TarPos = Input.mousePosition;
		_Modechange();
	}
	public override void Rolling()
	{
		if(!isRolling && isGrounded && !isAiming && Input.GetKey(KeyCode.LeftControl)){
			if(inputDashVertical > 0.5f || inputDashVertical < -0.5f || inputDashHorizontal > 0.5f || inputDashHorizontal < -0.5f){
				StartCoroutine(_DirectionalRoll());
			}
		}
	}

	void ChangeTargetPos()
	{
		if (inputFire)
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				navMeshAgent.destination = hit.point;
			}
		}
	}

	public override void  CameraRelativeInput(){
		//Camera relative movement
		Transform cameraTransform = sceneCamera.transform;
		//Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		//directional inputs
		dv = inputDashVertical;
		dh = inputDashHorizontal;
		if(!isRolling && !isAiming){
			targetDashDirection = dh * right + dv * -forward;
		}
		x = inputHorizontal;
		z = inputVertical;
		inputVec = x * right + z * forward;
	}
	public override void Aiming()
	{
		for (int i = 0; i < Input.GetJoystickNames().Length; i++)
		{
			//if the right joystick is moved, use that for facing
			if (Mathf.Abs(inputDashHorizontal) > 0.1 || Mathf.Abs(inputDashVertical) < -0.1)
			{
				Vector3 joyDirection = new Vector3(inputDashHorizontal, 0, -inputDashVertical);
				joyDirection = joyDirection.normalized;
				Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
				transform.rotation = joyRotation;
			}
		}
		//no joysticks, use mouse aim
		if (Input.GetJoystickNames().Length == 0)
		{
			Plane characterPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePosition = new Vector3(0, 0, 0);
			float hitdist = 0.0f;
			if (characterPlane.Raycast(ray, out hitdist))
			{
				mousePosition = ray.GetPoint(hitdist);
			}
			mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
			Vector3 relativePos = transform.position - mousePosition;
			Quaternion rotation = Quaternion.LookRotation(-relativePos);
			transform.rotation = rotation;

		}
	}
}
