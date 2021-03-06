﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Transform areaOnHealFXObj;

    private float bonusAmount;
    private bool targetReacted = false;
    private float movemenetSpeed = 45f;
    private float hitRange = 0.3f;

    private SkeletonAI.Team team;
    private Rigidbody rigidbody;
    private Transform target;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (target != null) {
            if (HasTargetReached() == false) {
                Vector3 direction = target.position - transform.position;
                transform.Translate(direction.normalized * movemenetSpeed * Time.fixedDeltaTime);
            } else if (targetReacted == false) {
                OnTargetReached();
            }
        }
    }

    void OnTargetReached() {
        if (target.GetComponent<SkeletonAI>().isDeath)
            return;

        target.GetComponent<SkeletonAI>().TakePower(bonusAmount);
        targetReacted = true;
        Instantiate(areaOnHealFXObj.gameObject, 
            new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 
            Quaternion.identity);
    }

    bool HasTargetReached() {
        return Vector3.Distance(transform.position, target.position) <= hitRange ? true : false;
    }

    public void SetTarget(Transform target, SkeletonAI.Team myTeam, float bonusAmount) {
        this.target = target;
        team = myTeam;
        this.bonusAmount = bonusAmount;
    }
}
