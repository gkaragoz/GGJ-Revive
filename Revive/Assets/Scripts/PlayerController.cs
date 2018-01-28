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
    public float graveInteractionTime = 1f;
    public float graveInteractRange = 0.5f;
    public float flowerInteractionTime = 2f;
    public float flowerInteractRange = 2f;
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    public bool hasHealOnHands = false;
    [HideInInspector]
    public Transform target;
    public Transform healFXObj;

    private NavMeshAgent agent;
    private List<FlowerManager> interactableFlowers = new List<FlowerManager>();
    private List<GraveManager> interactableGraves = new List<GraveManager>();

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
                StartCoroutine(StartInteractWithFlowers());
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && isInteracting == false)
        {
            FindInteractableGraves();

            if (interactableGraves.Count > 0) {
                StartCoroutine(StartInteractWithGraves());
            }
        }

        if (target != null) {
            if (Input.GetMouseButtonDown(0)) {
                OnMouseLeftClick();
            }
            if (Input.GetMouseButtonDown(1)) {
                OnMouseRightClick(target);
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
			flower.OnInteracted(transform);
        }

        yield return new WaitForSeconds(flowerInteractionTime);
        isInteracting = false;
        hasHealOnHands = true;
        ReleaseAgent();
    }

    void FindInteractableGraves() {
        float distance = 0f;

        interactableGraves = new List<GraveManager>();

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

        foreach (var grave in interactableGraves)
        {
            grave.OnInteracted(SkeletonAI.Team.Player);
            //FX.Play(skeletonSpawn);
        }

        yield return new WaitForSeconds(graveInteractionTime);
        isInteracting = false;
        ReleaseAgent();
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, flowerInteractRange);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(transform.position, graveInteractRange);
    }

    void OnMouseLeftClick() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            SetTarget(hit.point);
        }
    }

    void OnMouseRightClick(Transform target) {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            if (hit.transform.gameObject.tag == "Skeleton") {
                if (hasHealOnHands == true) {
                    if (hit.transform.gameObject.GetComponent<SkeletonAI>().isDeath == false) {
                        if (hit.transform.gameObject.GetComponent<SkeletonAI>().team == SkeletonAI.Team.Player) {
                            hasHealOnHands = false;
                            GameObject fx = Instantiate(healFXObj.gameObject, transform.position, Quaternion.identity);
                            fx.GetComponent<Projectile>().SetTarget(hit.transform, SkeletonAI.Team.Player);
                        } 
                    }
                }
            }
        }

    }
}
