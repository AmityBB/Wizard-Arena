using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score;
    public int currentWave;
    private float timer;
    [SerializeField]
    private bool started = false;

    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI waveTimerTxt;
    public List<BoxCollider> spawnAreas;
    public List<GameObject> enemies;
    public List<GameObject> LiveEnemies;
    private Coroutine m_coroutine = null;

    public static Vector3 RandomPosInBox(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            1,
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }

    void Start()
    {
        currentWave = 1;
        StartWave(currentWave);
    }

    void Update()
    {
        scoreTxt.text = "Score: " + score.ToString("0");
        if (started)
        {
            if (LiveEnemies.Count == 0)
            {
                StopCoroutine(m_coroutine);
                currentWave++;
                StartWave(currentWave);
            }
            foreach (GameObject enemy in LiveEnemies)
            {
                if (enemy == null)
                {
                    LiveEnemies.Remove(enemy);
                }
            }
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int seconds = (int)timer % 60;
        }
        waveTimerTxt.text = timer.ToString("F0");
    }
    public void AddScore(float scorepoints)
    {
        score += scorepoints;
    }

    public void StartWave(int wave)
    {
        foreach (BoxCollider box in spawnAreas)
        {
            for (int i = 0; i < wave; i++)
            {
                Vector3 position = RandomPosInBox(box.bounds);
                LiveEnemies.Add(Instantiate(enemies[Random.Range(0, enemies.Count)], position, Quaternion.identity));
                if(i == wave -1)
                {
                    StopAllCoroutines();
                    m_coroutine = StartCoroutine(WaveTimer());
                    timer = 60;
                    started = true;
                }
            }
        }
    }
    private IEnumerator WaveTimer()
    {
        yield return new WaitForSeconds(60f);
        currentWave++;
        StartWave(currentWave);
    }
}
