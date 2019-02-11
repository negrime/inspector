using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit : MonoBehaviour {

	// Use this for initialization
	public float speed;
	public enum Dir : int
	{
		mY = -1,
		pY = 1	
	}

	public Dir dir;

	private void Start()
	{
		StartCoroutine(Pause());
	}

	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new  Vector2(0, (float)dir)*  speed * Time.deltaTime, Space.World);
	}

	IEnumerator Pause()
	{
		yield return new WaitForSeconds(1);
		Destroy(this.gameObject);
	}
}
