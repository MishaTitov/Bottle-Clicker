using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance { get; private set; }
    [SerializeField] GameObject pooledPopuTextGO;
    [SerializeField] GameObject pooledPopuTextAchievementGO;
    [SerializeField] int pooledPopuTextAmount;
    [SerializeField] List<GameObject> pooledPopupTextGOs;
    [SerializeField] int pooledPopuTextAchievementAmount;
    [SerializeField] List<GameObject> pooledPopupTextAchievementGOs;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < pooledPopuTextAmount; ++i)
        {
            GameObject gameObject = Instantiate(pooledPopuTextGO);
            gameObject.SetActive(false);
            pooledPopupTextGOs.Add(gameObject);
            gameObject.transform.SetParent(transform);
        }

        for (int i = 0; i < pooledPopuTextAchievementAmount; ++i)
        {
            GameObject gameObject = Instantiate(pooledPopuTextAchievementGO);
            gameObject.SetActive(false);

            pooledPopupTextAchievementGOs.Add(gameObject);
            gameObject.transform.SetParent(transform);
        }
    }

    public GameObject GetPooledPopupTextGO()
    {
        for (int i = 0; i < pooledPopuTextAmount; ++i)
        {
            if (!pooledPopupTextGOs[i].activeInHierarchy)
            {
                pooledPopupTextGOs[i].SetActive(true);
                return pooledPopupTextGOs[i];
            }
        }

        return null;
    }

    public GameObject GetPooledPopupTextAchivementGO()
    {
        for (int i = 0; i < pooledPopuTextAchievementAmount; ++i)
        {
            if (!pooledPopupTextAchievementGOs[i].activeInHierarchy)
            {
                pooledPopupTextAchievementGOs[i].SetActive(true);
                return pooledPopupTextAchievementGOs[i];
            }
        }

        return null;
    }
}
