using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

    public bool isGreen = true;
    public Material darkFlower;
    public Material greenFlower;

    private MeshRenderer flower;

    void Awake() {
        flower = GetComponent<MeshRenderer>();
    }

    public void OnInteracted() {
        if (isGreen == true) {
            BecomeDark();
        } else {
            GrowGreen();
        }
    }

    void BecomeDark() {
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnBecomeDarkComplete);
    }

    void OnBecomeDarkComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad();
        flower.material = darkFlower;
        isGreen = false;
    }

    void GrowGreen() {
        LeanTween.scale(this.gameObject, Vector3.zero, 2f).setEaseOutQuad().setOnComplete(OnGrowGreenComplete);
    }

    void OnGrowGreenComplete() {
        LeanTween.scale(this.gameObject, Vector3.one, 2f).setEaseOutQuad();
        flower.material = greenFlower;
        isGreen = true;
    }

}
