using UnityEngine;

public class Firefield : MonoBehaviour
{
    public float damage;
    public float duration;
    private void Start()
    {
        Destroy(gameObject, duration);
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
