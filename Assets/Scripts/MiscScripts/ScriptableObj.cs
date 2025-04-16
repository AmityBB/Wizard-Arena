using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Scores", order = 1)]
public class ScriptableObj : ScriptableObject
{
    public List<float> scoreList;
}
