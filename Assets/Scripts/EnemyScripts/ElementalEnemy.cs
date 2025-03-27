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
        rndnr = UnityEngine.Random.Range(0, 5);
        if(rndnr > 4 ) rndnr = 4;
        if(rndnr < 0 ) rndnr = 0;
        element = (Elements)rndnr;
        for (int i = 0;  i < colorchanging.Count; i++)
        {
            colorchanging[i].GetComponent<MeshRenderer>().material = textures[rndnr];
        }
        base.Start();
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
            //instatiate null particle
        }
        else
        {
            base.TakeDamage(dmg, element);
        }
    }
}
