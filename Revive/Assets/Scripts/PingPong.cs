using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour {

    public float time = 0.75f;

    void Awake() {
		LeanTween.scale(this.gameObject, transform.localScale * 1.25f, time).setEase(LeanTweenType.pingPong).setLoopType(LeanTweenType.pingPong);
    }

}
