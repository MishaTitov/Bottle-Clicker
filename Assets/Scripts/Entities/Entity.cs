using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] protected CounterBottlesManager counterBottlesManager;
    [SerializeField] string nameEntity;
    [SerializeField] protected float value;
    [SerializeField] float discountOnCost;
    [SerializeField] protected int costEntity;

    [SerializeField] Image availableBackground;
    [SerializeField] protected bool isAvailable;

    protected virtual void IsAvailableToBuy()
    {
        Debug.Log("IsAvailableToBuy");
    }

    protected virtual void BuyEntity()
    {
        Debug.Log("Buy Entity");
    }
}
