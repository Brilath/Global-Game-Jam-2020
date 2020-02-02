using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tapeCount;
    [SerializeField] private TextMeshProUGUI cakeCount;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        TapeHolder.OnTapeAmountChange += HandleTapeAmountChange;
        CakeHolder.OnCakeAmountChange += HandleCakeAmountChange;
    }
    private void OnDisable()
    {
        TapeHolder.OnTapeAmountChange -= HandleTapeAmountChange;
        CakeHolder.OnCakeAmountChange -= HandleCakeAmountChange;
    }

    private void HandleTapeAmountChange(int amount)
    {
        tapeCount.text = amount.ToString();
    }

    private void HandleCakeAmountChange(int amount)
    {
        Debug.Log($"Cake amount charges {amount}");
        cakeCount.text = amount.ToString();
    }
}
