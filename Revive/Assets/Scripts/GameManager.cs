using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<FlowerManager> allFlowers = new List<FlowerManager>();
    public List<GameObject> allGraves = new List<GameObject>();

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;

        GameObject [] allFlowersObjs = GameObject.FindGameObjectsWithTag("Flower");
        foreach (var flower in allFlowersObjs) {
            allFlowers.Add(flower.GetComponent<FlowerManager>());
        }

        allGraves = GameObject.FindGameObjectsWithTag("Grave").ToList();
    }
}
