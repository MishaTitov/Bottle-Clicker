using TMPro;
using UnityEngine;

public class PopupInfoWindow : MonoBehaviour
{
    public static PopupInfoWindow instance;
    [SerializeField] TextMeshProUGUI infoTMP;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void SetInfoTMP(string info)
    {
        infoTMP.text = info;
    }
}
