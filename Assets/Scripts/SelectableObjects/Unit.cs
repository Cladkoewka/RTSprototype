using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;
    public GameObject HealthBarPrefab;

    private int _maxHealth;
    private HealthBar _healthBar;
    public override void Start()
    {
        base.Start();
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab, transform.position, Quaternion.identity, transform);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
        _healthBar.SetHealth(Health, _maxHealth);
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        NavMeshAgent.SetDestination(point);
    }

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
        FindObjectOfType<SelectManager>().Unselect(this);
        Destroy(gameObject);
    }
}
