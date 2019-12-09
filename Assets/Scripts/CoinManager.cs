using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int coinCount;

    public static CoinManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        text.text = coinCount.ToString();
    }

    public void Refresh(int count)
    {
        coinCount += count;
        text.text = coinCount.ToString();
    }
}
