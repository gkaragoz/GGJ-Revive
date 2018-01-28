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
    public float baseDamage = 2f;
    public float currentDamage = 2f;
    public float upgradeAmount = 0f;

    [Header("Movement Values")]
    public float movementSpeed = 2.5f;
    public float angularSpeed = 360;
    public float acceleration = 8;
    public float stoppingDistance = 0f;
    public float bornOffsetTime = -0.2f;
    public bool autoBraking = true;
    [HideInInspector]
    public Transform target;
    public Transform necromancerTarget;
    public TextMesh txtStats;

    public Color opponentColor;
    public Color playerColor;
    private Animator anim;
    private float bornRotateVariation = 50f;

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

        currentDamage = baseDamage;
        attackSpeed = Random.Range(1f, 2f);
        bornOffsetTime = Random.Range(-0.2f, -0.5f);
        bornRotateVariation = Random.Range(50f, 150f);
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        RotateALilBit();
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

        if (GameManager.instance.GameFinished) {
            StopAgent();
            anim.SetBool("isAttacking", false);
            anim.SetBool("Walk", false);
            return;
        }

        SkeletonAI skeleton = GetClosestSkeleton();
        if (skeleton == null || skeleton.isDeath == true) {
            SetTarget(necromancerTarget);
            if (HasTargetReached(necromancerTarget)) {
                if (necromancerTarget.tag == "Player") {
                    if (necromancerTarget.GetComponent<PlayerController>().isDeath == false) {
                        if (isAttacking == false) {
                            StartCoroutine(Attack(necromancerTarget));
                        }
                    } else {
                        necromancerTarget = null;
                        skeleton = null;
                    }
                } else if (necromancerTarget.tag == "Opponent") {
                    if (necromancerTarget.GetComponent<OpponentAI>().isDeath == false) {
                        if (isAttacking == false) {
                            StartCoroutine(Attack(necromancerTarget));
                        }
                    } else {
                        necromancerTarget = null;
                        skeleton = null;
                    }
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

            if (agent != null && target != null)
                agent.SetDestination(target.position);
        }
    }

    public void SetStatsText() {
        if (txtStats == null)
            return;

        if (team == Team.Player) {
            txtStats.color = playerColor;
        } else if (team == Team.Enemy) {
            txtStats.color = opponentColor;
        }

        txtStats.text = baseDamage + " + " + upgradeAmount;
    }

    IEnumerator Attack(Transform obj) {
        anim.SetBool("Walk", false);
        isAttacking = true;
        anim.SetBool("isAttacking", isAttacking);
        StopAgent();

        if (obj.gameObject.tag == "Skeleton")
        {
            SkeletonAI skeleton = obj.GetComponent<SkeletonAI>();

            while (skeleton.isDeath == false && HasTargetReached(skeleton.transform) == true)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(attackSpeed);
                StartCoroutine(skeleton.HitDamage(currentDamage));
            }
        }
        else if (obj.gameObject.tag == "Player")
        {
            PlayerController player = obj.GetComponent<PlayerController>();

            while (player.isDeath == false && HasTargetReached(player.transform) == true)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(attackSpeed);
                StartCoroutine(player.HitDamage(currentDamage));
            }
        }
        else if (obj.gameObject.tag == "Opponent")
        {
            OpponentAI opponent = obj.GetComponent<OpponentAI>();

            while (opponent.isDeath == false && HasTargetReached(opponent.transform) == true)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(attackSpeed);
                StartCoroutine(opponent.HitDamage(currentDamage));
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

    public void TakePower(float amount) {
        upgradeAmount += amount;
        currentDamage = baseDamage + upgradeAmount;
        SetStatsText();
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

    public void RotateALilBit() {
        transform.Rotate(Vector3.up * bornRotateVariation); 
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

        yield return new WaitForSeconds(bornOffsetTime + AnimationDatas.instance.GetAnimationLength(AnimationDatas.AnimationStates.Born));

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
