using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public bool isTurning = false;
    public bool isGreen = true;
    public Material darkFlower;
    public Material greenFlower;

    private MeshRenderer flower;

    void Awake() {
        flower = GetComponent<MeshRenderer>();
    }

    public void OnInteracted() {
        if (isTurning == false) {
            if (isGreen == true) {
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
