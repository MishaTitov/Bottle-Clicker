using UnityEngine;
using BCUtils;
using System.Numerics;

public class UpgradeBE : UpgradeEntity
{
    [SerializeField] BuildingEntity buildingEntity;
    [SerializeField] string costUEString;
    [SerializeField] int threshold;
    [SerializeField] int upgradeMultiplier;

    private void Awake()
    {
        costUE = BigInteger.Parse(PlayerPrefs.GetString($"CostUBE{buildingEntity.name}", costUEString));
        threshold = PlayerPrefs.GetInt($"threshold{buildingEntity.name}", threshold);
        upgradeMultiplier = PlayerPrefs.GetInt($"upgradeMultiplier{buildingEntity.nameBulding}", 2);
    }

    private void Start()
    {
        //costUE = BigInteger.Parse(costUEString);
        UpdateTMPs();

        SaveManager.instance.saveEvent.AddListener(SaveUBE);
    }

    public override void BuyUpgradeEntity()
    {
        IsAvailableToBuy();
        if (isAvailable)
        {
            AudioManager.instance.PlayButtonSFXByIndex(0);
            counterBottlesManager.ReduceAmountBottles(costUE);
            threshold += 50;
            MakeUpgrade();
        }
    }

    protected override void MakeUpgrade()
    {
        buildingEntity.UpgradeBpS(upgradeMultiplier);
        upgradeMultiplier += 1;
        //counterBottlesManager.IncreaseBpSByUpgrade(valueUE);
        base.MakeUpgrade();
        UpdateTMPs();
    }

    //REFACTOR Threshold
    protected override void IsAvailableToBuy()
    {
        if (buildingEntity.amountBuildings == 0) return;

        isAvailable = counterBottlesManager.amountBottlesBI >= costUE && buildingEntity.amountBuildings >= threshold;
        availableBackground.enabled = !isAvailable;
    }

    protected override void UpdateTMPs()
    {
        costUpgradeTMP.text = BCPrinter.FormatBigInteger(costUE, true);
        descriptionUpgradeTMP.text = $"x{upgradeMultiplier} to BpS of {buildingEntity.nameBulding}";
    }

    public void InfoButton()
    {
        PopupInfoWindow.instance.SetInfoTMP("> To unlock you need to owe " + threshold + " " + buildingEntity.nameBulding);
    }

    private void OnApplicationQuit()
    {
        SaveUBE();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveUBE();
    }

    private void SaveUBE()
    {
        PlayerPrefs.SetString($"CostUBE{buildingEntity.name}", costUE.ToString());
        PlayerPrefs.SetInt($"threshold{buildingEntity.name}", threshold);
        PlayerPrefs.SetInt($"upgradeMultiplier{buildingEntity.nameBulding}", upgradeMultiplier);
    }
}
