using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Spell
{
    public NavMeshAgent m_agent;
    private GameManager gameManager;
    [SerializeField] private Collider hitbox;
    [SerializeField] private GameObject target;
    private float distance;
    [SerializeField] private bool isAttacking;
    [SerializeField] private float atkRange;
    [SerializeField] private float health;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<Player>();
        m_agent = gameObject.GetComponent<NavMeshAgent>();
    }
    public override void Start()
    {
        distance = Mathf.Infinity;
        Destroy(gameObject, 60f);
    }
    private void Update()
    {
        if (transform.position.y < -1 || health <= 0)
        { 
            Destroy(gameObject);
        }
            foreach (GameObject enemy in gameManager.LiveEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, enemy.transform.position);
                target = enemy;
            }
        }
        if (target == null)
        {
            distance = Mathf.Infinity;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > atkRange + 1)
        {
            StopAllCoroutines();
            m_agent.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            isAttacking = false;
        }
        else
        if (!isAttacking)
        {
            StartCoroutine(Attacking());
            isAttacking = true;
        }
        m_agent.speed = speed;
    }

    private void AttackEnemy(Collider c)
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] cols = Physics.OverlapBox(c.bounds.center, c.bounds.extents, Quaternion.identity, mask);
        foreach (Collider col in cols)
        {
            col.GetComponent<Enemy>().TakeDamage(damage, 10);
            health -= 1;
        }
    }
    IEnumerator Attacking()
    {
        AttackEnemy(hitbox);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attacking());
    }
}
