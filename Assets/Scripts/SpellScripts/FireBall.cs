using UnityEngine;

public class FireBall : Spell
{
    [SerializeField] private SphereCollider explosion;
    [SerializeField] private GameObject FireField;
    [SerializeField] private bool fieldSpawned;
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
            Explode(explosion);
            if (!fieldSpawned)
            {
                Instantiate(FireField, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                fieldSpawned = true;
            }
            Destroy(gameObject);
        }
    }
    public void Explode(Collider collider)
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, mask);
        foreach (Collider c in cols)
        {
            c.GetComponent<Enemy>().TakeDamage(damage, 0);
        }
    }
}
