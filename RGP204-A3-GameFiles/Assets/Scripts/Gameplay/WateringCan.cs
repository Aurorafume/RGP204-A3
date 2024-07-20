using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public void WaterPlant(Plant plant)
    {
        Debug.Log("Watering can used to water plant.");
        plant.WaterPlant();
    }
}
