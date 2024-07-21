using UnityEngine;

public class VinePot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the collider is a seed
        if (other.CompareTag("Seed"))
        {
            Plant plant = other.GetComponent<Plant>();
            if (plant != null && plant.seedType == SeedType.None)
            {
                plant.seedType = SeedType.Vine; // Assign the seed type
                AssignRandomSeedling(plant);
                SnapToPot(other.transform);
                Destroy(other.gameObject); // Remove the seed item after it has been placed
            }
            else
            {
                Destroy(other.gameObject); // Remove incorrect seed type
            }
        }
    }

    // Snap the seed to the top of the pot's collider
    private void SnapToPot(Transform seedTransform)
    {
        BoxCollider2D potCollider = GetComponent<BoxCollider2D>();
        float offsetY = potCollider.bounds.size.y / 2; // Calculate the offset to position at the top
        seedTransform.position = new Vector3(potCollider.bounds.center.x, potCollider.bounds.max.y + offsetY, seedTransform.position.z);
        seedTransform.SetParent(GameObject.Find("-Objects-").transform); // Set the plant under the "-Objects-" gameobject
    }

    // Assign a random vine seedling sprite to the plant
    private void AssignRandomSeedling(Plant plant)
    {
        int randomIndex = UnityEngine.Random.Range(1, 3);
        switch (randomIndex)
        {
            case 1:
                plant.seedlingSprite = plant.vineSeedling1;
                plant.seedlingWitheredSprite = plant.vineSeedlingWithered1;
                break;
            case 2:
                plant.seedlingSprite = plant.vineSeedling2;
                plant.seedlingWitheredSprite = plant.vineSeedlingWithered2;
                break;
        }
        plant.SetImageForCurrentStage(); // Update the image to reflect the selected seedling
    }
}
