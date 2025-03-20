using UnityEngine;

public class IceShard : Spell
{
    [SerializeField] private GameObject IceField;
    private bool fieldSpawned;
    private Rigidbody rb;
    public override void Start()
    {
        base.Start();
        fieldSpawned = false;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 20f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 6)
        {
            if(collision.gameObject.layer == 9)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, 1);
            }
            if (!fieldSpawned)
            {
                Instantiate(IceField, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                fieldSpawned = true;
            }
            Destroy(gameObject);
        }
    }
}
