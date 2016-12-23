using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (Character2D))]

public class PlayerControl2D : MonoBehaviour {

	private Character2D m_Character;
	private bool m_Jump;
	private int m_Attack;


	private void Awake()
	{
		m_Character = GetComponent<Character2D>();
	}


	private void Update()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
		if (m_Attack==0) {
			if (CrossPlatformInputManager.GetButtonDown ("Fire1"))
				m_Attack = 1;
			else if (m_Attack == 0) {
				if (CrossPlatformInputManager.GetButtonDown ("Fire2"))
					m_Attack = 2;
			}
		}

	}


	private void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		//print(m_Attack);
		m_Character.Move(h, crouch, m_Jump,m_Attack);
		m_Jump = false;
		m_Attack = 0;
	}
}
