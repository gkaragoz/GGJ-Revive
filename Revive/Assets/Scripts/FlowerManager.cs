using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public bool isTurning = false;
    public bool isGreen = true;
    public Material darkFlower;
    public Material greenFlower;
	public ParticleSystem dieEffect;

    private MeshRenderer flower;
	private GameObject soul;

    void Awake() {
        flower = GetComponent<MeshRenderer>();
		dieEffect = transform.Find ("Flower Die Particle Effect").GetComponent<ParticleSystem> ();
		dieEffect.Stop ();
    }

	public void OnInteracted(Transform interactor) {
        if (isTurning == false) {
            if (isGreen == true) {
				PlayDeathParticleAnimation (interactor);
                BecomeDark();
            } else {
                GrowGreen();
            }
        }
    }

    void BecomeDark() {
        isTurning = true;
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnBecomeDarkComplete);
    }

	void PlayDeathParticleAnimation(Transform interactor){
		dieEffect.gameObject.transform.LookAt (interactor);
		dieEffect.Play ();
	}

    void OnBecomeDarkComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinish);
        flower.material = darkFlower;
        isGreen = false;
    }

    void OnFinish() {
        isTurning = false;
    }

    void GrowGreen() {
        isTurning = true;
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnGrowGreenComplete);
    }

    void OnGrowGreenComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinish);
        flower.material = greenFlower;
        isGreen = true;
    }

}
