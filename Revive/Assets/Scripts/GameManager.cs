﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<FlowerManager> allFlowers = new List<FlowerManager>();
    public List<GraveManager> allGraves = new List<GraveManager>();

    public List<SkeletonAI> allSkeletons = new List<SkeletonAI>();

    public PlayerController player;
    public OpponentAI opponent;

    private bool isGameFinished = false;
    public bool isGameStarted = false;

    public bool GameFinished
    {
        get { return isGameFinished; }
        set
        {
            isGameFinished = value;
            if (isGameFinished == true)
            {
                UIManager.instance.OpenGameOver();
            }
        }
    }

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;

        GameObject[] allFlowersObjs = GameObject.FindGameObjectsWithTag("Flower");
        foreach (var flower in allFlowersObjs) {
            allFlowers.Add(flower.GetComponent<FlowerManager>());
        }

        GameObject[] allGravesObjs = GameObject.FindGameObjectsWithTag("Grave");
        foreach (var grave in allGravesObjs) {
            allGraves.Add(grave.GetComponent<GraveManager>());
        }

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        opponent = GameObject.Find("Opponent").GetComponent<OpponentAI>();
    }

    public void SetCameraToDeath() {
        if (player.isDeath)
            Camera.main.GetComponent<RTS_Cam.RTS_Camera>().SetTarget(player.transform);
        else if (opponent.isDeath)
            Camera.main.GetComponent<RTS_Cam.RTS_Camera>().SetTarget(opponent.transform);

    }
}
