using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : MonoBehaviour
{
    [SerializeField] private GameObject rat;
    [SerializeField] private List<GameObject> spawns;
    [SerializeField] private List<GameObject> rats;
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnRats());
    }
    private void Update()
    {
        foreach (GameObject r in rats)
        {
            if (r == null)
            {
                rats.Remove(r);
                break;
            }
        }
        if(rats.Count == 0)
        {
            GetComponent<Enemy>().speed = 3.5f;
        }
        else
        {
            GetComponent<Enemy>().speed = 5;
        }
    }
    private IEnumerator SpawnRats()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            GameObject clone = Instantiate(rat, spawns[i].transform.position, Quaternion.identity);
            gameManager.LiveEnemies.Add(clone);
            clone.GetComponent<Enemy>().deathScore = 0;
            rats.Add(clone);
        }
        yield return new WaitForSeconds(15);
        StartCoroutine(SpawnRats());
    }
}
