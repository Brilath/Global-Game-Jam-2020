using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeHolder : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private int maxCharges;
    [SerializeField] private int _charges;

    static public event Action<int> OnTapeAmountChange = delegate { };

    public int Charges { get { return _charges;  }
        private set { _charges = Mathf.Clamp(value, 0, maxCharges );  }
    } 
    
    public void UseTape()
    {
        Charges--;
        OnTapeAmountChange(Charges);
    }

    public void CollectTape(int amount = 1)
    {
        Debug.Log($"Increases tape charges by {amount}");
        Charges += amount;
        OnTapeAmountChange(Charges);
    }
}
