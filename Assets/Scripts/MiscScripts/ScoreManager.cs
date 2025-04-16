using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public float runScore;
    public static ScoreManager instance;
    public ScriptableObj scoreObj;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetScore(float s)
    {
        runScore = s;
        if (scoreObj.scoreList.Count < 10)
        {
            scoreObj.scoreList.Add(runScore);
            Debug.Log("added");
        }
        else
        {
            foreach (float sc in scoreObj.scoreList)
            {
                if (runScore > sc)
                {
                    scoreObj.scoreList.Insert(scoreObj.scoreList.IndexOf(sc), runScore);
                    scoreObj.scoreList.RemoveAt(10);
                    break;
                }
            }
            Debug.Log("replaced");
        }
        scoreObj.scoreList = scoreObj.scoreList.OrderByDescending(x => x).ToList();
    }
}
