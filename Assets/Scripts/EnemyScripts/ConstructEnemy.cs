using UnityEngine;

public class ConstructEnemy : Enemy
{
    public float manaSteal;
    public float maxDamage;

    public override void Start()
    {
        base.Start();
        manaSteal = damage * 0.75f;
        maxDamage = damage * 2.5f;
    }
    public override void AttackPlayer(Collider col)
    {
        base.AttackPlayer(col);
        foreach (GameObject player in hitPlayers)
        {
            ManaSteal();
        }
    }
    private void ManaSteal()
    {
        damage += manaSteal / 2;
        player.mana -= manaSteal;
    }
}
