using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class Achievements : MonoBehaviour
{
    public void openAcheivementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void updateIncremental()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_youre_a_natural, 1, null);
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_sorry_i_cant_hear_you_with_all_this_winning, 1, null);

    }

    public void updateIncrementalButton()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_damn_youre_really_good_at_pushing_that_button, 1, null);
    }



    public void unlockRegular()
    {
        Social.ReportProgress(GPGSIds.achievement_you_want_a_medal_for_pushing_a_button, 100f, null);
    }
}
