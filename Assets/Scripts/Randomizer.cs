using BCUtils;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] CounterBottlesManager counterBottlesManager;
    [SerializeField] GameObject upgradeRandomizerGO;
    [SerializeField] TextMeshProUGUI descriptionUpgradeTMP;
    [SerializeField] TextMeshProUGUI costUpgradeTMP;
    //[SerializeField] protected TextMeshProUGUI descriptionUpgradeTMP;
    [SerializeField] Image availableBackground;
    [SerializeField] bool isAvailable;
    [SerializeField] BigInteger costRandomizer = new BigInteger(5000000000000);
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] GameObject randomizerUI;
    [SerializeField] Image imageRandom;
    [SerializeField] Image imageDuration;
    [SerializeField] Sprite[] spritesOfRandom;
    [SerializeField] [Range(0, 100)] int percentToAdd;
    [SerializeField] [Range(0, 100)] int percentToRemove;
    [SerializeField] int timerRandom;
    [SerializeField] int multiplierBuffBpC;
    [SerializeField] int durationBuffBpC;
    [SerializeField] int multiplierBuffBpSC;
    [SerializeField] int durationBuffBpSC;
    public int isPurchasedHourMultiplier { get; private set; }
    private int isPurchasedRandomizer;

    private void OnEnable()
    {
        if (isPurchasedHourMultiplier == 1) return;
        InvokeRepeating(nameof(IsAvailableToBuy), 0f, 1f);
    }

    private void OnDisable()
    {
        if (isPurchasedHourMultiplier == 1) return;
        CancelInvoke(nameof(IsAvailableToBuy));
    }

    private void Start()
    {
        CheckIsRandomizerPurchased();

        SaveManager.instance.saveEvent.AddListener(SaveRandomizer);

        if (isPurchasedHourMultiplier == 1)
        {
            upgradeRandomizerGO.SetActive(false);
            CancelInvoke(nameof(IsAvailableToBuy));
        }
    }

    private void CheckIsRandomizerPurchased()
    {
        isPurchasedRandomizer = PlayerPrefs.GetInt("randomizerIsPurchased", 0);
        isPurchasedHourMultiplier= PlayerPrefs.GetInt("hourMultiplierIsPurchased", 0);

        if (isPurchasedRandomizer == 1)
        {
            InvokeRepeating(nameof(pickRandom), timerRandom, timerRandom);
        }
        else if (isPurchasedHourMultiplier == 0)
        {
            //InvokeRepeating(nameof(pickRandom), 2.0f, timerRandom);
            costUpgradeTMP.text = BCPrinter.FormatBigInteger(costRandomizer, true);
        }
    }

    #region MainFuncs

    private void pickRandom()
    {
        float num = Random.Range(0.0f, 1.0f);
        Debug.Log(num);
        if (num < 0.2f)
        {
            FailRandom();
        }
        else if (0.2f <= num && num < 0.55f)
        {
            LuckRandom();
        }
        else if (0.55f <= num && num < 0.85f)
        {
            BuffBpSCRandom();
        }
        else
        {
            BuffBpCRandom();
        }
    }

    private void BuffBpSCRandom()
    {
        PlayClipByIndex(2);

        SetupPopupText($"+{multiplierBuffBpSC}% to BpS and BpC for {durationBuffBpSC} sec");
        counterBottlesManager.SetTempBuffBpS(multiplierBuffBpSC, durationBuffBpSC);
        counterBottlesManager.SetTempBuffBpC(multiplierBuffBpSC, durationBuffBpSC);

        randomizerUI.SetActive(true);
        imageRandom.sprite = spritesOfRandom[0];
        //imageDuration.sprite = spritesOfRandom[0];
        StartCoroutine(PlayDuration(durationBuffBpSC));
        //Invoke(nameof(counterBottlesManager.SetTempBuffBpS), durationBuffBpS);
        //GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
        //if (popupText != null)
        //{
        //    popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.zero,
        //                                                         $"+{percentBuffBpS}% to BpC for {durationBuffBpS} sec");
        //}
    }

    private void BuffBpCRandom()
    {
        PlayClipByIndex(3);

        SetupPopupText($"+{multiplierBuffBpC}% to BpC for {durationBuffBpC} sec");
        counterBottlesManager.SetTempBuffBpC(multiplierBuffBpC, durationBuffBpC);

        randomizerUI.SetActive(true);
        imageRandom.sprite = spritesOfRandom[1];
        //imageDuration.sprite = spritesOfRandom[1];
        StartCoroutine(PlayDuration(durationBuffBpC));
        //Invoke(nameof(counterBottlesManager.SetTempBuffBpC), durationBuffBpC);
        //GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
        //if (popupText != null)
        //{
        //    popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.zero,
        //                                                         $"+{percentBuffBpC}% to BpC for {durationBuffBpC} sec");
        //}
    }

    IEnumerator PlayDuration(int duration)
    {
        float normalizedTime = 0.0f;
        while (normalizedTime < 1)
        {
            imageDuration.fillAmount = normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        randomizerUI.SetActive(false);
    }

    private void FailRandom()
    {
        Debug.Log("Fail");

        //audioSource.clip = audioClips[1];
        //audioSource.Play();
        PlayClipByIndex(1);

        BigInteger value = percentToRemove * counterBottlesManager.amountBottlesBI / 100;
        counterBottlesManager.ReduceAmountBottles(value);
        SetupPopupText($"Fail\nYou broke {BCPrinter.FormatBigInteger(value, false)} Bottles");
        //GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
        //if (popupText != null)
        //{
        //    popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.zero,
        //                                                         $"Fail\nYou broke {BCPrinter.FormatBigInteger(value, false)} Bottles");
        //}
    }

    private void LuckRandom()
    {
        Debug.Log("Luck SFX");

        PlayClipByIndex(0);

        BigInteger value = percentToAdd * counterBottlesManager.amountBottlesBI / 100;
        counterBottlesManager.AddToAmountBottles(value);
        SetupPopupText($"LUCK\nYou get {BCPrinter.FormatBigInteger(value, false)} Bottles");
        //GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
        //if (popupText != null)
        //{
        //    popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.zero,
        //                                                         $"LUCK\nYou get {BCPrinter.FormatBigInteger(value, false)} Bottles");
        //}
    }

    private void PlayClipByIndex(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private void SetupPopupText(string messageToSend)
    {
        GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
        if (popupText != null)
        {
            popupText.GetComponent<PopupText>().SetupAchievement(UnityEngine.Vector3.down * 4.0f, messageToSend);
        }
    }
    #endregion

    #region Upgrade

    private void IsAvailableToBuy()
    {
        isAvailable = counterBottlesManager.amountBottlesBI >= costRandomizer;
        availableBackground.enabled = !isAvailable;
    }

    public void BuyRandomizer()
    {
        IsAvailableToBuy();
        if (isAvailable && isPurchasedRandomizer == 0)
        {
            AudioManager.instance.PlayButtonSFXByIndex(0);
            counterBottlesManager.ReduceAmountBottles(costRandomizer);
            InvokeRepeating(nameof(pickRandom), 2.0f, timerRandom);

            isPurchasedRandomizer = 1;
            costRandomizer *= 2;
            costUpgradeTMP.text = BCPrinter.FormatBigInteger(costRandomizer, true);
            descriptionUpgradeTMP.text = "Get Time Upgrade";
        }
        else if (isAvailable && isPurchasedRandomizer == 1)
        {
            AudioManager.instance.PlayButtonSFXByIndex(0);
            isPurchasedHourMultiplier = 1;
            StatsManager.instance.CheckHourMultiplier();

            CancelInvoke(nameof(IsAvailableToBuy));
            upgradeRandomizerGO.SetActive(false);
        }
    }

    public void InfoButton()
    {
        if (isPurchasedRandomizer == 0 )
            PopupInfoWindow.instance.SetInfoTMP("> Randomizer randomly drops the bottle effect");
        else if (isPurchasedRandomizer == 1)
            PopupInfoWindow.instance.SetInfoTMP("> Randomizer gives +N% BpS to BpS and BpC for each Nth hour in game");
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveRandomizer();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveRandomizer();
    }

    private void SaveRandomizer()
    {
        PlayerPrefs.SetInt("randomizerIsPurchased", isPurchasedRandomizer);
        PlayerPrefs.SetInt("hourMultiplierIsPurchased", isPurchasedHourMultiplier);
    }
}
