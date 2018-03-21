using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class PlayerMove : MonoBehaviour 
	{
		public float moveSpeed =30f;
		public float turnSpeed =250f;

		public Camera headCamera;
		public Camera thirdPersonCamera;

		private AudioSource playerAudio;
		private Rigidbody playerRigidbody;
		private float movementInputValue;
		private float turnInputValue;

		private float rotLeftRight;
		private float upDownRange=40f;
		private float verticalRotation=0f;

		[HideInInspector]
		public bool isMovable = true;
		[HideInInspector]
		public bool isInEscalator=false;
		[HideInInspector]
		public string playerMode;

		void Awake()
		{
			playerAudio = GetComponent<AudioSource>();
			playerRigidbody = GetComponent<Rigidbody>();
		}

		void Start ()
		{
			//Debug.Log ("player mode is "+playerMode);
//			transform.position.x = SharingData.currentTransform.x;
			CameraSwitch ();
		}

		void CameraSwitch()
		{
			headCamera.enabled = true;
			thirdPersonCamera.enabled = false;
		}

		void Update () 
		{
			ExplorationMode ();
		}

		void ExplorationMode()
		{
			if (isMovable)
			{
				movementInputValue = Input.GetAxis ("Vertical");
				turnInputValue = Input.GetAxis ("Horizontal");
				//rotLeftRight = Input.GetAxis ("Mouse X");
				StepAudio ();
				OnEnable ();
			}
			else
			{
				rotLeftRight = Input.GetAxis ("Mouse X");
				transform.Rotate(0, rotLeftRight, 0);
				OnDisable ();
			}
			LookUpAndDown ();
		}

		void FixedUpdate()
		{
			if (isMovable) 
			{
				Move ();
				Turn ();
			}
		}

		void Move()
		{
			Vector3 movement = transform.forward * movementInputValue * moveSpeed * Time.deltaTime;
			playerRigidbody.MovePosition (playerRigidbody.position + movement);
		}

		void Turn()
		{
			float turn = turnInputValue * turnSpeed * Time.deltaTime;
			Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
			playerRigidbody.MoveRotation (playerRigidbody.rotation * turnRotation);
		}

		void LookUpAndDown()
		{
			verticalRotation -= Input.GetAxis ("Mouse Y");
			verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
			headCamera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		}

		void OnEnable()
		{
			playerRigidbody.isKinematic = false;
		}

		void OnDisable()
		{
			playerRigidbody.isKinematic = true;
		}	

		void StepAudio()
		{
			if (Mathf.Abs (movementInputValue) > 0.1f || Mathf.Abs (turnInputValue) > 0.1f)
			{
				if (!playerAudio.isPlaying)
					playerAudio.Play ();
			} 
		}
	}
}

