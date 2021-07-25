using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public enum ItemType
    {
        Egg,
        Butter,
        Flour,
        Milk,
        Cream,
        Bread,
        Fruit,
        Spatula,
        Firework,
        Balloon,
        Present,
        Gun,
        DogSnack
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Egg: return ItemAssets.Instance.eggSprite;
            case ItemType.Butter: return ItemAssets.Instance.butterSprite;
            case ItemType.Flour: return ItemAssets.Instance.flourSprite;
            case ItemType.Milk: return ItemAssets.Instance.milkSprite;
            case ItemType.Cream: return ItemAssets.Instance.creamSprite;
            case ItemType.Bread: return ItemAssets.Instance.breadSprite;
            case ItemType.Fruit: return ItemAssets.Instance.fruitSprite;
            case ItemType.Spatula: return ItemAssets.Instance.spatulaSprite;
            case ItemType.Firework: return ItemAssets.Instance.fireworkSprite;
            case ItemType.Balloon: return ItemAssets.Instance.balloonSprite;
            case ItemType.Present: return ItemAssets.Instance.presentSprite;
            case ItemType.Gun: return ItemAssets.Instance.gunSprite;
            case ItemType.DogSnack: return ItemAssets.Instance.dogsnackSprite;
        }
    }
}
