using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public int money;

    public void EarnMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
}
