using UnityEngine;
using System.Collections;

public class attackDetection : MonoBehaviour
{

	private Transform judge1;
	private Transform judge2;
	private Transform edge;
	private GameObject characterSelf;
	private Character2D character_;
	private Vector3 point1;
	private Vector3 point2;
	private ArrayList hitList=new ArrayList();

	private float[] skill01Info;
	private float[] skill02Info;

	public LayerMask m_hitTarget;
	public ParticleSystem particle;
	public ParticleSystem particle2;

	private void Awake ()
	{
		judge1 = transform.Find ("judge1");
		judge2 = transform.Find ("judge2");
		edge = transform.Find ("edge");
		character_ = GetComponentInParent<Character2D> ();
		characterSelf = gameObject.transform.parent.gameObject;
		skill01Info= new float[6] {0.36f,0.672f,0.0f,20.0f,20.0f,0.0f}; 
		//attack begin frame, end frame, hit weapon damage to health，hit weapon damage to resist,hit player damage to health,hit player damage to resist
		skill02Info= new float[6] {0.21f,0.57f,0.0f,40.0f,40.0f,0.0f};
	}

	private void hitMessage (Character2D obj, int type,float[] effect)
	{//type as hit type, 1 for weapon, 2 for player
		if (type == 1) {
			if (!obj.hasBeenHitThisTime) {
				if (obj.attactState == 2) {
					obj.hasBeenHitThisTime = true;
					hitList.Add (obj);
					obj.SendMessage ("OnHit", effect);
					particle.GetComponent<Transform>().position= edge.position;
					particle.Play ();
				}
			}
		} else if (type == 2) {
			if (!obj.hasBeenHitThisTime) {
				obj.SendMessage ("OnHit", effect);
				obj.hasBeenHitThisTime = true;
				hitList.Add (obj);
				particle2.GetComponent<Transform>().position= edge.position;
				particle2.Play ();
			}
		}
	}

	private void hitRecover ()
	{
		if (hitList.Count == 0)
			return;
		else {
			for (int i = 0; i < hitList.Count; i++) {
				Character2D obj = hitList [i] as Character2D; //question is this obj automatically a pointer?
				obj.hasBeenHitThisTime = false;
				hitList.RemoveAt (i);
			}
		}
	}

	private float[] calculateDamage(int type)
	{//type as hit type, 1 for weapon, 2 for player
		float skillFrame = characterSelf.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime;
		int skillIndex_ = character_.Skill_Index;
		float[] result= new float[2] {0.0f,0.0f};
		switch (skillIndex_)
		{
		case 1:
			{
				float frameRatio = Mathf.InverseLerp (skill01Info [0], skill01Info [1], skillFrame);
				float effectRatio = Mathf.Lerp (0.5f, 1.0f, frameRatio);

				if (type == 1) {
					result [0] = skill01Info [2]*effectRatio;
					result [1] = skill01Info [3]*effectRatio;
				} else if (type == 2) {
					result [0] = skill01Info [4]*effectRatio;
					result [1] = skill01Info [5]*effectRatio;
				}
				break;
			}
		case 2:
			{
				float frameRatio = Mathf.InverseLerp (skill02Info [0], skill02Info [1], skillFrame);

				float effectRatio = Mathf.Lerp (0.5f, 1.0f, frameRatio);
				if (type == 1) {
					result [0] = skill02Info [2]*effectRatio;
					result [1] = skill02Info [3]*effectRatio;
				} else if (type == 2) {
					result [0] = skill02Info [4]*effectRatio;
					result [1] = skill02Info [5]*effectRatio;
				}
				break;
			}
		default:
			break;
		}
		return result;
	}

	private void FixedUpdate ()
	{
		if (character_.attactState == 1) {
			hitRecover ();
			return;
		}
		point1 = judge1.position;
		point2 = judge2.position;
		Collider2D[] colliders = Physics2D.OverlapAreaAll (point1, point2, m_hitTarget);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject != gameObject && colliders [i].gameObject != characterSelf) {
				switch (colliders [i].gameObject.tag) {
				case "weapon":
					{
						hitMessage (colliders [i].gameObject.GetComponentInParent<Character2D> (), 1,calculateDamage(1));
						break;
					}
				case"Player":
					{
						hitMessage (colliders [i].gameObject.GetComponentInParent<Character2D> (), 2,calculateDamage(2));
						break;
					}
				default:
					break;
				}
			}
		}
		
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
