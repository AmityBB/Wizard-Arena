using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    private ScoreManager scoreManager;
    public ScriptableObj scoreObj;
    [SerializeField] private TextMeshProUGUI lastRun;
    [SerializeField] private List<TextMeshProUGUI> top5;
    [SerializeField] private float lastRunScore;
    [SerializeField] private List<float> topScores;

    
    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        lastRunScore = scoreManager.runScore;
        for(int i = 0; i < scoreObj.scoreList.Count; i++)
        {
            topScores[i] = scoreObj.scoreList[i];
        }
    }
    void Update()
    {
        for(int i = 0; i < topScores.Count; i++)
        {
            top5[i].text = (i+1) + ":" + topScores[i].ToString();
        }
        lastRun.text = "Run: " + lastRunScore;
    }
}
