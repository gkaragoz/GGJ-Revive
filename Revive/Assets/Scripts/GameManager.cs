using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> allFlowers = new List<GameObject>();
    public List<GameObject> allGraves = new List<GameObject>();

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;

        allFlowers = GameObject.FindGameObjectsWithTag("Flower").ToList();
        allGraves = GameObject.FindGameObjectsWithTag("Grave").ToList();
    }
}
