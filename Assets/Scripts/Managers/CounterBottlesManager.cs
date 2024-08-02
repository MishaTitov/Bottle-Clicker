using System.Numerics;
using TMPro;
using UnityEngine;
using BCUtils;
using System.Collections;

public class CounterBottlesManager : MonoBehaviour
{
    public BigInteger[] arrayBE = new BigInteger[20];
    [SerializeField] TextMeshProUGUI amountBottlesTMP;
    [SerializeField] TextMeshProUGUI bottlesPerSecondTMP;

    [SerializeField] int tempBuffBpS;
    [SerializeField] int tempBuffBpC;
    public int percentUpgradeBpC { get; private set; }
    // SETTER and GETTERS
    public BigInteger multiplierBpS { get; private set; }
    public BigInteger amountBottlesBI { get; private set; } = 0;
    public BigInteger amountBottlesAllOfTimeBI { get; private set; } = 0;
    public BigInteger bottlesPerClickBI;
    public BigInteger bottlesPerSecondBI;
    //public int amountBottles { get; private set; }
    //public int bottlesPerClick;
    //public int bottlesPerSecond;

    private void Awake()
    {
        //LOADS
        amountBottlesAllOfTimeBI = BigInteger.Parse(PlayerPrefs.GetString("AmountBottlesAllOfTime", "0"));
        amountBottlesBI = BigInteger.Parse(PlayerPrefs.GetString("CurAmountBottles", "0"));
        bottlesPerClickBI = BigInteger.Parse(PlayerPrefs.GetString("BpC", "1"));
        bottlesPerSecondBI = BigInteger.Parse(PlayerPrefs.GetString("BpS", "0"));
        percentUpgradeBpC = PlayerPrefs.GetInt("percentUpgradeBpC", 0);
        multiplierBpS = BigInteger.Parse(PlayerPrefs.GetString("multiplierBpS", "0"));
    }

    private void Start()
    {
        InvokeRepeating(nameof(AddToAmountBottlesBpS), 0f, 1f);
        SaveManager.instance.saveEvent.AddListener(SaveCBM);
    }

    private void Update()
    {
        UpdateTMPs();
    }

    void UpdateTMPs()
    {
        //amountBottlesTMP.text = amountBottlesBI.ToString() + " bottles";
        amountBottlesTMP.text = BCPrinter.FormatBigInteger(amountBottlesBI, false) + " bottles";
        //bottlesPerSecondTMP.text = bottlesPerSecondBI.ToString() + " BpS";
        bottlesPerSecondTMP.text = BCPrinter.FormatBigInteger(CalculateBpS(), true) + " BpS";
    }
        
    public void AddToAmountBottles(BigInteger amountToAddBI)
    {
        //amountBottles += amountToAdd;
        amountBottlesBI += amountToAddBI;
        amountBottlesAllOfTimeBI += amountToAddBI;
    }

    public void ReduceAmountBottles(BigInteger valueBI)
    {
        //amountBottles -= value;
        amountBottlesBI -= valueBI;
    }

    //REFACTOR
    public BigInteger CalculateBpC()
    {
        BigInteger amountToAdd = (percentUpgradeBpC * CalculateBpS()) / 100;
        amountToAdd += bottlesPerClickBI;
        return amountToAdd * tempBuffBpC;
    }

    public BigInteger CalculateBpS()
    {
        return (multiplierBpS * bottlesPerSecondBI / 100 + bottlesPerSecondBI) * tempBuffBpS;
    }

    void AddToAmountBottlesBpS()
    {
        amountBottlesBI += CalculateBpS();
        amountBottlesAllOfTimeBI += CalculateBpS();
    }

    public void AddToAmountBottlesByPerClick()
    {        
        AddToAmountBottles(CalculateBpC());
    }
    
    public void IncreaseBpCByBuilding()
    {
        bottlesPerClickBI *= 2;
    }
    
    public void IncreaseBpCByUpgrade(int percentToAdd)
    {
        percentUpgradeBpC += percentToAdd;
    }

    public void IncreaseBpSByBuilding(int index, BigInteger valueToAddBI)
    {
        arrayBE[index] += valueToAddBI;
        bottlesPerSecondBI += valueToAddBI;
    }
    
    public void IncreaseBpSByUpgrade(BigInteger percentageToAdd)
    {
        multiplierBpS += percentageToAdd;
    }

    public void SetTempBuffBpC(int value, int duration)
    {
        StartCoroutine(StartTempBuffBpC(value, duration));
    }

    IEnumerator StartTempBuffBpC(int value, int duration)
    {
        tempBuffBpC = value;
        float curTime = 0.0f;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            yield return null;
        }
        tempBuffBpC = 1;
    }

    public void SetTempBuffBpS(int value, int duration)
    {
        StartCoroutine(StartTempBuffBpS(value, duration));
    }

    IEnumerator StartTempBuffBpS(int value, int duration)
    {
        tempBuffBpS = value;
        float curTime = 0.0f;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            yield return null;
        }
        tempBuffBpS = 1;
    }

    private void OnApplicationQuit()
    {
        SaveCBM();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveCBM();
    }

    private void SaveCBM()
    {
        PlayerPrefs.SetString("BpC", bottlesPerClickBI.ToString());
        PlayerPrefs.SetString("BpS", bottlesPerSecondBI.ToString());
        PlayerPrefs.SetString("CurAmountBottles", amountBottlesBI.ToString());
        PlayerPrefs.SetString("AmountBottlesAllOfTime", amountBottlesAllOfTimeBI.ToString());
        PlayerPrefs.SetInt("percentUpgradeBpC", percentUpgradeBpC);
        PlayerPrefs.SetString("multiplierBpS", multiplierBpS.ToString());
    }
}
