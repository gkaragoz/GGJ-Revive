using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour {
	public GameObject goodVersion;
	public GameObject evilVersion;

	public float goodDieSpeed;
	public float goodGrowSpeed;
	public float evilDieSpeed;
	public float evilGrowSpeed;

	private Vector3 initialGoodScale;
	private Vector3 initialEvilScale;

	void Start(){
		goodVersion = Instantiate (goodVersion);
		evilVersion = Instantiate (evilVersion);
		initialGoodScale = goodVersion.transform.localScale;
		initialEvilScale = evilVersion.transform.localScale;
		evilVersion.transform.localScale = Vector3.zero;
		evilVersion.GetComponent<Renderer> ().enabled = false;
	}

	void Update(){
		if(Input.anyKeyDown)
			StartCoroutine (killGood());
		
	}

	public IEnumerator killGood(){
		bool dead = false;
		while (!dead) {
			goodVersion.transform.localScale -= new Vector3 (goodDieSpeed, goodDieSpeed, goodDieSpeed);
			yield return new WaitForSeconds (0.01f);
			if (goodVersion.transform.localScale.x <= 0) {
				dead = true;
				evilVersion.GetComponent<Renderer> ().enabled = true;
				StartCoroutine (growEvil());
			}
		}
		goodVersion.GetComponent<Renderer> ().enabled = false;
		goodVersion.transform.localScale = Vector3.zero;
	}

	public IEnumerator growEvil(){
		bool fullyGrown = false;
		while (!fullyGrown) {
			evilVersion.transform.localScale += new Vector3 (evilGrowSpeed, evilGrowSpeed, evilGrowSpeed);
			yield return new WaitForSeconds (0.1f);
			if (evilVersion.transform.localScale.x > initialEvilScale.magnitude) {
				evilVersion.transform.localScale = initialEvilScale;
				fullyGrown = true;
			}
		}
	}
}
