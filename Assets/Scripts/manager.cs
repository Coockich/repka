using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts._4;
using System.Linq;
using Platformer;

public class manager : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private hud levelHUD;
    private List<Coins> coinsList;
    private LevelProgress progress;

    private void Awake()
    {
        progress = new LevelProgress();
        coinsList = FindObjectsOfType<Coins>().ToList();

        foreach (Coins coin in coinsList)
        {
            coin.SetLevelManager(this);
        }
    }
    public void UpdateScore(int score)
    {
        progress.LevelScore += score;
        levelHUD.UpdateScore(progress.LevelScore);
    }
}
