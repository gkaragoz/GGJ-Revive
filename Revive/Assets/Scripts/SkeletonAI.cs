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
    public bool isDeath = false;

    [Header("Settings")]
    public bool overrideAgentValues = true;
    public Team team;

    [Header("Combat Values")]
    public float maxHealth = 10f;
    public float currentHealth = 0f;
    public float attackSpeed = 2f;
    public float attackRange = 2f;
    public float attackDamage = 2f;
    public float upgradeAmount = 0f;

    [Header("Movement Values")]
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;
    public Transform necromancerTarget;
    public TextMesh txtStats;

    private Animator anim;

    public float Health {
        get { return currentHealth; }
        set {
            currentHealth = value;
            if (currentHealth <= 0) {
                Die();
            }
        }
    }

    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        StartCoroutine(BornAnimation());
    }

    void Update() {
        //search for closest skeleton
        //if there is no skeleton than settarget necromancer
        //if it finds closest skeleton
        //than go to that skeleton

        //if that skeleton reached enemy
        //than attack it

        if (isDeath)
            return;

        LookAtCamera(txtStats.transform.parent);

        if (GameManager.instance.isGameFinished) {
            StopAgent();
            anim.SetBool("isAttacking", false);
            anim.SetBool("Walk", false);
            return;
        }

        SkeletonAI skeleton = GetClosestSkeleton();
        if (skeleton == null) {
            SetTarget(necromancerTarget);
            if (HasTargetReached(necromancerTarget)) {
                if (necromancerTarget.GetComponent<PlayerController>().isDeath == false) {
                    if (isAttacking == false) {
                        StartCoroutine(Attack(necromancerTarget));
                    }
                } else {
                    necromancerTarget = null;
                    skeleton = null;
                }
            }
        } else if (isAttacking == false && HasTargetReached(skeleton.transform) == true) {
            StartCoroutine(Attack(skeleton.transform));
        } else if (isAttacking == false && skeleton.isDeath == false && HasTargetReached(skeleton.transform) == false) {
            SetTarget(skeleton.transform);
        } else {
            SetTarget(necromancerTarget);
        }

        if (isBorning == false && isAttacking == false) {
            anim.SetBool("Walk", true);
            agent.SetDestination(target.position);
        }
    }

    void SetStatText() {
        if (team == Team.Player) {
            txtStats.color = new Color(23, 179, 210);
        } else {
            txtStats.color = new Color(210, 89, 23);
        }

        txtStats.text = attackDamage + " + " + upgradeAmount;
    }

    IEnumerator Attack(Transform obj) {
        anim.SetBool("Walk", false);
        isAttacking = true;
        anim.SetBool("isAttacking", isAttacking);
        StopAgent();
        
        if (obj.gameObject.tag == "Skeleton") {
            SkeletonAI skeleton = obj.GetComponent<SkeletonAI>();

            while (skeleton.isDeath == false) {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(attackSpeed);
                StartCoroutine(skeleton.HitDamage(attackDamage));
            }
        } else if (obj.gameObject.tag == "Player") {
            PlayerController player = obj.GetComponent<PlayerController>();

            while (player.isDeath == false)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(attackSpeed);
                StartCoroutine(player.HitDamage(attackDamage));
            }
        }

        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        ReleaseAgent();
    }

    IEnumerator HitDamage(float amount) {
        Health -= amount;
        //Take hit animation.
        yield return new WaitForSeconds(attackSpeed);
    }

    bool HasTargetReached(Transform target) {
        if (target == null)
            return false;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange){
            return true;
        }

        return false;
    }

    public void SetTeam(Team team) {
        this.team = team;

        SetNecromancerTarget();
    }

    void ReleaseAgent() {
        if (agent != null)
            agent.isStopped = false;
    }

    void StopAgent() {
        if (agent != null)
            agent.isStopped = true;
    }

    IEnumerator BornAnimation() {
        isBorning = true;
        StopAgent();
        yield return new WaitForSeconds(AnimationDatas.instance.GetAnimationLength(AnimationDatas.AnimationStates.Born));

        ReleaseAgent();
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

    void LookAtCamera(Transform obj) {
        obj.LookAt(Camera.main.transform);
    }

    void Die() {
        isDeath = true;
        isBorning = false;
        isAttacking = false;
        maxHealth = 0;
        anim.SetTrigger("Die");
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("Walk", false);
        StopAgent();

        if (agent != null)
            Destroy(agent);

        if (txtStats != null)
            Destroy(txtStats.transform.parent.gameObject);
    }

}
