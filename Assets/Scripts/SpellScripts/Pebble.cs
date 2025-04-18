using UnityEngine;

public class Pebble : Spell
{
    private Rigidbody rb;
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }
    public override void Start()
    {
        transform.position = transform.position + (transform.up * Random.Range(-0.5f, 0.5f) + (transform.right * Random.Range(-0.5f, 0.5f)));
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 20f);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != 6)
        {
            if(collision.gameObject.layer == 9)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, 2);
            }
            Destroy(gameObject);
        }
    }
}
