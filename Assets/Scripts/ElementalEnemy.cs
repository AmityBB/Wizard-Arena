using System;
using UnityEngine;

public class ElementalEnemy : Enemy
{
    public Material[] textures;
    private int rndnr;
    public enum Elements
    {
        Fire,
        Ice,
        Rock,
        Wind,
        Electric
    }
    public Elements element = Elements.Fire;
    public override void Start()
    {
        base.Start();
        rndnr = UnityEngine.Random.Range(0, 5);
        if(rndnr > 4 ) rndnr = 4;
        if(rndnr < 0 ) rndnr = 0;
        element = (Elements)rndnr;
        defaultTexture = GetComponent<MeshRenderer>().material = textures[rndnr];
    }
    public override void Update()
    {
        base.Update();
        speed = 4;
    }
    public override void TakeDamage(float dmg, int element)
    {
        if (element == rndnr)
        {
            Debug.Log("immune");
        }
        else
        {
            base.TakeDamage(dmg, element);
        }
    }
}
