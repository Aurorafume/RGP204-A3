using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public void WaterPlant(Plant plant)
    {
        if (plant.currentStage != Plant.GrowthStage.Withered)
        {
            // Logic to water the plant
        }
    }
}
