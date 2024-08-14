using System;
using UnityEngine;
using UnityEngine.UI;

public enum GrowthStage { Seedling, FirstGrowth, SecondGrowth, Mature, Withered }
public enum SeedType { None, Normal, Vine }

public class Plant : MonoBehaviour
{
    public SeedType seedType = SeedType.None;
    public GrowthStage currentStage;

    // Sprites for different growth stages
    public Sprite normalSeedling1, normalFirstGrowth1, normalSecondGrowth1, normalMature1, normalSeedlingWithered1, normalFirstGrowthWithered1, normalSecondGrowthWithered1;
    public Sprite normalSeedling2, normalFirstGrowth2, normalSecondGrowth2, normalMature2, normalSeedlingWithered2, normalFirstGrowthWithered2, normalSecondGrowthWithered2;
    public Sprite normalSeedling3, normalFirstGrowth3, normalSecondGrowth3, normalMature3, normalSeedlingWithered3, normalFirstGrowthWithered3, normalSecondGrowthWithered3;
    public Sprite normalSeedling4, normalFirstGrowth4, normalSecondGrowth4, normalMature4, normalSeedlingWithered4, normalFirstGrowthWithered4, normalSecondGrowthWithered4;
    public Sprite normalSeedling5, normalFirstGrowth5, normalSecondGrowth5, normalMature5, normalSeedlingWithered5, normalFirstGrowthWithered5, normalSecondGrowthWithered5;
    
    public Sprite vineSeedling1, vineFirstGrowth1, vineSecondGrowth1, vineMature1, vineSeedlingWithered1, vineFirstGrowthWithered1, vineSecondGrowthWithered1;
    public Sprite vineSeedling2, vineFirstGrowth2, vineSecondGrowth2, vineMature2, vineSeedlingWithered2, vineFirstGrowthWithered2, vineSecondGrowthWithered2;

    private Sprite[][] normalSpritePaths = new Sprite[7][];
    private Sprite[][] vineSpritePaths = new Sprite[3][];
    private int spriteIndex = 0;

    public Sprite seedlingSprite;
    public Sprite seedlingWitheredSprite;

    public float timeBetweenStages = 60f;
    private float lastWatered;
    private bool isWatered;
    public int sellPrice = 25;
    public int plantingCost = 10;

    private Image imageComponent;

    // Flag to indicate if the plant is planted
    public bool isPlanted = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();

        currentStage = GrowthStage.Seedling;
        lastWatered = Time.time;
        isWatered = false;
        AssignRandomSeedling();  // Assign initial seedling sprite
        SetImageForCurrentStage(); // Set initial image

        // Define sprite paths
        normalSpritePaths[1] = new Sprite[] { normalSeedling1, normalFirstGrowth1, normalSecondGrowth1, normalMature1 };
        normalSpritePaths[2] = new Sprite[] { normalSeedling2, normalFirstGrowth2, normalSecondGrowth2, normalMature2 };
        normalSpritePaths[3] = new Sprite[] { normalSeedling3, normalFirstGrowth3, normalSecondGrowth3, normalMature3 };
        normalSpritePaths[4] = new Sprite[] { normalSeedling4, normalFirstGrowth4, normalSecondGrowth4, normalMature4 };
        normalSpritePaths[5] = new Sprite[] { normalSeedling5, normalFirstGrowth5, normalSecondGrowth5, normalMature5 };

