using UnityEngine;

public class BatEnemy : Enemy
{
    [SerializeField] private float lifeSteal;
    [SerializeField] private float maxhealth;

    public override void Start()
    {
        base.Start();
        maxhealth = health * 2;
        lifeSteal = damage * 0.25f;
    }

    public override void Update()
    {
        if(health <= 0)
        {
            Die();
        }
        if(health > maxhealth)
        {
            health = maxhealth;
        }
        base.Update();
    }

    public override void Die()
    {
        Destroy(transform.parent.gameObject);
        base.Die();
    }
    public override void AttackPlayer(Collider col)
    {
        base.AttackPlayer(col);
        foreach (GameObject player in hitPlayers)
        {
            LifeSteal();
        }
    }
    
    private void LifeSteal()
    {
        health += lifeSteal;
    }
}
