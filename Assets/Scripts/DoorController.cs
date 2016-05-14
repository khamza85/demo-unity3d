using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
	public float openAngle = 120f;
	public float smooth = 5f;

	Transform joint;
	Transform door;
	Quaternion openRotation, closeRotation;
	bool isOpen = false;
	AudioSource[] audios;

	void Awake()
	{
		joint = transform.Find ("Joint");

		closeRotation = joint.localRotation;
		openRotation = Quaternion.Euler (0, closeRotation.eulerAngles.y - openAngle, 0);
		audios = GetComponents<AudioSource> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag ("Player"))
			return;

		isOpen = true;
		audios[0].Play();
	}
		
	void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag ("Player"))
			return;

		isOpen = false;
		audios[1].Play();
	}

	void FixedUpdate()
	{
		Quaternion to = isOpen ? openRotation : closeRotation;
		joint.transform.localRotation = Quaternion.Slerp (
			joint.transform.localRotation, to, smooth * Time.deltaTime);
	}
}
