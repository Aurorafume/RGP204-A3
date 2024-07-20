using UnityEngine;

public class Shovel : MonoBehaviour
{
    public void DigPlant(Plant plant)
    {
        Debug.Log("Shovel used to dig plant.");
        plant.RemovePlant();
    }
}
