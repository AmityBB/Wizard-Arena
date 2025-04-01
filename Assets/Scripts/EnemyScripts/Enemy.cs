using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    public float range;
    public float deathScore;
    [SerializeField] private float sightRange;
    [SerializeField]private bool attacking = false;
    
    private GameManager gameManager;
    public Player player;
    public NavMeshAgent m_agent;
    [SerializeField] private Collider hitbox;
    public List<GameObject> hitPlayers;
    public GameObject navObject;
    public GameObject lightning;
    public Material defaultTexture;
    public Material hurtTexture;
    public List<GameObject> colorchanging;
    public List<Material> baseColors;
    private Coroutine attackCoroutine;

    public virtual void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<Player>();
        m_agent = navObject.GetComponent<NavMeshAgent>();
    }
    public virtual void Start()
    {
        if (gameManager.currentWave > 1)
        {
            health += (health * 0.01f) * (gameManager.currentWave - 1);
            damage += (damage * 0.01f) * (gameManager.currentWave - 1);
            deathScore += (deathScore * 0.05f) * (gameManager.currentWave - 1);
        }
        for(int i = 0; i < colorchanging.Count; i++)
        {
            baseColors.Add(colorchanging[i].GetComponent<Renderer>().material);
        }
    }

    public virtual void Update()
    {
        if (transform.position.y < -1) 
        {
            deathScore = 0;
            Die();
        }
        m_agent.speed = speed;
        if (Vector3.Distance(navObject.transform.position, new Vector3(player.transform.position.x, navObject.transform.position.y, player.transform.position.z)) < sightRange)
        {
            navObject.transform.LookAt(new Vector3(player.transform.position.x, navObject.transform.position.y, player.transform.position.z));
            if (Vector3.Distance(navObject.transform.position, player.transform.position) > range-1)
            {
                attacking = false;
                ChasePlayer();
                hitPlayers.Clear();
            }
            else
            {
                m_agent.ResetPath();
                if (!attacking)
                {
                    attackCoroutine = StartCoroutine(Attacking());
                    attacking = true;
                }
            }
        }
    }

    public virtual void Chain()
    {
        foreach(GameObject enemy in gameManager.LiveEnemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < 10 && enemy != gameObject)
            {
                GameObject clone = Instantiate(lightning, Vector3.zero, Quaternion.identity);
                clone.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                clone.GetComponent<LineRenderer>().SetPosition(1, enemy.transform.position);
                enemy.GetComponentInChildren<Enemy>().TakeDamage(lightning.GetComponent<ChainedLightning>().damage, 4);
            }
        }
    }

    public virtual void Die()
    {
        gameManager.AddScore(deathScore);
        Destroy(gameObject);
    }
    public virtual void ChasePlayer()
    {
        m_agent.SetDestination(new Vector3(player.transform.position.x, navObject.transform.position.y, player.transform.position.z));
    }

    public virtual void AttackPlayer(Collider col)
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity, mask);
        if(cols.Length == 0)
        {
            hitPlayers.Clear();
        }
        foreach(Collider c in cols)
        {
            hitPlayers.Add(c.gameObject);
            c.GetComponent<Player>().TakeDamage(damage);
        }
    }

    IEnumerator Attacking()
    {
        AttackPlayer(hitbox);
        yield return new WaitForSeconds(1f);
        attackCoroutine = StartCoroutine(Attacking());
    }
    public virtual void TakeDamage(float dmg, int element)
    {
        health -= dmg;
        for(int i = 0; i < colorchanging.Count; i++)
        {
            colorchanging[i].GetComponent<Renderer>().material = hurtTexture;
            StartCoroutine(TextureSet(baseColors[i], colorchanging[i]));
        }
        if (health <= 0)
        {
            Die();
        }
    }
    IEnumerator TextureSet(Material color, GameObject part)
    {
        yield return new WaitForSeconds(0.5f);
        part.GetComponent<Renderer>().material = color;
    }
}
