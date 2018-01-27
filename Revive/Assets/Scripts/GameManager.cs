using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<FlowerManager> allFlowers = new List<FlowerManager>();
    public List<GameObject> allGraves = new List<GameObject>();

    public List<SkeletonAI> allSkeletons = new List<SkeletonAI>();

    public PlayerController player;
    public OpponentAI opponent;

    public SkeletonAI skeletonAIPrefab;

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;

        GameObject [] allFlowersObjs = GameObject.FindGameObjectsWithTag("Flower");
        foreach (var flower in allFlowersObjs) {
            allFlowers.Add(flower.GetComponent<FlowerManager>());
        }

        allGraves = GameObject.FindGameObjectsWithTag("Grave").ToList();

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        opponent = GameObject.Find("Opponent").GetComponent<OpponentAI>();
    }

    public void InstantiateSkeleton(Transform grave, SkeletonAI.Team team) {
        SkeletonAI skeleton = Instantiate(skeletonAIPrefab, grave.position, Quaternion.identity).GetComponent<SkeletonAI>();
        skeleton.SetTeam(team);
        allSkeletons.Add(skeleton);
    }
}
