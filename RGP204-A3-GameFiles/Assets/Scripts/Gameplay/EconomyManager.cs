using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // Initial balance
    public int balance = 25;
    public GameObject textObject;

    // Start is called before the first frame update
    void Start()
    {
        // Nothing to initialise in Start
    }

    // Update is called once per frame
    void Update()
    {
        // Update the displayed balance
        textObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + balance;
    }

    // Check if the player can afford an item
    public bool CanAfford(int cost)
    {
        return balance >= cost;
    }

    // Add money to the balance
    public void AddMoney(int amount)
    {
        balance += amount;
    }

    // Subtract money from the balance
    public void SubtractMoney(int amount)
    {
        balance -= amount;
    }

    // Set the balance to a specific amount
    public void SetMoney(int amount)
    {
        balance = amount;
    }

    // Get the current balance
    public int GetMoney()
    {
        return balance;
    }

    // Reset the balance to zero
    public void ResetMoney()
    {
        balance = 0;
    }
}
