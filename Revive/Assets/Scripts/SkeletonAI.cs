using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SkeletonAI : MonoBehaviour {

    public enum Team {
        Enemy,
        Player
    }

    public bool isBorning = false;
    public bool isAttacking = false;

    [Header("Settings")]
    public bool overrideAgentValues = true;
    public Team team;

    [Header("Values")]
    public float health = 10f;
    public float currentHealth = 0f;
    public float attackSpeed = 2f;
    public float attackRange = 2f;
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;
    public Transform necromancerTarget;
    public Transform healthBarObj;

    public float Health {
        get { return health; }
        set {
            health = value;
            if (health <= 0) {
                health = 0;
                Debug.Log("Die: " + gameObject.name);
                StopAgent();
            }

            SetHealthUIBar();
        }
    }

    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();

        currentHealth = health;

        StartCoroutine(BornAnimation());
    }

    void Update() {
        FixHealthUIBarRotation();

        //search for closest skeleton
        //if there is no skeleton than settarget necromancer
        //if it finds closest skeleton
        //than go to that skeleton

        //if that skeleton reached enemy
        //than attack it

        SkeletonAI skeleton = GetClosestSkeleton();
        if (skeleton == null) {
            SetTarget(necromancerTarget);
        } else {
            SetTarget(skeleton.transform);
        }

        if (isBorning == false) {
            agent.SetDestination(target.position);
        }
    }

    public void SetTeam(Team team) {
        this.team = team;

        SetNecromancerTarget();
    }

    void ReleaseAgent() {
        agent.isStopped = false;
    }

    void StopAgent() {
        agent.isStopped = true;
    }

    IEnumerator BornAnimation() {
        isBorning = true;
        Debug.Log("I'm borning: " + this.name);
        yield return new WaitForSeconds(2f);
        Debug.Log("I borned: " + this.name);
        isBorning = false;
    }

    SkeletonAI GetClosestSkeleton() {
        List<SkeletonAI> skeletons = GameManager.instance.allSkeletons.Where(a => a.team != team).ToList();

        SkeletonAI bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (SkeletonAI potentialTarget in skeletons) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    void SetTarget(Transform target) {
        if (target != null)
            this.target = target;
        else
            this.target = null;
    }

    void SetNecromancerTarget() {
        if (team == Team.Enemy) {
            necromancerTarget = GameManager.instance.player.transform;
        } else if (team == Team.Player) {
            necromancerTarget = GameManager.instance.opponent.transform;
        }
    }

    void FixHealthUIBarRotation() {
        healthBarObj.localRotation = Quaternion.Euler(0, -transform.rotation.eulerAngles.y + 90, 0);
    }

    void SetHealthUIBar() {
        float mappedValue = Utility.Map(currentHealth, 0, 1, 0, health);
        healthBarObj.localScale = new Vector3(healthBarObj.localScale.x, healthBarObj.localScale.y, mappedValue);
    }

}
