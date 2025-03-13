using System.Collections;
using UnityEngine;

public class WindSpell : Spell
{
    public float windMult;
    public void Cast()
    {
        if (player.grounded)
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
            StartCoroutine(SpellEnabledTime());
            player.rb.AddForce(transform.up * windMult, ForceMode.Impulse);
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
            StartCoroutine(SpellEnabledTime());
            player.rb.AddForce(player.cam.transform.forward * windMult, ForceMode.Impulse);
        }
        ConsumeMana();
    }

    IEnumerator SpellEnabledTime()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SphereCollider>().enabled = false;

    }
}
