using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public float interactRange = 2f;
    [HideInInspector]
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

    void OnDrawGizmosSelected() {
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidArc(transform.position,
                transform.up,
                -transform.right,
                360,
                interactRange);
    }
}
