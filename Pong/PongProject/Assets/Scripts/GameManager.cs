using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;

    public GUISkin layout;

    GameObject theBall;


    // Start is called before the first frame update
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void score(string wallID)
    {
        if(wallID == "RightWall")
        {
            PlayerScore1++;
        }

        else
        {
            PlayerScore2++;
        }
    }//end of score()

    private void OnGUI()
    {
        
        GUI.skin = layout;
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 150 - 12, 20, 100, 100), "" + PlayerScore2);

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theBall.SendMessage("restartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if(PlayerScore1 == 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            theBall.SendMessage("resetBall", null, SendMessageOptions.RequireReceiver);

            if (PlayerScore1 == 10 && PlayerScore2 == 9)
            {
                Social.ReportProgress(GPGSIds.achievement_clutching_out_the_win, 100f, null);
            }

            Wins.incrementWins();
            Social.ReportProgress(GPGSIds.achievement_just_warming_up, 100f, null);

        }

        else if (PlayerScore2 == 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            theBall.SendMessage("resetBall", null, SendMessageOptions.RequireReceiver);

            Social.ReportProgress(GPGSIds.achievement_its_the_end_of_the_world, 100f, null);
        }
    }
}
