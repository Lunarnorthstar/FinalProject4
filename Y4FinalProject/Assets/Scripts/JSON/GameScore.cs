using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "ScoreKeeper", menuName = "ScriptableObjects/Score", order = 1)]

public class GameScore : ScriptableObject
{
        public float previousScore;
        public float highScore;

}
