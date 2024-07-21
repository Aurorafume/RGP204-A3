using UnityEngine;

public class Shovel : MonoBehaviour
{
    // Dig out the plant from the pot
    public void DigPlant(Plant plant)
    {
        plant.RemovePlant();
    }
}
