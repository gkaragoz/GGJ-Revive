﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public bool isInteracting = false;

    [Header("Settings")]
    public bool overrideAgentValues = true;

    [Header("Values")]
    public float graveInteractionTime = 1f;
    public float graveInteractRange = 0.5f;
    public float flowerInteractionTime = 2f;
    public float flowerInteractRange = 2f;
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;

    private NavMeshAgent agent;
    private List<FlowerManager> interactableFlowers = new List<FlowerManager>();
    private List<GameObject> interactableGraves = new List<GameObject>();

    void Start () {
        GetReferences();
    }

    void Update() {
        if (overrideAgentValues) {
            SetAgent();
        }

        if (Input.GetKeyDown(KeyCode.E) && isInteracting == false) {
            FindInteractableFlowers();

            if (interactableFlowers.Count > 0) {
                Debug.Log("I'm gonna interact with flowers: " + interactableFlowers.Count);
                StartCoroutine(StartInteractWithFlowers());
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && isInteracting == false)
        {
            FindInteractableGraves();

            if (interactableGraves.Count > 0)
            {
                Debug.Log("I'm gonna interact with graves: " + interactableGraves.Count);
                StartCoroutine(StartInteractWithGraves());
            }
        }

        if (target != null) {
            if (Input.GetMouseButtonDown(0)) {
                OnMouseClick();
            }
        }
    }

    void GetReferences() {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Target").transform;
    }

    void SetAgent() {
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = autoBraking;
    }

    void ReleaseAgent() {
        agent.isStopped = false;
    }

    void StopAgent() {
        agent.isStopped = true;
        SetTarget(transform.position);
    }

    void SetTarget(Vector3 position) {
        if (agent != null && target != null) {
            target.position = position;
            agent.destination = target.position;
        }
    }

    void FindInteractableFlowers() {
        float distance = 0f;

        interactableFlowers = new List<FlowerManager>();

        foreach (var flower in GameManager.instance.allFlowers) {
            distance = Vector3.Distance(transform.position, flower.transform.position);

            if (distance <= flowerInteractRange) {
                interactableFlowers.Add(flower);
            }
        }
    }

    IEnumerator StartInteractWithFlowers() {
        isInteracting = true;
        StopAgent();

        //anim.Start(flowerInteractAnimation);

        foreach (var flower in interactableFlowers) {
            flower.OnInteracted();
        }

        yield return new WaitForSeconds(flowerInteractionTime);
        Debug.Log("I completed my flower interaction.");
        isInteracting = false;
        ReleaseAgent();
    }

    void FindInteractableGraves() {
        float distance = 0f;

        interactableGraves = new List<GameObject>();

        foreach (var grave in GameManager.instance.allGraves)
        {
            distance = Vector3.Distance(transform.position, grave.transform.position);

            if (distance <= graveInteractRange)
            {
                interactableGraves.Add(grave);
            }
        }
    }

    IEnumerator StartInteractWithGraves()
    {
        isInteracting = true;
        StopAgent();

        //anim.Start(graveInteractAnimation);

        foreach (var graves in interactableGraves)
        {
            Debug.Log("Spawn skeleton from: " + graves.name);
            //FX.Play(skeletonSpawn);
        }

        yield return new WaitForSeconds(graveInteractionTime);
        Debug.Log("I completed my grave interaction.");
        isInteracting = false;
        ReleaseAgent();
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, flowerInteractRange);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, graveInteractRange);
    }

    void OnMouseClick() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            SetTarget(hit.point);
        }
    }
}
