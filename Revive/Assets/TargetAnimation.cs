using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LeanTween.scale(this.gameObject, transform.localScale * 1.5f, 0.5f).setEase(LeanTweenType.pingPong).setLoopType(LeanTweenType.pingPong);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
