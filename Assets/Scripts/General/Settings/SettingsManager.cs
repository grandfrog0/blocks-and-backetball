using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> OnSoundValueChanged = new();
    [SerializeField] UnityEvent<bool> OnMusicValueChanged = new();
    [SerializeField] SettingsConfig Settings;

    public bool IsSoundOn
    {
        get => Settings.IsSoundOn;
        set => Settings.IsSoundOn = value;
    }
    public bool IsMusicOn
    {
        get => Settings.IsMusicOn;
        set => Settings.IsMusicOn = value;
    }

    private void OnEnable()
    {
        OnSoundValueChanged.Invoke(IsSoundOn);
        OnMusicValueChanged.Invoke(IsMusicOn);
    }
}
