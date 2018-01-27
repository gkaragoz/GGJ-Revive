using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(BornAnimation());
    }

    void Update() {
        if (target == null) {
            SetTarget(necromancerTarget);
            return;
        }

        if (isBorning == false) {
            agent.SetDestination(necromancerTarget.position);
        }
    }

    public void SetTeam(Team team) {
        this.team = team;

        SetNecromancerTarget();
    }

    IEnumerator BornAnimation() {
        isBorning = true;
        Debug.Log("I'm borning: " + this.name);
        yield return new WaitForSeconds(2f);
        Debug.Log("I borned: " + this.name);
        isBorning = false;
    }

    void FindEnemy() {

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

}
