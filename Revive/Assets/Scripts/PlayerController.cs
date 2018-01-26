using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public bool isInteracting = false;

    [Header("Settings")]
    public bool overrideAgentValues = true;

    [Header("Values")]
    public float interactionTime = 2f;
    public float interactRange = 2f;
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;

    private NavMeshAgent agent;
    private List<GameObject> interactableFlowers = new List<GameObject>();

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
                StartCoroutine(StartInteractToFlowers());
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

        interactableFlowers = new List<GameObject>();

        foreach (var flower in GameManager.instance.allFlowers) {
            distance = Vector3.Distance(transform.position, flower.transform.position);

            if (distance <= interactRange) {
                interactableFlowers.Add(flower);
            }
        }
    }

    IEnumerator StartInteractToFlowers() {
        isInteracting = true;
        StopAgent();

        //anim.Start(flowerInteractAnimation);

        foreach (var flower in interactableFlowers) {
            Debug.Log("Go Die: " + flower.name);
            //flower.Die();
        }

        yield return new WaitForSeconds(interactionTime);
        Debug.Log("I completed my interaction.");
        isInteracting = false;
        ReleaseAgent();
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    void OnMouseClick() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            SetTarget(hit.point);
        }
    }
}
