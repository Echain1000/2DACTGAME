using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (Character2D))]

public class EnemyControl2D : MonoBehaviour {

	private Character2D m_Character;
	private bool m_Jump;
	private int m_Attack;

	private void Awake()
	{
		m_Character = GetComponent<Character2D>();
	}

	private void FixedUpdate()
	{
		// Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis("player2Control");

		bool crouch=false;

		// Pass all parameters to the character control script.
		m_Character.Move(h, crouch, m_Jump,m_Attack);
		m_Jump = false;
		m_Attack = 0;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("player2Jump");
		}
		if (m_Attack==0) {
			if (CrossPlatformInputManager.GetButtonDown ("player2Fire"))
				m_Attack = 1;
			else if (m_Attack == 0) {
				if (CrossPlatformInputManager.GetButtonDown ("player2Fire2"))
					m_Attack = 2;
			}
		}
	}
}
