using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 0.01f;

	Vector3 offsetPos;
	Quaternion rotation;

	public enum Position
	{
		FirstPerson,
		ThirdPerson,
		Strategic
	}

	// Use this for initialization
	void Start () {
		Screen.SetResolution (480, 320, true);
		Switch (Position.ThirdPerson);
	}
	
	void FixedUpdate () {
		Vector3 targetCamPos = offsetPos;
		transform.localPosition = Vector3.Lerp (transform.localPosition, targetCamPos,
			smoothing * Time.deltaTime);

		transform.localRotation = Quaternion.Slerp (transform.localRotation, rotation,
			smoothing * Time.deltaTime);
	}

	public void Switch(Position pos)
	{
		switch (pos)
		{
			case Position.FirstPerson:
				offsetPos = new Vector3 (0, 1.5f, 0);
				rotation = Quaternion.Euler (10f, 0, 0);
				break;

			case Position.ThirdPerson:
				offsetPos = new Vector3 (0f, 3f, -2f);
				rotation = Quaternion.Euler (30f, 0, 0);
				break;

			default:
				break;
		}
	}
}
