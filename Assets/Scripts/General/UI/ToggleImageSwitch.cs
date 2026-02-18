using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageSwitch : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color isOnColor, isOffColor;

    public void SetValue(bool value)
    {
        image.color = value ? isOnColor : isOffColor;
    }
}
