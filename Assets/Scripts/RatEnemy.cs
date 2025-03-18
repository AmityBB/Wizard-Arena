using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : Enemy
{
    [SerializeField] private float poisonDmg;
    [SerializeField] private int poisontics;
    [SerializeField] private bool m_poisoner;

    public override void Start()
    {
        base.Start();
        poisonDmg = damage / 4;
    }
    public override void Update()
    {
        base.Update();
        if (m_poisoner)
        {
            player.GetComponent<Player>().poisoned = true;
        }
        else
        {
            player.GetComponent<Player>().poisoned = false;
        }
    }
    public override void AttackPlayer(Collider col)
    {
        base.AttackPlayer(col);
        foreach (GameObject player in hitPlayers) 
        {
            StartCoroutine(Poison(player, poisontics));
        }
    }
    private IEnumerator Poison(GameObject target, int time)
    {
        m_poisoner = true;
        player.GetComponent<Player>().poisoned = true;
        yield return new WaitForSeconds(1);
        target.GetComponent<Player>().TakeDamage(poisonDmg);
        hitPlayers.Remove(target);
        time--;
        if (time > 0)
        {
            StartCoroutine(Poison(target, time));
        }
        else
        {
            m_poisoner = false;
        }
    }
    private void OnDestroy()
    {    
            player.GetComponent<Player>().poisoned = false;
    }
}
