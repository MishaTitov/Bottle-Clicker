using BCUtils;
using UnityEngine;
using UnityEngine.UI;

public class AchievementEntity : MonoBehaviour
{
    [SerializeField] BuildingEntity buildingEntity;
    [SerializeField] AchievementManager achievementManager;
    //[SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Image imageElement;
    [SerializeField] Sprite lockSprite;
    [SerializeField] Sprite achievementSprite;
    [SerializeField] int threshold;
    [SerializeField] public bool isLocked { get; private set; }

    public void LockAchievement()
    {
        //spriteRenderer.sprite= lockSprite;
        imageElement.sprite= lockSprite;
    }

    public void UnlockAchievement()
    {
        //spriteRenderer.sprite = achievementSprite;
        imageElement.sprite= achievementSprite;
        AudioManager.instance.PlayButtonSFXByIndex(2);
    }

    private void Awake()
    {
        isLocked = 1 == PlayerPrefs.GetInt($"{buildingEntity.nameBulding}Achievement{threshold}", 1);
    }

    // Start is called before the first frame update
    void Start()
    {      

        achievementManager.ReportStartAchievementStatus(isLocked);
        if (isLocked)
            LockAchievement();
        else
            //UnlockAchievement();
            imageElement.sprite = achievementSprite;

        SaveManager.instance.saveEvent.AddListener(SaveAE);
    }

    // Update is called once per frame
    void Update()
    {
        // TODEL for test
        if (isLocked && buildingEntity != null) {
            if (buildingEntity.amountBuildings >= threshold)
            {
                UnlockAchievement();
                isLocked = false;
                achievementManager.ReportUnlockAchievementStatus(isLocked);
                GameObject popupTextGO = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
                if (popupTextGO != null)
                    popupTextGO.GetComponent<PopupText>().SetupAchievement(Vector3.down * 2.0f,
                    "Achievement Unlocked\n" + threshold + " " + buildingEntity.nameBulding);
            }
            else
            {
                LockAchievement();
            }
        }
    }

    public void InfoButton()
    {
        if (isLocked)
        {
            PopupInfoWindow.instance.gameObject.SetActive(false);
            return;
        }

        PopupInfoWindow.instance.SetInfoTMP("> Have " + threshold + " " + buildingEntity.nameBulding);
    }

    private void OnApplicationQuit()
    {
        SaveAE();
    }

    private void OnApplicationPause()
    {
        if (Time.timeSinceLevelLoad > 30.0f)
            SaveAE();
    }

    public void SaveAE()
    {
        PlayerPrefs.SetInt($"{buildingEntity.nameBulding}Achievement{threshold}", isLocked? 1:0);
    }
}