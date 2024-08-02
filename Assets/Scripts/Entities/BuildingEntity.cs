using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BCUtils;
using System;
using System.IO;

public class BuildingEntity : MonoBehaviour
{
    [SerializeField] CounterBottlesManager counterBottlesManager;
    public string nameBulding;
    public int amountBuildings { get; private set; }
    [SerializeField] int multiplierBE;
    [SerializeField] int index;
    [SerializeField] TextMeshProUGUI amountBuildingsTMP;
    [SerializeField] TextMeshProUGUI costPerAmountBuildingsTMP;
    [SerializeField] TextAsset infoFullTXT;
    //TODO BigInteger
    [SerializeField] int discountOnCost;
    [SerializeField] string costOneBuildingSTR;
    BigInteger costOneBuildingBI;
    [SerializeField] string costTenBuildingsSTR;
    BigInteger costTenBuildingsBI;
    //[SerializeField] string costOneBuildingAfterTenSTR;
    BigInteger costOneBuildingAfterTenBI;
    [SerializeField] string costHundredBuildingsSTR;
    BigInteger costHundredBuildingsBI;
    //[SerializeField] string costOneBuildingAfterHundredSTR;
    BigInteger costOneBuildingAfterHundredBI;
    [SerializeField] int percentIncreaseCostBuilding;
    [SerializeField] BigInteger factorPowerTen = 404;
    [SerializeField] BigInteger summuryTenFactor = 2026;
    [SerializeField] BigInteger factorPowerHundred = 117431345;
    [SerializeField] BigInteger summuryHundredFactor = 782874966;
    
    [SerializeField] int bottlesPerSecond;
    public BigInteger bottlesPerSecondBI { get; private set; }
    [SerializeField] bool isAvailable;
    [SerializeField] Image availableBackground;
    string infoFullText;

    private void OnEnable()
    {
        InvokeRepeating(nameof(IsAvailableToBuy),0f,1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(IsAvailableToBuy));
    }

    private void Awake()
    {
        amountBuildings = PlayerPrefs.GetInt($"{nameBulding}AmountBuildings", amountBuildings);
        costOneBuildingBI = BigInteger.Parse(PlayerPrefs.GetString($"CostOneBuildingBI{name}", costOneBuildingSTR));
        bottlesPerSecondBI = BigInteger.Parse(PlayerPrefs.GetString($"bottlesPerSecondBI{name}", bottlesPerSecond.ToString()));
    }

    private void Start()
    {
        DUMPCalculateCostsOfBuildings();
        UpdateSTRs();
        UpdateTMPs();

        SaveManager.instance.saveEvent.AddListener(SaveBE);
    }

    private void UpdateSTRs()
    {
        costOneBuildingSTR = BCPrinter.FormatBigInteger(costOneBuildingBI, true);
        costTenBuildingsSTR = BCPrinter.FormatBigInteger(costTenBuildingsBI, true);
        costHundredBuildingsSTR = BCPrinter.FormatBigInteger(costHundredBuildingsBI, true);
    }

    private void GetFullText()
    {
        //infoFullText = File.ReadAllText("Assets/Art/Text/InfoText.txt");
        infoFullText = infoFullTXT.text;
        infoFullText = String.Format(infoFullText,
                                    nameBulding,
                                    BCPrinter.FormatBigInteger(bottlesPerSecondBI,false),
                                    amountBuildings,
                                    nameBulding,
                                    BCPrinter.FormatBigInteger(bottlesPerSecondBI * amountBuildings, false)
                                    );
    }

    public void InfoButton()
    {
        GetFullText();
        PopupInfoWindow.instance.SetInfoTMP(infoFullText);
    }

    //MAYBE Refactor
    public void UpdateBpS(int factor)
    {
        //bottlesPerSecond *= factor;
        bottlesPerSecondBI *= factor;
    }

    public void UpgradeBpS(int upgradeMultiplier)
    {
        //bottlesPerSecond *= 2;
        multiplierBE *= upgradeMultiplier;
        counterBottlesManager.IncreaseBpSByBuilding(index, (upgradeMultiplier - 1) * amountBuildings * bottlesPerSecondBI);
        bottlesPerSecondBI *= upgradeMultiplier;

    }

