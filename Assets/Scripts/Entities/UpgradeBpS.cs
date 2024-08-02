using BCUtils;
using System.Numerics;
using UnityEngine;

public class UpgradeBpS : UpgradeEntity
{
    BigInteger conditionAmountBottlesAOTBI;

    private void Awake()
    {
        costUE = BigInteger.Parse(PlayerPrefs.GetString("CostUEBpS", "1000000"));
        conditionAmountBottlesAOTBI = BigInteger.Parse(PlayerPrefs.GetString("ConditionAmountBottlesAOTBI", "50000"));
        valueUE = BigInteger.Parse(PlayerPrefs.GetString("ValueUEBpS", "1"));
    }


    private void Start()
    {
        //valueUE = 1;
        //conditionAmountBottlesAOTBI = 50000;
        //costUE = 1000000;
        UpdateTMPs();

        SaveManager.instance.saveEvent.AddListener(SaveUBpS);
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
        counterBottlesManager.IncreaseBpSByUpgrade(valueUE);
        costUE *= 40;
        UpdateCostUpgradeTMP();

        valueUE += 1;

        conditionAmountBottlesAOTBI *= 40;
        UpdateTMPs();        
    }

    protected override void IsAvailableToBuy()
    {
        isAvailable = counterBottlesManager.amountBottlesBI >= costUE && counterBottlesManager.amountBottlesAllOfTimeBI >= conditionAmountBottlesAOTBI;
        availableBackground.enabled = !isAvailable;
    }

    protected override void UpdateTMPs()
    {
        costUpgradeTMP.text = BCPrinter.FormatBigInteger(costUE, true);
        descriptionUpgradeTMP.text = $"+{valueUE}% BpS to BpS";
    }

    public void InfoButton()
    {
        PopupInfoWindow.instance.SetInfoTMP("> To unlock you need " + BCPrinter.FormatBigInteger(conditionAmountBottlesAOTBI, false) + " bottles all of time");
    }

    private void OnApplicationQuit()
    {
        SaveUBpS();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveUBpS();
    }

    private void SaveUBpS()
    {
        PlayerPrefs.SetString("CostUEBpS", costUE.ToString());
        PlayerPrefs.SetString("ConditionAmountBottlesAOTBI", conditionAmountBottlesAOTBI.ToString());
        PlayerPrefs.SetString("ValueUEBpS", valueUE.ToString());
    }
}
