using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveManager : MonoBehaviour {

    public float rebornTime = 5f; //Seconds
    public float remainingRebornTime = 0f;
    public bool isDeath = false;

    public GameObject spawnPoint;
    public SkeletonAI skeletonAIPrefab;

    void Awake()
    {
        remainingRebornTime = rebornTime;
    }

    void Update()
    {
        if (isDeath == true)
        {
            remainingRebornTime -= Time.deltaTime;

            if (remainingRebornTime <= 0)
            {
                remainingRebornTime = rebornTime;
                BornIt();
            }
        }
    }

    void BornIt() {
        isDeath = false;
    }

    public void OnInteracted(SkeletonAI.Team team)
    {
        if (isDeath == false)
        {
            isDeath = true;
            InstantiateSkeleton(team);
        }
    }

    void InstantiateSkeleton(SkeletonAI.Team team) {
        SkeletonAI skeleton = Instantiate(skeletonAIPrefab, spawnPoint.transform.position, Quaternion.identity).GetComponent<SkeletonAI>();
        skeleton.SetTeam(team);
        GameManager.instance.allSkeletons.Add(skeleton);
    }
}
