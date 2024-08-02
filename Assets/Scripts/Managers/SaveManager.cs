using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public UnityEvent saveEvent;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("InvokeSaveEvent", 120.0f, 120.0f);
    }

    private void InvokeSaveEvent()
    {
        if (saveEvent != null)
        {
            saveEvent.Invoke();
            GameObject popupTextGO = ObjectPoolManager.instance.GetPooledPopupTextAchivementGO();
            if (popupTextGO != null)
                popupTextGO.GetComponent<PopupText>().SetupAchievement(Vector3.down * 2.0f,
                "Game Saved");
        }
    }

    public void QuitButton()
    {
        Debug.Log("GAME SAVE Before quit");
        Application.Quit();
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void RestartButton()
    {
        DeletePlayerPrefs();
        //PlayerPrefs.SetString("AmountBottlesAllOfTime", counterBottlesManager.amountBottlesAllOfTimeBI.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
