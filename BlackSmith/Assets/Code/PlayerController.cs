using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
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
        inputStrafe = Input.GetKey(KeyCode.LeftShift);
        inputTargetBlock = Input.GetAxisRaw("TargetBlock");
        inputDashVertical = Input.GetAxisRaw("DashVertical");
        inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputAiming = Input.GetButtonDown("Aiming");
		inputDiUp = Input.GetButtonDown("DirectUp");
		inputDiDown = Input.GetButtonDown("DirectDown");
		inputDiLeft= Input.GetButtonDown("Directleft");
		inputDiRight = Input.GetButtonDown("DirectRight");
		inputFire = Input.GetButtonDown("Fire");
		TarPos = Input.mousePosition;
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
