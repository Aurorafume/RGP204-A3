using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public enum GrowthStage { Seedling, FirstGrowth, SecondGrowth, Mature, Withered }
    public GrowthStage currentStage;
    public Sprite[] stageSprites;

    private SpriteRenderer spriteRenderer;
    private float growthTimer;
    public float growthDuration;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentStage = GrowthStage.Seedling;
        growthTimer = 0f;
    }

    void Update()
    {
        growthTimer += Time.deltaTime;
        if (growthTimer >= growthDuration)
        {
            Grow();
            growthTimer = 0f;
        }
    }

    void Grow()
    {
        if (currentStage != GrowthStage.Withered)
        {
            currentStage++;
            spriteRenderer.sprite = stageSprites[(int)currentStage];
        }
    }
}