using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WindSpell : Spell
{
    public float windMult;
    public Collider hitbox;
    public void Cast()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        StartCoroutine(SpellEnabledTime());
        if (player.grounded)
        {
            player.rb.AddForce(transform.up * windMult, ForceMode.Impulse);
        }
        else
        {
            player.rb.AddForce(player.cam.transform.forward * windMult, ForceMode.Impulse);
        }
        ConsumeMana();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage, 3);
        }
    }
    IEnumerator SpellEnabledTime()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SphereCollider>().enabled = false;

    }
}
