using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : Enemy
{
    [SerializeField] private float poisonDmg;
    [SerializeField] private int poisontics;

    public override void Start()
    {
        base.Start();
        poisonDmg = damage / 4;
    }
    public override void AttackPlayer(Collider col)
    {
        base.AttackPlayer(col);
        foreach (GameObject player in hitPlayers) 
        {
            player.GetComponent<Player>().poisoned = true;
            StartCoroutine(Poison(player, poisontics));
        }
    }
    private IEnumerator Poison(GameObject target, int time)
    {
        yield return new WaitForSeconds(1);
        target.GetComponent<Player>().TakeDamage(poisonDmg);
        hitPlayers.Remove(target);
        time--;
        if (time > 0)
        {
            player.GetComponent<Player>().poisoned = true;
            StartCoroutine(Poison(target, time));
        }
        else
        {
            player.GetComponent<Player>().poisoned = false;
        }
    }
}
