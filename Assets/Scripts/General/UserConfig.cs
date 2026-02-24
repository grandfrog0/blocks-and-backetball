using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "UserConfig", menuName = "SO/General/User Config")]
[Serializable]
public class UserConfig
{
    public float BestScore;
    public float Coins;
    public SettingsConfig SettingsConfig;
    public List<float> DailyRewards;
    public float LastReward;

    public DateTime PrizeTime;
    [SerializeField] string prizeTimeText;

    private void OnValidate()
    {
        if (DateTime.TryParse(prizeTimeText, out PrizeTime))
        {
            Debug.Log(PrizeTime);
        }
    }
}
