using UnityEngine;

public class BatEnemy : Enemy
{
    [SerializeField] private float lifeSteal;
    [SerializeField] private float maxhealth;

    public override void Start()
    {
        base.Start();
        maxhealth = health * 1.5f;
        lifeSteal = damage / 8;
    }

    public override void Update()
    {
        if(health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
        if(health > maxhealth)
        {
            health = maxhealth;
        }
        base.Update();
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
