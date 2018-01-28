using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public bool isInteracting = false;
    public bool isDeath = false;

    [Header("Settings")]
    public bool overrideAgentValues = true;

    [Header("Combat Values")]
    public float maxHealth = 10f;
    public float currentHealth = 0f;
    public float upgradeAmount = 0f;

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

    private Animator anim;
    private NavMeshAgent agent;
    private List<FlowerManager> interactableFlowers = new List<FlowerManager>();
    private List<GraveManager> interactableGraves = new List<GraveManager>();

    public float Health
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Start () {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        GetReferences();
    }

    void Update() {
        if (isDeath)
            return;

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
                StartCoroutine(OnMouseRightClick(target));
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
        if (agent != null)
            agent.isStopped = false;
    }

    void StopAgent() {
        if (agent != null)
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

        anim.SetTrigger("GetFlowerSprit");

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

        anim.SetTrigger("CallSkeleton");

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

    public IEnumerator HitDamage(float amount)
    {
        Health -= amount;
        //Take hit animation.
        yield return new WaitForSeconds(0f);
    }

    void OnMouseLeftClick() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            SetTarget(hit.point);
        }
    }

    IEnumerator OnMouseRightClick(Transform target) {
        anim.SetTrigger("HealIt");

        yield return new WaitForSeconds(AnimationDatas.instance.GetAnimationLength(AnimationDatas.AnimationStates.HealIt));

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            if (hit.transform.gameObject.tag == "Skeleton") {
                if (hasHealOnHands == true) {
                    if (hit.transform.gameObject.GetComponent<SkeletonAI>().isDeath == false) {
                        if (hit.transform.gameObject.GetComponent<SkeletonAI>().team == SkeletonAI.Team.Player) {
                            hasHealOnHands = false;
                            GameObject fx = Instantiate(healFXObj.gameObject, transform.position, Quaternion.identity);
                            fx.transform.parent = GameObject.Find("FX_TRASH").transform;
                            fx.GetComponent<Projectile>().SetTarget(hit.transform, SkeletonAI.Team.Player);
                        } 
                    }
                }
            }
        }
    }

    void Die() {
        GameManager.instance.isGameFinished = true;
        isDeath = true;
        isInteracting = false;
        maxHealth = 0;
        anim.SetTrigger("Die");
        StopAgent();

        if (agent != null)
            Destroy(agent);
    }
}
