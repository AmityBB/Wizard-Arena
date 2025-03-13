using UnityEngine;

public class FireBall : Spell
{
    [SerializeField] private SphereCollider explosion;
    [SerializeField] private GameObject FireField;
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
            explosion.enabled = true;
            if (!fieldSpawned)
            {
                Instantiate(FireField, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                fieldSpawned = true;
            }
            Destroy(gameObject);
        }
    }
}
