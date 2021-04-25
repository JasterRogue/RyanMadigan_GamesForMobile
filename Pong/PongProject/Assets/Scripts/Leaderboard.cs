using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public void openLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void updateLeaderBoardScore()
    {
        if (PlayerPrefs.GetInt("WinToUpdate", 0) == 0)
        {
            return;
        }

        Social.ReportScore(PlayerPrefs.GetInt("WinToUpdate", 1), GPGSIds.leaderboard_total_wins, (bool sucess) =>
        {
            if (sucess)
            {
                PlayerPrefs.SetInt("WinToUpdate", 0);
            }
        });
    }
}
