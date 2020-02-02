using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeHolder : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private int maxCharges;
    [SerializeField] private int _charges;
    [SerializeField] private GameObject _cakePrefab;

    static public event Action<int> OnCakeAmountChange = delegate { };

    public int Charges
    {
        get { return _charges; }
        private set { _charges = Mathf.Clamp(value, 0, maxCharges); }
    }

    public void UseCake()
    {
        Instantiate(_cakePrefab, transform.position, Quaternion.identity);
        
        Charges--;
        OnCakeAmountChange(Charges);
    }

    public void CollectCake(int amount = 1)
    {
        Debug.Log($"Increases tape charges by {amount}");
        Charges += amount;
        OnCakeAmountChange(Charges);
    }
}
