using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private int score = 0;

    public void AddScore(int additionalScore)
    {
        score += additionalScore;
    }
}
