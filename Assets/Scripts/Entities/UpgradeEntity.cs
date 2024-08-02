using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntity : MonoBehaviour
{
    [SerializeField] protected CounterBottlesManager counterBottlesManager;
    [SerializeField] protected TextMeshProUGUI costUpgradeTMP;
    [SerializeField] protected TextMeshProUGUI descriptionUpgradeTMP;
    [SerializeField] protected Image availableBackground;
    [SerializeField] protected bool isAvailable;
    protected BigInteger valueUE;
    protected BigInteger costUE;
    //MAYBE contidition

    private void OnEnable()
    {
        InvokeRepeating(nameof(IsAvailableToBuy), 0f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(IsAvailableToBuy));
    }

    protected virtual void MakeUpgrade()
    {
        //Debug.Log("DID UPGRADE UE");
        costUE *= 100;
        UpdateCostUpgradeTMP();
    }

    protected virtual void UpdateCostUpgradeTMP()
    {
        costUpgradeTMP.text = costUE.ToString();
    }

    protected virtual void IsAvailableToBuy()
    {
        isAvailable = counterBottlesManager.amountBottlesBI >= costUE;
        availableBackground.enabled = !isAvailable;
    }

    public virtual void BuyUpgradeEntity()
    {
        IsAvailableToBuy();
        if (isAvailable)
        {
            counterBottlesManager.ReduceAmountBottles(costUE);
            MakeUpgrade();
            //TODO UpdateCostAndValue
            //gameObject.SetActive(false);
        }
    }

    protected virtual void UpdateTMPs()
    {
        Debug.Log("DID UPDATE TMPs");
    }
}
