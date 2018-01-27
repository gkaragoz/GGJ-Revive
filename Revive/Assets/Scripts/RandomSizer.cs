using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomSizer : MonoBehaviour {

    public Vector3 size = Vector3.one;

    public float maxSize = 1f;
    public float minSize = 0.1f;

    void Awake() {
        size = Vector3.one * Random.Range(minSize, maxSize);
        transform.localScale = size;
    }
}
