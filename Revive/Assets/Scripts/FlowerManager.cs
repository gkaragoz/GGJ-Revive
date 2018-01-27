using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public float rebornTime = 5f; //Seconds
    public float remainingRebornTime = 0f;
    public bool isDeath = false;
    public bool isTurning = false;
    public bool isLive = true;
    public Transform deathFlowerPrefab;
    public Transform liveFlowerPrefab;
	public ParticleSystem dieEffect;
	public float effectDistance;

    private MeshRenderer flower;
	private GameObject soul;

    void Awake() {
        flower = GetComponent<MeshRenderer>();
		dieEffect = transform.Find("Flower Die Particle Effect").GetComponent<ParticleSystem>();
		dieEffect.Stop ();

        remainingRebornTime = rebornTime;
    }

    void Update() {
        if (isDeath == true) {
            remainingRebornTime -= Time.deltaTime;

            if (remainingRebornTime <= 0) {
                remainingRebornTime = rebornTime;
                Grow();
            }
        }
    }

	public void OnInteracted(Transform interactor) {
        if (isTurning == false) {
            if (isLive == true) {
				PlayDeathParticleAnimation (interactor);
                Die();
            }
        }
    }

    void Die() {
        isTurning = true;
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnBecomeDarkComplete);
    }

	void PlayDeathParticleAnimation(Transform interactor){
		dieEffect.gameObject.transform.LookAt (interactor);
		ParticleSystem.MainModule main = dieEffect.main;
		float distance = Vector3.Distance (interactor.position, transform.position);
		main.duration = effectDistance * distance * distance * distance * distance;
		//dieEffect.main = main;
		dieEffect.Play ();
	}

    void OnBecomeDarkComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinishTurningToDeath);
        ActiveDeathFlower();
    }

    void OnFinishTurningToDeath() {
        isTurning = false;
        isDeath = true;
    }

    void Grow() {
        isTurning = true;
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnGrowGreenComplete);
    }

    void OnGrowGreenComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad().setOnComplete(OnFinishTurningToLive);
        ActiveLiveFlower();
    }

    void OnFinishTurningToLive() {
        isTurning = false;
        isDeath = false;
    }

    void ActiveDeathFlower() {
        deathFlowerPrefab.gameObject.SetActive(true);
        liveFlowerPrefab.gameObject.SetActive(false);
        isLive = false;
    }

    void ActiveLiveFlower() {
        deathFlowerPrefab.gameObject.SetActive(false);
        liveFlowerPrefab.gameObject.SetActive(true);
        isLive = true;
    }

}
