using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public float rebornTime = 5f; //Seconds
    public float remainingRebornTime = 0f;
    public bool isDeath = false;
    public bool isTurning = false;
    public Transform deathFlowerPrefab;
    public Transform liveFlowerPrefab;
	public ParticleSystem dieEffect;
	public float effectDistance;

    private MeshRenderer flower;
	public Vector3 defaultSizeForLiveFlower;
	public Vector3 defaultSizeForDeathFlower;

    void Awake() {
		defaultSizeForLiveFlower = liveFlowerPrefab.localScale;
		defaultSizeForDeathFlower = deathFlowerPrefab.localScale;

        flower = GetComponent<MeshRenderer>();
		dieEffect.Stop ();

        remainingRebornTime = rebornTime;
    }

    void Update() {
        if (isDeath == true && isTurning == false) {
            remainingRebornTime -= Time.deltaTime;

            if (remainingRebornTime <= 0) {
                remainingRebornTime = rebornTime;
                Grow();
            }
        }
    }

	public void OnInteracted(Transform interactor) {
        if (isTurning == false) {
            if (isDeath == false) {
				PlayDeathParticleAnimation (interactor);
                Die();
            }
        }
    }

    void Die() {
        isTurning = true;
        isDeath = true;
		deathFlowerPrefab.localScale = Vector3.zero;
		LeanTween.scale(liveFlowerPrefab.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnDieComplete);
    }

	void PlayDeathParticleAnimation(Transform interactor){
		dieEffect.gameObject.transform.LookAt (interactor);
		ParticleSystem.MainModule main = dieEffect.main;
		float distance = Vector3.Distance (interactor.position, transform.position);
		main.duration = effectDistance * distance;
		//dieEffect.main = main;
		dieEffect.Play ();
	}

    void OnDieComplete() {
		ActiveDeathFlower();
		LeanTween.scale(deathFlowerPrefab.gameObject, defaultSizeForDeathFlower, 2f).setEaseOutQuad().setOnComplete(OnFinishTurningToDeath);
    }

    void OnFinishTurningToDeath() {
        isTurning = false;
    }

    void Grow() {
        isTurning = true;
        isDeath = false;
		LeanTween.scale(deathFlowerPrefab.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnGrowComplete);
    }

    void OnGrowComplete() {
		LeanTween.scale(liveFlowerPrefab.gameObject, defaultSizeForLiveFlower, 2f).setEaseOutQuad().setOnComplete(OnFinishTurningToLive);
        ActiveLiveFlower();
    }

    void OnFinishTurningToLive() {
        isTurning = false;
        isDeath = false;
    }

    void ActiveDeathFlower() {
        deathFlowerPrefab.gameObject.SetActive(true);
        liveFlowerPrefab.gameObject.SetActive(false);
    }

    void ActiveLiveFlower() {
        deathFlowerPrefab.gameObject.SetActive(false);
        liveFlowerPrefab.gameObject.SetActive(true);
    }

}
