using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed;
	public float turningSpeed;

	Vector3 movement;
	Animator anim;
	Rigidbody rb;
	AudioSource[] audios;
	float footstepInterval = 0.5f;
	float footstepTimer = 0f;
	CameraFollow cameraFollow;
	InputControls input;


	void Awake()
	{
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		audios = transform.Find ("Footsteps").GetComponents<AudioSource> ();
		cameraFollow = GetComponentInChildren<CameraFollow> ();
		input = new InputControls ();
	}

	class InputControls
	{
		public float Vertical { get; set; }
		public float Horizontal { get; set; }
		public bool Running { get; set; }

		static readonly float LongTap = .15f;
		float longTapTimer = 0;
		bool isAndroid;


		public InputControls()
		{
			isAndroid = Application.platform == RuntimePlatform.Android;
		}

		public void Detect()
		{
			Vertical = Horizontal = 0f;
			Running = false;

			if (isAndroid)
			{
				if (Input.touchCount == 0) return;
				Touch touch = Input.GetTouch (0);

				if (touch.phase == TouchPhase.Moved)
				{
					Horizontal = sign (touch.deltaPosition.x);
				}
				if (touch.phase == TouchPhase.Stationary)
				{
					longTapTimer += Time.deltaTime;
					Debug.Log ("Stationary: " + longTapTimer);
					if (longTapTimer > LongTap)
					{
						Running = Screen.height / 2 < touch.position.y;
						Vertical = 1;
					}
				} else longTapTimer = 0;
					
			} else
			{
				Running = Input.GetKey (KeyCode.LeftShift);
				Vertical = Input.GetAxisRaw ("Vertical");
				Horizontal = Input.GetAxisRaw ("Horizontal");
			}
		}

		private int sign(float f)
		{
			return f == 0 ? 0 : (f > 0f ? 1 : -1);
		}
	}


	void FixedUpdate ()
	{
		// move
		float v = input.Vertical * (input.Running ? 2 : 1);
		rb.MovePosition (rb.position +  v * transform.forward * movementSpeed * Time.deltaTime);

		// rotate
		rb.MoveRotation (rb.rotation * Quaternion.Euler (0, input.Horizontal * turningSpeed, 0));

		// anim
		bool isIdle = v == 0f;
		if (isIdle)
		{
			anim.SetBool ("Stop", true);
		} else
		{
			if (input.Running)
			{
				anim.SetBool ("Run", true);
				footstepInterval = .25f;
			} else
			{
				anim.SetBool ("Walk", !isIdle);
				footstepInterval = .5f;
			}
		}

		// audio
		footstepTimer += Time.deltaTime;
		if (!isIdle && footstepTimer > footstepInterval)
		{
			footstepTimer = 0f;
			audios [Random.Range (0, audios.Length)].Play ();
		}
	}

	void Update()
	{
		input.Detect ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Inside"))
		{
			cameraFollow.Switch (CameraFollow.Position.FirstPerson);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Inside"))
		{
			cameraFollow.Switch (CameraFollow.Position.ThirdPerson);
		}
	}
}
