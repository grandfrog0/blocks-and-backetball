using Blocks3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DailyRewardManager : MonoBehaviour
{
    public UnityEvent OnBeforeDailyRewardGot = new();

    [SerializeField] FormattedText rewardText, rewardTimeText;
    [SerializeField] FormattedText coinsText;

    [SerializeField] string waitRewardFormat = "Reward in<br>{0:00}:{1:00}:{2:00}";
    [SerializeField] string readyRewardFormat = "Reward is ready!";

    public DateTime PrizeTime
    {
        get => dailyRewardConfig.UserConfig.PrizeTime;
        set => dailyRewardConfig.UserConfig.PrizeTime = value;
    }

    [SerializeField] DailyRewardConfig dailyRewardConfig;
    private XmlItemParser<DailyRewardConfig> _parser;

    private void TakeReward()
    {
        float reward = dailyRewardConfig.PrizeCount;
        GlobalManager.Instance.Coins += reward;

        rewardText.SetValue(reward);

        coinsText.SetValue(GlobalManager.Instance.Coins);
    }

    public void TryTakeReward()
    {
        if (GetTimeLeft() == TimeSpan.Zero)
        {
            OnBeforeDailyRewardGot.Invoke();
            TakeReward();
            PrizeTime = DateTime.Now + dailyRewardConfig.RefreshRewardTime;
            Refresh();
        }
    }

    public TimeSpan GetTimeLeft()
    {
        TimeSpan timeLeft = PrizeTime - DateTime.Now;

        if (timeLeft.TotalSeconds <= 0)
            return TimeSpan.Zero;

        return timeLeft;
    }
    public void Refresh()
    {
        TimeSpan timeLeft = GetTimeLeft();

        if (timeLeft != TimeSpan.Zero)
        {
            rewardTimeText.format = waitRewardFormat;
            rewardTimeText.SetValues(timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
        }
        else
        {
            rewardTimeText.format = readyRewardFormat;
            rewardTimeText.Refresh();
        }
    }
    private void OnEnable()
    {
        if (GlobalManager.Instance == null)
        {
            SceneManager.LoadScene(0);
            return;
        }

        _parser = new XmlItemParser<DailyRewardConfig>("General/dailyReward.xml", dailyRewardConfig);
        _parser.Load();
        dailyRewardConfig = _parser.Value;

        coinsText.SetValue(GlobalManager.Instance.Coins);
        StartCoroutine(UpdateTimeLeftRoutine());
    }
    private void OnDisable()
    {
        _parser.Save();
    }

    private IEnumerator UpdateTimeLeftRoutine()
    {
        while (true)
        {
            Refresh();
            yield return new WaitForSeconds(1);
        }
    }
}
