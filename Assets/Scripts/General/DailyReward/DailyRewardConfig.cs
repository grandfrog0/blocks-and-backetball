using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyRewardConfig
{
    public UserConfig UserConfig;
    public long RefreshRewardTime = new TimeSpan(24, 0, 0).Ticks;
    [SerializeField] string refreshRewardTimeText;

    //private void OnValidate()
    //{
        //if (TimeSpan.TryParse(refreshRewardTimeText, out RefreshRewardTime))
        //{
            //Debug.Log(RefreshRewardTime.TotalMinutes);
        //}
    //}
}
