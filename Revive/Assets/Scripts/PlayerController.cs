using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    [Header("Settings")]
    public bool overrideAgentValues = true;

    [Header("Values")]
    public float interactRange = 2f;
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;

    private NavMeshAgent agent;

    void Start () {
        GetReferences();
    }

    void FixedUpdate() {
        if (overrideAgentValues) {
            SetAgent();
        }

        if (target != null) {
            agent.destination = target.position;
        }
    }

    void GetReferences() {
        agent = GetComponentInChildren<NavMeshAgent>();
        target = GameObject.Find("Target").transform;
    }

    void SetAgent() {
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = autoBraking;
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
