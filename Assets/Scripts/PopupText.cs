using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] TextMeshPro popupTextTMP;
    [SerializeField] TMP_FontAsset fontPopupTextAchiev;
    [SerializeField] Color popupTextAchievColor;
    [SerializeField] float moveYSpeed;
    [SerializeField] float disapperearSpeed;
    private float disapperearTime = 1f;
    private Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup(Vector3 newPosition, string text)
    {
        transform.position = newPosition;

        popupTextTMP.text = text;

        textColor = Color.white;
        textColor.a = 1f;
        popupTextTMP.color = textColor;

        disapperearTime = 1f;
    }

    public void SetupAchievement(Vector3 newPosition, string newText)
    {
        transform.position = newPosition;

        popupTextTMP.text = newText;

        textColor = popupTextAchievColor;
        textColor.a = 1f;
        popupTextTMP.color = popupTextAchievColor;

        //popupTextTMP.font = fontPopupTextAchiev;

        disapperearTime = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, moveYSpeed, 0f) * Time.deltaTime;

        if (disapperearTime < 0f)
        {
            textColor.a -= disapperearSpeed * Time.deltaTime;
            popupTextTMP.color = textColor;

            if (textColor.a < 0f)
            {
                gameObject.SetActive(false);
            }
        }
        else
            disapperearTime -= Time.deltaTime;

    }
}
