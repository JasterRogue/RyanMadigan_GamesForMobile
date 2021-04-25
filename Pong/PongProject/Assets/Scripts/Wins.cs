using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wins : MonoBehaviour
{
    public static int wins = 0;

    public static void incrementWins()
    {
        wins++;
        PlayerPrefs.SetInt("WinToUpdate", PlayerPrefs.GetInt("WinToUpdate", 0) + 1);
    }
}
