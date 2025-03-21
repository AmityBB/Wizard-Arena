using UnityEngine;

public class RockSpike : Spell
{
    LayerMask mask;
    public override void Start()
    {
        mask = LayerMask.GetMask("Wall", "Ground", "Enemy");
        base.Start();
        CastRock();
        Destroy(gameObject,5f);
    }

    private void CastRock()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.cam.transform.TransformDirection(Vector3.forward), out hit, 100f, mask))
        {
            Debug.DrawRay(player.transform.position, player.cam.transform.TransformDirection(Vector3.forward), Color.red, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    transform.position = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage, 2);
                }
                else
                {
                    transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
        }
    }
}
