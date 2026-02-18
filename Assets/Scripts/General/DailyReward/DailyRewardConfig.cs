using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyReward", menuName = "SO/General/Daily Reward Config")]
public class DailyRewardConfig : ScriptableObject
{
    public UserConfig UserConfig;
    public float PrizeCount;
    public TimeSpan RefreshRewardTime;
    [SerializeField] string refreshRewardTimeText;

    private void OnValidate()
    {
        if (TimeSpan.TryParse(refreshRewardTimeText, out RefreshRewardTime))
        {
            //Debug.Log(RefreshRewardTime.TotalMinutes);
        }
    }
}
