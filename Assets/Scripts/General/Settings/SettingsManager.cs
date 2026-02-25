using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> OnSoundValueChanged = new();
    [SerializeField] UnityEvent<bool> OnMusicValueChanged = new();
    [SerializeField] DailyRewardManager dailyRewardManager;

    public bool IsSoundOn
    {
        get => dailyRewardManager.DailyRewardConfig.UserConfig.SettingsConfig.IsSoundOn;
        set => dailyRewardManager.DailyRewardConfig.UserConfig.SettingsConfig.IsSoundOn = value;
    }
    public bool IsMusicOn
    {
        get => dailyRewardManager.DailyRewardConfig.UserConfig.SettingsConfig.IsMusicOn;
        set => dailyRewardManager.DailyRewardConfig.UserConfig.SettingsConfig.IsMusicOn = value;
    }

    private void Start()
    {
        OnSoundValueChanged.Invoke(IsSoundOn);
        OnMusicValueChanged.Invoke(IsMusicOn);
    }
}
