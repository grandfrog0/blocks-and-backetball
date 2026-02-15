using System;
using System.Collections;
using System.Collections.Generic;
using MainStore;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MinigameView : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image iconImage;
    [SerializeField] FormattedText ingameTimeText;

    public void Subscribe(StoreMinigame minigame, UnityAction onClick)
    {
        iconImage.sprite = minigame.Icon;
        ingameTimeText.SetValues(minigame.IngameTime / 3600, minigame.IngameTime / 60 % 60);

        button.onClick.AddListener(onClick);
    }
}
