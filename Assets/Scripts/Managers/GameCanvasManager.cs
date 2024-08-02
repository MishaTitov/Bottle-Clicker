using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject bottleGO;
    [SerializeField] GameObject[] screens;
    [SerializeField] Image mainBackground;
    [SerializeField] TextAsset howToPlayTXT;
    [SerializeField] int indexCurrentScreen;
    [SerializeField] float tempAlphaMainBackground;
    private Color tempColor;
    private float defaultAlphaMainBackground;

    private void Start()
    {
        tempColor = mainBackground.color;
        defaultAlphaMainBackground = tempColor.a;
    }

    public void ChangeScreen(int nextIndex)
    {
        tempColor.a = tempAlphaMainBackground;
        mainBackground.color = tempColor;
        screens[indexCurrentScreen].SetActive(false);
        bottleGO.SetActive(false);
        indexCurrentScreen = nextIndex;
        screens[indexCurrentScreen].SetActive(true);
    }

    public void DisableCurrentScreen()
    {
        tempColor.a = defaultAlphaMainBackground;
        mainBackground.color = tempColor;
        screens[indexCurrentScreen].SetActive(false);
        bottleGO.SetActive(true);
    }

    public void HowToPlayButton()
    {
        PopupInfoWindow.instance.SetInfoTMP(howToPlayTXT.text);
    }
}
