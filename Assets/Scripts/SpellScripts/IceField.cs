using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class IceField : MonoBehaviour
{
    public float slowdownMult;
    public List<GameObject> colliding;
    public float duration;
    public enum Elements
    {
        Ice
    }
    public Elements element = Elements.Ice;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<Enemy>().speed /= slowdownMult;
            colliding.Add(collision.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<Enemy>().speed *= slowdownMult;
            colliding.Remove(collision.gameObject);
        }
    }
    private void OnDestroy()
    {
        foreach (GameObject obj in colliding)
        {
            obj.GetComponent<Enemy>().speed *= slowdownMult;
        }
    }
}
