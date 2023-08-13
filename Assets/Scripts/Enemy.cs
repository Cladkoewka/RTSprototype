using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToUnit,
    WalkToBuilding,
    Attack
}
public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;
    public Unit CurrentUnitTarget;
    public Building CurrentBuildingTarget;
    public NavMeshAgent NavMeshAgent;
    public GameObject HealthBarPrefab;

    public int Health = 10;
    public float FollowDistance = 7f;
    public float AttackDistance = 1f;
    public float AttackPeriod = 1f;

    private int _maxHealth;
    private HealthBar _healthBar;
    private float _timer = 0;

    private void Start()
    {
        SetState(EnemyState.WalkToBuilding);
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab, transform.position, Quaternion.identity, transform);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
        _healthBar.SetHealth(Health, _maxHealth);
    }

    private void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();
            if (CurrentBuildingTarget)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosestUnit();
        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestUnit();
            if (CurrentBuildingTarget == null)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (CurrentUnitTarget)
            {
                NavMeshAgent.SetDestination(CurrentUnitTarget.transform.position);
                float distance = Vector3.Distance(transform.position, CurrentUnitTarget.transform.position);
                if (distance > FollowDistance)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < AttackDistance)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if (CurrentUnitTarget)
            {
                NavMeshAgent.SetDestination(CurrentUnitTarget.transform.position);
                float distance = Vector3.Distance(transform.position, CurrentUnitTarget.transform.position);
                if (distance > AttackDistance)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    CurrentUnitTarget.TakeDamage(1);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestBuilding();
            if (CurrentBuildingTarget)
            {
                NavMeshAgent.SetDestination(CurrentBuildingTarget.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
                _timer = 0;
        }
    }

    public void FindClosestBuilding()
    {
        Building[] buildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;
        for (int i = 0; i < buildings.Length; i++)
        {
            float distance = Vector3.Distance(buildings[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = buildings[i];
            }
        }
        CurrentBuildingTarget = closestBuilding;
    }

    public void FindClosestUnit()
    {
        Unit[] units = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;
        for (int i = 0; i < units.Length; i++)
        {
            float distance = Vector3.Distance(units[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = units[i];
            }
        }

        if (minDistance < FollowDistance)
        {
            CurrentUnitTarget = closestUnit;
            SetState(EnemyState.WalkToUnit);
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

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
