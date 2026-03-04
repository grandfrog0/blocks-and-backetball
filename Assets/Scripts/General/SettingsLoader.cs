using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsLoader : MonoBehaviour
{
    private DailyRewardConfig _dailyRewardConfig;
    public DailyRewardConfig DailyRewardConfig 
    { 
        get
        {
            if (_dailyRewardConfig != null)
                return _dailyRewardConfig;

            Load();
            return _dailyRewardConfig;
        }
        private set => _dailyRewardConfig = value; 
    }
    private XmlItemParser<DailyRewardConfig> _parser;
    [SerializeField] AudioMixer mixer;

    private void Awake()
    {
        if (_dailyRewardConfig == null)
            Load();
    }
    private void Load()
    {
        _parser = new XmlItemParser<DailyRewardConfig>("General/DailyReward.xml", _dailyRewardConfig);
        _parser.Load();
        DailyRewardConfig = _parser.Value;

        ApplySettings();
    }

    public void Save()
    {
        _parser?.Save();
    }

    public void ApplySettings()
    {
        float musicVolume = DailyRewardConfig.UserConfig.SettingsConfig.IsMusicOn ? 0f : -80f;
        mixer.SetFloat("BackgroundVol", musicVolume);

        float soundVolume = DailyRewardConfig.UserConfig.SettingsConfig.IsSoundOn ? 0f : -80f;
        mixer.SetFloat("MasterVol", soundVolume);
    }
}
