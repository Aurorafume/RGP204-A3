using System;
using UnityEngine;

public enum GrowthStage { Seedling, FirstGrowth, SecondGrowth, Mature, Withered }

public class Plant : MonoBehaviour
{
    public GrowthStage currentStage;
    public Sprite seedlingSprite;
    public Sprite firstGrowthSprite;
    public Sprite secondGrowthSprite;
    public Sprite matureSprite;
    public Sprite seedlingWitheredSprite;
    public Sprite firstGrowthWitheredSprite;
    public Sprite secondGrowthWitheredSprite;
    public Sprite matureWitheredSprite;
    public float timeBetweenStages = 60f;
    private float lastWatered;
    private bool isWatered;
    public int sellPrice = 25;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentStage = GrowthStage.Seedling;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpriteForCurrentStage();
        lastWatered = Time.time;
        isWatered = false;
    }

    void Update()
    {
        Debug.Log($"Current Stage: {currentStage}, Is Watered: {isWatered}, Time Since Last Watered: {Time.time - lastWatered}");
        if (currentStage == GrowthStage.Withered) return;

        if (isWatered && Time.time - lastWatered >= timeBetweenStages)
        {
            Debug.Log("Calling AdvanceGrowth.");
            AdvanceGrowth();
        }
        else if (!isWatered && Time.time - lastWatered >= timeBetweenStages)
        {
            Debug.Log("Calling Wither.");
            Wither();
        }
    }

    public void WaterPlant()
    {
        isWatered = true;
        lastWatered = Time.time;
        Debug.Log("Plant watered.");
        AdvanceGrowth();
    }

    private void AdvanceGrowth()
    {
        Debug.Log("AdvanceGrowth called.");
        if (currentStage < GrowthStage.Mature)
        {   
            currentStage++;
            isWatered = false; // Reset the isWatered flag
            SetSpriteForCurrentStage();
            Debug.Log($"Plant advanced to {currentStage} stage.");
        }
    }

    private void Wither()
    {
        Debug.Log("Wither called.");
        SetWitheredSpriteForCurrentStage();
        currentStage = GrowthStage.Withered;
        Debug.Log("Plant withered.");
    }

    public void RemovePlant()
    {
        Debug.Log("Plant removed.");
        Destroy(gameObject);
    }

    private void SetSpriteForCurrentStage()
    {
        switch (currentStage)
        {
            case GrowthStage.Seedling:
                spriteRenderer.sprite = seedlingSprite;
                break;
            case GrowthStage.FirstGrowth:
                spriteRenderer.sprite = firstGrowthSprite;
                break;
            case GrowthStage.SecondGrowth:
                spriteRenderer.sprite = secondGrowthSprite;
                break;
            case GrowthStage.Mature:
                spriteRenderer.sprite = matureSprite;
                break;
        }
        Debug.Log($"Sprite set to {spriteRenderer.sprite.name} for stage {currentStage}");
    }

    private void SetWitheredSpriteForCurrentStage()
    {
        switch (currentStage)
        {
            case GrowthStage.Seedling:
                spriteRenderer.sprite = seedlingWitheredSprite;
                break;
            case GrowthStage.FirstGrowth:
                spriteRenderer.sprite = firstGrowthWitheredSprite;
                break;
            case GrowthStage.SecondGrowth:
                spriteRenderer.sprite = secondGrowthWitheredSprite;
                break;
            case GrowthStage.Mature:
                spriteRenderer.sprite = matureWitheredSprite;
                break;
        }
        Debug.Log($"Withered sprite set to {spriteRenderer.sprite.name} for stage {currentStage}");
    }

    public void SellPlant()
    {
        Debug.Log("Plant sold for:" + sellPrice);
        Destroy(gameObject);
    }
}
