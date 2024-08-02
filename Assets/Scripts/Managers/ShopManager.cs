using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] BuildingEntity[] arrayBE;
    public int amountBuildingsToBuy { get; private set; } = 1;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void BuyNBuildingsByIndex(int indexBuildingEntity)
    {
        arrayBE[indexBuildingEntity].BuyBuilding();
    }

    //TODO per AMOUNT
    public void SetAmountBuildingsToBuy(int num)
    {
        amountBuildingsToBuy = num;
        for (int i = 0; i < arrayBE.Length; i++)
        {
            arrayBE[i].UpdateTMPs();
            arrayBE[i].IsAvailableToBuy();
        }
    }
}
