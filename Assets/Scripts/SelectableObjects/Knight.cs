using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{
    public UnitState CurrentUnitState;
    public Vector3 CurrentTargetPoint;
    public Enemy CurrentTargetEnemy;

    public float FollowDistance = 7f;
    public float AttackDistance = 1f;
    public float AttackPeriod = 1f;

    private float _timer = 0;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.WalkToPoint);
    }

    private void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (CurrentTargetEnemy)
            {
                NavMeshAgent.SetDestination(CurrentTargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, CurrentTargetEnemy.transform.position);
                if (distance > FollowDistance)
                {
                    SetState(UnitState.WalkToPoint);
                }
                if (distance < AttackDistance)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (CurrentTargetEnemy)
            {
                NavMeshAgent.SetDestination(CurrentTargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, CurrentTargetEnemy.transform.position);
                if (distance > AttackDistance)
                {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    CurrentTargetEnemy.TakeDamage(1);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;
        if (CurrentUnitState == UnitState.Idle)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        { 

        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {

        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            _timer = 0;
        }
    }


    public void FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        for (int i = 0; i < enemies.Length; i++)
        {
            float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemies[i];
            }
        }

        if (minDistance < FollowDistance)
        {
            CurrentTargetEnemy = closestEnemy;
            SetState(UnitState.WalkToEnemy);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, AttackDistance);
        Gizmos.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, FollowDistance);
    }
#endif
}