        vineSpritePaths[1] = new Sprite[] { vineSeedling1, vineFirstGrowth1, vineSecondGrowth1, vineMature1 };
        vineSpritePaths[2] = new Sprite[] { vineSeedling2, vineFirstGrowth2, vineSecondGrowth2, vineMature2 };
    }

    void Update()
    {
        // Only update growth if the plant is planted
        if (!isPlanted) return;

        if (currentStage == GrowthStage.Withered) return;

        // Advance growth if watered and time passed
        if (isWatered && Time.time - lastWatered >= timeBetweenStages)
        {
            AdvanceGrowth();
        }
        // Wither if not watered and time passed
        else if (!isWatered && Time.time - lastWatered >= timeBetweenStages)
        {
            Wither();
        }
    }

    // Water the plant
    public void WaterPlant()
    {
        if (!isPlanted) return;

        isWatered = true;
        lastWatered = Time.time;
        AdvanceGrowth();
    }

    // Advance the plant's growth stage
    private void AdvanceGrowth()
    {
        if (currentStage < GrowthStage.Mature)
        {
            currentStage++;
            isWatered = false; // Reset the isWatered flag
            SetImageForCurrentStage();
        }
    }

    // Wither the plant
    private void Wither()
    {
        SetWitheredImageForCurrentStage();
        currentStage = GrowthStage.Withered;
    }

    // Remove the plant from the game
    public void RemovePlant()
    {
        Destroy(gameObject);
    }

    // Set the image for the current growth stage
    public void SetImageForCurrentStage()
    {
        if (!isPlanted) return;
        if (imageComponent == null)
        {
            return;
        }

        if (seedlingSprite == null)
        {
            return;
        }

        imageComponent.sprite = null; // Clear the existing image

        switch (currentStage)
        {
            case GrowthStage.Seedling:
                imageComponent.sprite = seedlingSprite;
                break;
            case GrowthStage.FirstGrowth:
                imageComponent.sprite = GetSpriteByStage(GrowthStage.FirstGrowth);
                break;
            case GrowthStage.SecondGrowth:
                imageComponent.sprite = GetSpriteByStage(GrowthStage.SecondGrowth);
                break;
            case GrowthStage.Mature:
                imageComponent.sprite = GetSpriteByStage(GrowthStage.Mature);
                break;
        }
    }

    // Set the withered image for the current growth stage
    private void SetWitheredImageForCurrentStage()
    {
        if (imageComponent == null)
        {
            return;
        }

        imageComponent.sprite = null; // Clear the existing image

        switch (currentStage)
        {
            case GrowthStage.Seedling:
                imageComponent.sprite = seedlingWitheredSprite;
                break;
            case GrowthStage.FirstGrowth:
                imageComponent.sprite = GetWitheredSpriteByStage(GrowthStage.FirstGrowth);
                break;
            case GrowthStage.SecondGrowth:
                imageComponent.sprite = GetWitheredSpriteByStage(GrowthStage.SecondGrowth);
                break;
        }
    }

    // Get the sprite for a specific growth stage
    private Sprite GetSpriteByStage(GrowthStage stage)
    {
        switch (seedType)
        {
            case SeedType.Normal:
                return GetNormalSpriteByIndex(spriteIndex, stage);
                break;
            case SeedType.Vine:
                return getVineSpriteByIndex(spriteIndex, stage);
                break;
        }
        return null;
    }

    // Get the withered sprite for a specific growth stage
    private Sprite GetWitheredSpriteByStage(GrowthStage stage)
    {
        switch (seedType)
        {
            case SeedType.Normal:
                switch (stage)
                {
                    case GrowthStage.FirstGrowth: return normalFirstGrowthWithered1; // Example, update for random logic
                    case GrowthStage.SecondGrowth: return normalSecondGrowthWithered1;
                }
                break;
            case SeedType.Vine:
                switch (stage)
                {
                    case GrowthStage.FirstGrowth: return vineFirstGrowthWithered1; // Example, update for random logic
                    case GrowthStage.SecondGrowth: return vineSecondGrowthWithered1;
                }
                break;
        }
        return null;
    }

    // Get the normal sprite by index
    private Sprite GetNormalSpriteByIndex(int index, GrowthStage stage)
    {
        if (index < 1 || index > 5) return normalSpritePaths[index][normalSpritePaths[index].Length - 1];
        return normalSpritePaths[index][(int)stage];
    }

    // Get the vine sprite by index
    private Sprite getVineSpriteByIndex(int index, GrowthStage stage)
    {
        if (index < 1 || index > 2) return vineSpritePaths[index][vineSpritePaths[index].Length - 1];
        return vineSpritePaths[index][(int)stage];
    }

    // Sell the plant for money
    public void SellPlant()
    {
        EconomyManager economyManager = GameObject.Find("Balance").GetComponent<EconomyManager>();
        if (economyManager != null)
        {
            economyManager.AddMoney(15); // Earn $15 when a plant is sold
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("EconomyManager not found. Cannot sell plant.");
        }
    }

    // Assign a random seedling sprite
    private void AssignRandomSeedling()
    {
        int randomIndex = UnityEngine.Random.Range(1, 6);
        spriteIndex = randomIndex;
        switch (randomIndex)
        {
            case 1:
                seedlingSprite = normalSeedling1;
                seedlingWitheredSprite = normalSeedlingWithered1;
                break;
            case 2:
                seedlingSprite = normalSeedling2;
                seedlingWitheredSprite = normalSeedlingWithered2;
                break;
            case 3:
                seedlingSprite = normalSeedling3;
                seedlingWitheredSprite = normalSeedlingWithered3;
                break;
            case 4:
                seedlingSprite = normalSeedling4;
                seedlingWitheredSprite = normalSeedlingWithered4;
                break;
            case 5:
                seedlingSprite = normalSeedling5;
                seedlingWitheredSprite = normalSeedlingWithered5;
                break;
        }
    }
}
