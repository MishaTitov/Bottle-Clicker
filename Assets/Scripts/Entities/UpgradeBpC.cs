using System.Numerics;
using BCUtils;
using UnityEngine;

public class UpgradeBpC : UpgradeEntity
{
    private BigInteger requiredCountClicks;
    int counterUpgrades;

    private void Awake()
    {
        costUE = BigInteger.Parse(PlayerPrefs.GetString("CostUEBpC", "50000"));
        requiredCountClicks = BigInteger.Parse(PlayerPrefs.GetString("RequiredCountClicks", "1000"));
        valueUE = BigInteger.Parse(PlayerPrefs.GetString("ValueUEBpC", "1"));
        counterUpgrades = PlayerPrefs.GetInt("CounterUpgradesBpC", 0);
    }

    void Start()
    {
        //requiredCountClicks = 1000;
        //valueUE = 1;
        //costUE = 50000;
        UpdateTMPs();

        SaveManager.instance.saveEvent.AddListener(SaveUBpC);
    }
    // REFACTOR add BuyUpgrade

    public override void BuyUpgradeEntity()
    {
        IsAvailableToBuy();
        if (isAvailable)
        {
            counterBottlesManager.ReduceAmountBottles(costUE);
            MakeUpgrade();
        }
    }

    protected override void MakeUpgrade()
    {
        AudioManager.instance.PlayButtonSFXByIndex(0);
        counterBottlesManager.IncreaseBpCByUpgrade((int)valueUE);
        valueUE += 1;
        base.MakeUpgrade();
        requiredCountClicks *= 2;
        UpdateTMPs();
    }

    protected override void IsAvailableToBuy()
    {
        //TODO CONDITION
        isAvailable = counterBottlesManager.amountBottlesBI >= costUE && StatsManager.instance.clicksOnBottleBI > requiredCountClicks;
        availableBackground.enabled = !isAvailable;
    }

    protected override void UpdateTMPs()
    {
        costUpgradeTMP.text = BCPrinter.FormatBigInteger(costUE, true);
        descriptionUpgradeTMP.text = $"+{valueUE}% BpS to BpC";
    }

    public void InfoButton()
    {
        PopupInfoWindow.instance.SetInfoTMP("> To unlock you need " + BCPrinter.FormatBigInteger(requiredCountClicks, false) + " clicks");
    }

    private void OnApplicationQuit()
    {
        SaveUBpC();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveUBpC();
    }

    private void SaveUBpC()
    {
        PlayerPrefs.SetString("CostUEBpC", costUE.ToString());
        PlayerPrefs.SetString("RequiredCountClicks", requiredCountClicks.ToString());
        PlayerPrefs.SetString("ValueUEBpC", valueUE.ToString());
        PlayerPrefs.SetInt("CounterUpgradesBpC", counterUpgrades);
    }
}