    public void BuyBuilding()
    {
        IsAvailableToBuy();
        if (isAvailable)
        {
            AudioManager.instance.PlayButtonSFXByIndex(0);
            int amountToBuy = ShopManager.instance.amountBuildingsToBuy;
            if (amountToBuy == 10)
            {
                counterBottlesManager.ReduceAmountBottles(costTenBuildingsBI);
                costOneBuildingBI = costOneBuildingAfterTenBI;
            }
            else if (amountToBuy == 100)
            {
                counterBottlesManager.ReduceAmountBottles(costHundredBuildingsBI);
                costOneBuildingBI = costOneBuildingAfterHundredBI;
            }
            else
            {
                counterBottlesManager.ReduceAmountBottles(costOneBuildingBI);
                costOneBuildingBI += (costOneBuildingBI * percentIncreaseCostBuilding / 100);
            }

            amountBuildings += amountToBuy;
            amountBuildingsTMP.text = amountBuildings.ToString() + " " + nameBulding;
            //GameManager.instance.IncreasetBottlesPerSecond(amountToBuy * bottlesPerSecond);
            counterBottlesManager.IncreaseBpSByBuilding(index, amountToBuy * bottlesPerSecondBI);

            //CalculateCostsOfBuildings();
            StatsManager.instance.AddPurchasedBuildeingEntities(amountToBuy);
            DUMPCalculateCostsOfBuildings();
            UpdateTMPs();
            IsAvailableToBuy();
        }
    }

    public void IsAvailableToBuy()
    {
        int amountToBuy = ShopManager.instance.amountBuildingsToBuy;
        if (amountToBuy == 10)
        {
            //isAvailable = GameManager.instance.amountBottles >= costTenBuilding;
            isAvailable = counterBottlesManager.amountBottlesBI >= costTenBuildingsBI;
        }
        else if (amountToBuy == 100)
        {
            //isAvailable = GameManager.instance.amountBottles >= costHundredBuilding;
            isAvailable = counterBottlesManager.amountBottlesBI >= costHundredBuildingsBI;
        }
        else
        {
            //isAvailable = GameManager.instance.amountBottles >= costOneBuilding;
            isAvailable = counterBottlesManager.amountBottlesBI >= costOneBuildingBI;
        }
        
        availableBackground.enabled = !isAvailable;
    }

    public void UpdateTMPs()
    {
        int amountToBuy = ShopManager.instance.amountBuildingsToBuy;
        if (amountToBuy == 10)
        {
            costPerAmountBuildingsTMP.text = $"X{amountToBuy} "+ costTenBuildingsSTR;
        }
        else if (amountToBuy == 100)
        {
            costPerAmountBuildingsTMP.text = $"X{amountToBuy} "+ costHundredBuildingsSTR;
        }
        else
        {
            costPerAmountBuildingsTMP.text = $"X{amountToBuy} "+ costOneBuildingSTR;
        }
        amountBuildingsTMP.text = amountBuildings.ToString() + " " + nameBulding;
    }

    public void CalculateCostsOfBuildings()
    {
        costTenBuildingsBI = (costOneBuildingBI * summuryTenFactor / 100);
        costOneBuildingAfterTenBI = (costOneBuildingBI * factorPowerTen / 100);
        costHundredBuildingsBI = (costOneBuildingBI * summuryHundredFactor / 100);
        costOneBuildingAfterHundredBI = (costOneBuildingBI * factorPowerHundred / 100);
        UpdateSTRs();
    }

    public void DUMPCalculateCostsOfBuildings()
    {
        BigInteger curPrice = costOneBuildingBI;
        BigInteger curSum = 0;
        for (int i = 0; i < 100; i++)
        {
            curSum += curPrice;
            curPrice += curPrice * percentIncreaseCostBuilding / 100;

            if (i == 9)
            {
                costTenBuildingsBI = curSum;
                costOneBuildingAfterTenBI = curPrice;
            }
        }

        costHundredBuildingsBI = curSum;
        costOneBuildingAfterHundredBI = curPrice;
        UpdateSTRs();
    }

    private void OnApplicationQuit()
    {
        SaveBE();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveBE();
    }

    private void SaveBE()
    {
        PlayerPrefs.SetInt($"{nameBulding}AmountBuildings", amountBuildings);
        PlayerPrefs.SetString($"CostOneBuildingBI{name}", costOneBuildingBI.ToString());
        PlayerPrefs.SetString($"bottlesPerSecondBI{name}", bottlesPerSecondBI.ToString());
    }
}
