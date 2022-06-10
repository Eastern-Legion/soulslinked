using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	PlayerController PlayerController;
	GameObject cameraTarget;
	public float rotateSpeed;
	float rotate;
	Vector3 CamAxis;
	public float offsetDistance;
	public float offsetHeight;
	public float smoothing;
	Vector3 offset;
	bool following = true;
	Vector3 lastPosition;

	void Start()
	{
		cameraTarget = GameObject.FindGameObjectWithTag("Player");
		PlayerController = cameraTarget.GetComponent<PlayerController>();
		lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
		offset = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			if(following)
			{
				following = false;
			} 
			else
			{
				following = true;
			}
		} 
		if(Input.GetKey(KeyCode.Q))
		{
			rotate = -1;
		} 
		else if(Input.GetKey(KeyCode.V))
		{
			rotate = 1;
		}
		else
		{
			rotate = 0;
		}

		if(Input.GetKey(KeyCode.Y))
		{
			CamAxis =  new Vector3(1,0,0);
		} 
		else
		{
			CamAxis = new Vector3(0,1,0);
		}

		if(PlayerController.inputLshift)
		{
			if (PlayerController.InputScroll == -1)
			{
				offsetHeight = offsetHeight-1;
			} 
			else if(PlayerController.InputScroll == 1)
			{
				offsetHeight = offsetHeight+1;
			}
		}
		
		if(following)
		{
			offset = Quaternion.AngleAxis(rotate * rotateSpeed, CamAxis) * offset;
			transform.position = cameraTarget.transform.position + offset; 
			transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime), 
				Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime), 
				Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offsetHeight, smoothing * Time.deltaTime));
		} 
		else
		{
			transform.position = lastPosition; 
		}
		transform.LookAt(cameraTarget.transform.position);
	}

	void LateUpdate()
	{
		lastPosition = transform.position;
	}


	//control camera with mouse to edge of screen
	/*
	float speed = 10.0f;
	int boundary = 1;
	int width;
	int height;
 
	void Start () 
	{
	width = Screen.width;
	height = Screen.height;
	
	}
	
	void Update () 
	{
		if (Input.mousePosition.x > width - boundary)
		{
			transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed, 0.0f, 0.0f);
		}
		
		if (Input.mousePosition.x < 0 + boundary)
		{
			transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed, 0.0f, 0.0f);
		}
		
		if (Input.mousePosition.y > height - boundary)
		{
			transform.position -= new Vector3 (0.0f, 0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed);		
		}
 
		if (Input.mousePosition.y < 0 + boundary)
		{
			transform.position -= new Vector3 (0.0f, 0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed);		
		}
 
	}
	*/
}