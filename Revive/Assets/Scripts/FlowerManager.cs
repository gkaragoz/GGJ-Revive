using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public float rebornTime = 5f; //Seconds
    public float remainingRebornTime = 0f;
    public bool isDeath = false;
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

        remainingRebornTime = rebornTime;
    }

    void Update() {
        if (isDeath == true) {
            remainingRebornTime -= Time.deltaTime;

            if (remainingRebornTime <= 0) {
                remainingRebornTime = rebornTime;
                GrowGreen();
            }
        }
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
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinishDarkTurning);
        flower.material = darkFlower;
        isGreen = false;
    }

    void OnFinishDarkTurning() {
        isTurning = false;
        isDeath = true;
    }

    void GrowGreen() {
        isTurning = true;
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnGrowGreenComplete);
    }

    void OnGrowGreenComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinishGreenTurning);
        flower.material = greenFlower;
        isGreen = true;
        isDeath = false;
    }

    void OnFinishGreenTurning()
    {
        isTurning = false;
        isDeath = true;
    }

}
