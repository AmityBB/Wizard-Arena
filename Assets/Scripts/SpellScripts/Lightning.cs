using UnityEngine;

public class Lightning : Spell
{
    LayerMask mask;
    public override void Start()
    {
        mask = LayerMask.GetMask("Wall", "Ground", "Enemy");
        base.Start();
        CastLightning();
    }
    public void CastLightning()
    {
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position, player.cam.transform.TransformDirection(Vector3.forward), out hit, 100f, mask))
        {
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.layer == 9)
                {
                    gameObject.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);
                    gameObject.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage, 4);
                    hit.transform.gameObject.SendMessage("Chain");
                }
                else
                {
                    gameObject.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);
                    gameObject.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                }
            }
        }
        Destroy(gameObject, 0.1f);
    }
}
