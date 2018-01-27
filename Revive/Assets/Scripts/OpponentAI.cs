﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class OpponentAI : MonoBehaviour {
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

    private List<FlowerManager> interactableFlowers = new List<FlowerManager>();
    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
	
	void Update () {
        if (overrideAgentValues)
            SetAgent();

		if (target == null) {
            //if no interaction
            //if have not any skeleton alive.
            //than find closest grave
            //go to grave
            //interact with grave
            //if have any skeleton alive
            //than find closest grave or flower (OPTIONAL _ CHANCE)
            //interact with object
            //else if have not any skeleton alive
            //than find closest grave
            //go to grave
            //interact with grave

            if (isInteracting == false) {
                if (HasAnyLiveSkeletons() == false) {
                    Transform grave = GetClosestAvailableGrave().transform;
                    SetTarget(grave);
                } else {
                    if (Random.Range(0, 1f) < 0.5f) { //50% percentage of chance.  
                        Transform grave = GetClosestAvailableGrave().transform;
                        SetTarget(grave);
                    } else {
                        Transform flower = GetClosestAvailableFlower().transform;
                        SetTarget(flower);
                    }
                }
            }
        } else {
            if (HasArrivedTarget(target) == true) {
                if (isInteracting == false)
                    Interact();
            }
        }
	}

    void SetAgent() {
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = autoBraking;
    }

    void Interact() {
        Debug.Log("Interaction with: " + target.name);
        if (target == null)
            return;

        isInteracting = true;

        if (target.tag == "Flower") {
            Debug.Log("I'm gonna interact with flower: " + target.name);
            FindInteractableFlowers();
            StartCoroutine(InteractFlower(target));
        } else if (target.tag == "Grave") {
            Debug.Log("I'm gonna interact with grave: " + target.name);
            StartCoroutine(InteractGrave(target));
        }

        target = null;
    }

    IEnumerator InteractGrave(Transform target) {
        isInteracting = true;
        StopAgent();

        //anim.Start(graveInteractAnimation);

        target.GetComponent<GraveManager>().OnInteracted(SkeletonAI.Team.Enemy);
        //FX.Play(skeletonSpawn);

        yield return new WaitForSeconds(graveInteractionTime);
        isInteracting = false;
        ReleaseAgent();
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

    IEnumerator InteractFlower(Transform target) {
        isInteracting = true;
        StopAgent();

        //anim.Start(flowerInteractAnimation);

        foreach (var flower in interactableFlowers) {
            flower.OnInteracted(transform);
        }

        yield return new WaitForSeconds(flowerInteractionTime);
        isInteracting = false;
        ReleaseAgent();
    }

    bool HasArrivedTarget(Transform target) {
        if (target == null)
            return false;

        float searchingDistance = 0f;
        if (target.tag == "Flower") {
            searchingDistance = flowerInteractRange;
        } else if (target.tag == "Grave") {
            searchingDistance = graveInteractRange;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= searchingDistance) {
            return true;
        }

        return false;
    }

    bool HasAnyLiveSkeletons() {
        foreach (var skeleton in GameManager.instance.allSkeletons) {
            if (skeleton.team == SkeletonAI.Team.Enemy) {
                return true;
            }
        }
        return false;
    }

    void SetTarget(Transform target) {
        if (target != null)
        {
            this.target = target;
            ReleaseAgent();
            agent.SetDestination(target.position);
        }
    }

    void ReleaseAgent() {
        agent.isStopped = false;
    }

    void StopAgent() {
        agent.isStopped = true;
    }

    GraveManager GetClosestAvailableGrave() {
        GraveManager bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GraveManager potentialTarget in GameManager.instance.allGraves) {
            if (potentialTarget.isDeath)
                continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    FlowerManager GetClosestAvailableFlower() {
        FlowerManager bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (FlowerManager potentialTarget in GameManager.instance.allFlowers) {
            if (potentialTarget.isDeath)
                continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, flowerInteractRange);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, graveInteractRange);
    }
}
