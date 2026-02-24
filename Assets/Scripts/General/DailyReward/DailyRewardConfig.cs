using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyRewardConfig
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
