using System.Numerics;
using TMPro;
using UnityEngine;
using BCUtils;
using System;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;
    public BigInteger clicksOnBottleBI { get; private set; }
    public int purchasedBuildingEntities { get; private set; }
    [SerializeField] CounterBottlesManager counterBottlesManager;
    [SerializeField] Randomizer randomizer;
    [SerializeField] AchievementManager achievementManager;
    //[SerializeField] TextMeshProUGUI clicksOnBottleTMP;
    //[SerializeField] TextMeshProUGUI purchasedBuildingEntitiesTMP;
    [SerializeField] TextAsset statsFullTXT;
    [SerializeField] TextMeshProUGUI statsFullTMP;
    [SerializeField] float runStartedTime;
    [SerializeField] int prevHour;
    string statsFullText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        GetFullText();

        runStartedTime = PlayerPrefs.GetInt("runStartedTime", 0);
        purchasedBuildingEntities = PlayerPrefs.GetInt("purchasedBuildingEntities", 0);
        clicksOnBottleBI = BigInteger.Parse(PlayerPrefs.GetString("clicksOnBottleBI","0"));
        prevHour = PlayerPrefs.GetInt("prevHour", 0);
    }

    private void Start()
    {
        SaveManager.instance.saveEvent.AddListener(SaveStatsM);
    }

    private void Update()
    {
        UpdateFullText();
    }

    private void FixedUpdate()
    {
        runStartedTime += Time.deltaTime;
        if (randomizer.isPurchasedHourMultiplier == 1)
            CheckHourMultiplier();
    }

    public void CheckHourMultiplier()
    {
        int curHour = (int) TimeSpan.FromSeconds(runStartedTime).TotalHours;
        if (curHour > prevHour)
        {
            prevHour = curHour;
            counterBottlesManager.IncreaseBpSByUpgrade(prevHour);
            counterBottlesManager.IncreaseBpCByUpgrade(prevHour);

            GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
            if (popupText != null)
            {
                popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.down * 4.0f, $"{curHour} hours in game\n+{prevHour}% BpS to BpS\n+{prevHour}% BpS to BpC");
            }
        }
    }

    public void AddClick()
    {
        clicksOnBottleBI += 1;
        UpdateFullText();
    }

    public void AddPurchasedBuildeingEntities(int value)
    {
        purchasedBuildingEntities += value;
        UpdateFullText();
    }

    public void GetFullText()
    {
        statsFullText = statsFullTXT.text;
        UpdateFullText();
    }

    private void UpdateFullText()
    {
        TimeSpan timeTmp = TimeSpan.FromSeconds(runStartedTime);
        statsFullTMP.text = String.Format(statsFullText,
            BCPrinter.FormatBigInteger(counterBottlesManager.amountBottlesBI, false),
            BCPrinter.FormatBigInteger(counterBottlesManager.amountBottlesAllOfTimeBI, false),
            (int)timeTmp.TotalHours + timeTmp.ToString(@"\:mm\:ss"),
            purchasedBuildingEntities,
            BCPrinter.FormatBigInteger(counterBottlesManager.CalculateBpS(), false),
            BCPrinter.FormatBigInteger(counterBottlesManager.multiplierBpS, false),
            BCPrinter.FormatBigInteger(counterBottlesManager.CalculateBpC(), false),
            counterBottlesManager.percentUpgradeBpC,
            BCPrinter.FormatBigInteger(clicksOnBottleBI, false),
            achievementManager.unlockedAchievements,
            achievementManager.totalAchievements);
    }

    private void OnApplicationQuit()
    {
        SaveStatsM();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveStatsM();
    }

    private void SaveStatsM()
    {
        PlayerPrefs.SetInt("runStartedTime", Mathf.FloorToInt(runStartedTime));
        PlayerPrefs.SetInt("purchasedBuildingEntities", purchasedBuildingEntities);
        PlayerPrefs.SetString("clicksOnBottleBI", clicksOnBottleBI.ToString());
        PlayerPrefs.SetInt("prevHour", prevHour);
    }
}
