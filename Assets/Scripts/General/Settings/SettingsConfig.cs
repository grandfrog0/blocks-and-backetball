using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "SO/General/Settings Config")]
public class SettingsConfig : ScriptableObject
{
    public bool IsSoundOn = true;
    public bool IsMusicOn = true;
}
