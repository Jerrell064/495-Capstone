﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    /* This script also goes on the player and controls the movement. It requires NavMeshAgent as a component
     * has a function that will assist in passing information to the battle script (TODO)
     * the region MovementStuff contains stuff to regulate movement and facing the target
     */ 
    Transform target;
    NavMeshAgent agent;
    [SerializeField]
    SceneMovement sceneMover;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    public void AttackMonster(Monster newTarget)
    {
        //this doesn't correctly calculate distance to stop
        //might also add in something to pass in information to the random event spawner
        agent.stoppingDistance = newTarget.radius * 1.8f;
        target = newTarget.transform;
        agent.updateRotation = false;
        Debug.Log(newTarget.name);
        BattleSystem.enemyUnit = newTarget;
        StartCoroutine(Engage(3f));
    }

    IEnumerator Engage(float delay/*, GameObject enemyToSpawn*/)
    {
        yield return new WaitForSeconds(delay);
        sceneMover.LoadLevel("Battle scene");
        //Instantiate(enemyToSpawn);
    }

    #region MovementStuff
    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        target = null;
        agent.updateRotation = true;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        //Debug.Log("This is direction: " + direction);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x * 2.5f , 0f, direction.z * 2.5f));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    #endregion 
}
