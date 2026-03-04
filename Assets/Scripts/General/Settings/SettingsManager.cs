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
        get => dailyRewardManager.SettingsLoader.DailyRewardConfig.UserConfig.SettingsConfig.IsSoundOn;
        set
        {
            dailyRewardManager.SettingsLoader.DailyRewardConfig.UserConfig.SettingsConfig.IsSoundOn = value;
            OnSoundValueChanged.Invoke(value);
        }
    }
    public bool IsMusicOn
    {
        get => dailyRewardManager.SettingsLoader.DailyRewardConfig.UserConfig.SettingsConfig.IsMusicOn;
        set
        {
            dailyRewardManager.SettingsLoader.DailyRewardConfig.UserConfig.SettingsConfig.IsMusicOn = value;
            OnMusicValueChanged.Invoke(value);
        }
    }

    private void Start()
    {
        OnSoundValueChanged.Invoke(IsSoundOn);
        OnMusicValueChanged.Invoke(IsMusicOn);
    }
}
