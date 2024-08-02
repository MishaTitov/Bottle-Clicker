using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] CounterBottlesManager counterBottlesManager;
    [SerializeField] int threshold;
    [SerializeField] int stepThreshold;
    [SerializeField] int multiplierToAdd;
    public int totalAchievements { get; private set; }
    public int unlockedAchievements { get; private set; }

    public void ReportStartAchievementStatus(bool isLocked)
    {
        totalAchievements ++;
        if(!isLocked) unlockedAchievements++;
        //ReportUnlockAchievementStatus(isLocked);
    }

    public void ReportUnlockAchievementStatus(bool isLocked)
    {
        if (!isLocked)
        {
            unlockedAchievements++;
            if (unlockedAchievements >= threshold)
            {
                counterBottlesManager.IncreaseBpSByUpgrade(multiplierToAdd);
                counterBottlesManager.IncreaseBpCByUpgrade(multiplierToAdd);
                GameObject popupText = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
                if (popupText != null)
                {
                    popupText.GetComponent<PopupText>().SetupAchievement(Vector3.zero,
                                                                         $"You get {threshold} unlocked achievements\nYou get {multiplierToAdd}% to BpS\nYou get {multiplierToAdd}% to BpC");
                }

                threshold += stepThreshold;
                multiplierToAdd += 1;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        multiplierToAdd = PlayerPrefs.GetInt("achievementMultiplierToAdd", multiplierToAdd);
        threshold = PlayerPrefs.GetInt("achievementThreshold", threshold);

        SaveManager.instance.saveEvent.AddListener(SaveAM);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        SaveAM();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveAM();
    }

    private void SaveAM()
    {
        PlayerPrefs.SetInt("achievementThreshold", threshold);
        PlayerPrefs.SetInt("achievementMultiplierToAdd", multiplierToAdd);
    }
}
