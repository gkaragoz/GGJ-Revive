using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomPrefabSelector : MonoBehaviour {

    public List<GameObject> prefabs = new List<GameObject>();

    void Awake() {
        if (transform.childCount <= 1)
        {
            int randomIndex = Random.Range(0, prefabs.Count);
            GameObject obj = Instantiate(prefabs[randomIndex], Vector3.zero, Quaternion.identity);
            obj.transform.parent = this.transform;
            obj.transform.position = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
