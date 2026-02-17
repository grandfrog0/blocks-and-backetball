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

        float ingameTimeMinutes = minigame.IngameTime / 60f;
        ingameTimeText.SetValues(ingameTimeMinutes / 60, ingameTimeMinutes % 60);

        button.onClick.AddListener(onClick);
    }
}
