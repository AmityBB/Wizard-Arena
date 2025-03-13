using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    [SerializeField] private float sightRange;
    public float range;
    [SerializeField]private bool attacking = false;
    public float deathScore;
    
    public Player player;
    public NavMeshAgent m_agent;
    [SerializeField] private Collider hitbox;
    public List<GameObject> hitPlayers;
    private GameManager gameManager;
    public GameObject navObject;
    public Material defaultTexture;
    public Material hurtTexture;
    private Coroutine attackCoroutine;

    public virtual void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<Player>();
        m_agent = navObject.GetComponent<NavMeshAgent>();
        if (gameManager.currentWave > 1)
        {
            health += (health * 0.01f) * (gameManager.currentWave - 1);
            damage += (damage * 0.01f) * (gameManager.currentWave - 1);
            deathScore += (deathScore * 0.05f) * (gameManager.currentWave - 1);
        }
    }

    public virtual void Update()
    {
        m_agent.speed = speed;
        if (Vector3.Distance(navObject.transform.position, new Vector3(player.transform.position.x, navObject.transform.position.y, player.transform.position.z)) < sightRange)
        {
            navObject.transform.LookAt(new Vector3(player.transform.position.x, navObject.transform.position.y, player.transform.position.z));
            if (Vector3.Distance(navObject.transform.position, player.transform.position) > range)
            {
                attacking = false;
                ChasePlayer();
                StopCoroutine(attackCoroutine);
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
        if(health <= 0)
        {
            gameManager.AddScore(deathScore);
            Destroy(gameObject);
        }
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
        yield return new WaitForSeconds(1f);
        AttackPlayer(hitbox);
        attackCoroutine = StartCoroutine(Attacking());
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        gameObject.GetComponent<Renderer>().material = hurtTexture;
        StartCoroutine(TextureSet());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<Spell>() != null)
        { 
            TakeDamage(collision.gameObject.GetComponent<Spell>().damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject.GetComponent<Spell>() != null)
        {
            TakeDamage(other.gameObject.GetComponent<Spell>().damage);
        }
    }
    IEnumerator TextureSet()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().material = defaultTexture;
    }
}
