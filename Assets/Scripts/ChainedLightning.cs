using UnityEngine;

public class ChainedLightning : MonoBehaviour
{
    public float damage;
    public enum Elements
    {
        Electric
    }
    public Elements element = Elements.Electric;
    private void Start()
    {
        Destroy(gameObject, 0.1f);
    }
}
