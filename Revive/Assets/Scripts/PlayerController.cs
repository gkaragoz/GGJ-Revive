using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public Transform target;

    private NavMeshAgent agent;

    void Start () {
        GetReferences();
    }

    void FixedUpdate() {
        if (target != null) {
            agent.destination = target.position;
        }
    }

    void GetReferences() {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Target").transform;
    }
}
