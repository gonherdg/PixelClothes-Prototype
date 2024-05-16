using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] protected int _gold = 1000;
    public int Gold => _gold;

    private void Start()
    {
        _gold = 1000;
    }

    public bool SpendMoney(int quantity)
    {
        if (_gold >= quantity)
        {
            _gold -= quantity;
        } else
        {
            Debug.Log("Not enough gold for that");
            return true;
        }
        
        return false; // If no error return false
    }
}
