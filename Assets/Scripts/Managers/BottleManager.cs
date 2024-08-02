using TMPro;
using UnityEngine;
using BCUtils;
using UnityEngine.EventSystems;

public class BottleManager : MonoBehaviour, IClickable
{
    [SerializeField] CounterBottlesManager counterBottlesManager;
    //[SerializeField] GameObject popupTextGO;
    //MAYBE REFACTOR switch by SET VALUES
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem bottlePS;
    [SerializeField] float idleTime;

    void Start()
    {
        addPhysics2DRaycaster();
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    public void IsClicked()
    {
        GetTimeCurrentAnimation();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        animator.Play("IsClicked");
        PlaySFX();
        //PlayPS();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //GameManager.instance.IncreaseAmountBottlesByClick();
        counterBottlesManager.AddToAmountBottlesByPerClick();
        StatsManager.instance.AddClick();

        //CREATE POOL OF POPUPs
        //popupTextGO.SetActive(true);
        GameObject popupTextGO = ObjectPoolManager.instance.GetPooledPopupTextGO();
        if (popupTextGO != null)
            popupTextGO.GetComponent<PopupText>().Setup(eventData.pointerPressRaycast.worldPosition,
                                                    "+" + BCPrinter.FormatBigInteger(counterBottlesManager.CalculateBpC(), false));
        
        IsClicked();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bottlePS.Stop();
        animator.Play("Idle", 0, idleTime);

    }

    private void GetTimeCurrentAnimation()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] curAnimatorClip = animator.GetCurrentAnimatorClipInfo(0);
        idleTime = curAnimatorClip[0].clip.length * animationState.normalizedTime;
    }

    void PlaySFX()
    {
        audioSource.Play();
    }

    void PlayPS()
    {
        bottlePS.Play();
    }


}
