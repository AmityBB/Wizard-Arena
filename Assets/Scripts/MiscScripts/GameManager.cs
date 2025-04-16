using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score;
    public int currentWave;
    public int waveFromBoss;
    private float timer;
    [SerializeField]
    public bool started = false;
    public bool locked;

    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI waveTimerTxt;
    public List<BoxCollider> spawnAreas;
    public List<GameObject> enemies;
    public List<GameObject> Bosses;
    public List<GameObject> LiveEnemies;
    private Coroutine m_coroutine = null;
    [SerializeField]
    private Player player;
    public GameObject pauseScreen;
    private ScoreManager scoreManager;

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
        scoreManager = FindFirstObjectByType<ScoreManager>();
        player = FindFirstObjectByType<Player>();
        started = false;
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        StopAllCoroutines();
        if (LiveEnemies != null)
        {
            foreach (GameObject obj in LiveEnemies)
            {
                if (obj.GetComponent<Enemy>() != null)
                {
                    obj.GetComponent<Enemy>().Die();
                }
                else
                {
                    obj.GetComponentInChildren<Enemy>().Die();
                }
            }
        }
        timer = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        locked = true;
        player.enabled = true;
        player.cam.GetComponent<PlayerCamera>().enabled = true;
        player.deathScreen.GetComponent<Canvas>().enabled = false;
        player.isDead = false;
        player.poisoned = false;
        player.health = 1000;
        player.mana = 1000;
        player.transform.position = new Vector3(0, 1, 0);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        score = 0;
        waveFromBoss = 0;
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
                    break;
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
        waveFromBoss++;
        if (waveFromBoss == 10)
        {
            LiveEnemies.Add(Instantiate(Bosses[Random.Range(0, Bosses.Count)], spawnAreas[0].transform.position, Quaternion.identity));
            StopAllCoroutines();
            m_coroutine = StartCoroutine(WaveTimer(60));
            timer = 60;
            waveFromBoss = 0;
            started = true;
        }
        else
        {
            foreach (BoxCollider box in spawnAreas)
            {
                for (int i = 0; i < wave; i++)
                {
                    Vector3 position = RandomPosInBox(box.bounds);
                    LiveEnemies.Add(Instantiate(enemies[Random.Range(0, enemies.Count)], position, Quaternion.identity));
                    if (i == wave - 1)
                    {
                        StopAllCoroutines();
                        m_coroutine = StartCoroutine(WaveTimer(60));
                        timer = 60;
                        started = true;
                    }
                }
            }
        }
    }
    private IEnumerator WaveTimer(int time)
    {
        yield return new WaitForSeconds(time);
        currentWave++;
        StartWave(currentWave);
    }

    public void Pause()
    {
        if (locked)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            locked = false;
            player.cam.GetComponent<PlayerCamera>().enabled = false;
            pauseScreen.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            locked = true;
            player.cam.GetComponent<PlayerCamera>().enabled = true;
            pauseScreen.GetComponent<Canvas>().enabled = false;
        }
    }

    public void EndGame()
    {
        scoreManager.SetScore(score);
    }
}
