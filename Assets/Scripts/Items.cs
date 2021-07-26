using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Items
{
    public enum ItemType
    {
        Egg, Butter, Flour, Milk, Cream, Bread, Fruit, Spatula, Firework, Balloon, Present, Watch, DogSnack, wrapPaper, chocolate, bowl, breadBoard, glove, stir
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Egg:        return ItemAssets.Instance.eggSprite;
            case ItemType.Butter:     return ItemAssets.Instance.butterSprite;
            case ItemType.Flour:      return ItemAssets.Instance.flourSprite;
            case ItemType.Milk:       return ItemAssets.Instance.milkSprite;
            case ItemType.Cream:      return ItemAssets.Instance.creamSprite;
            case ItemType.Bread:      return ItemAssets.Instance.breadSprite;
            case ItemType.Fruit:      return ItemAssets.Instance.fruitSprite;
            case ItemType.Spatula:    return ItemAssets.Instance.spatulaSprite;
            case ItemType.Firework:   return ItemAssets.Instance.fireworkSprite;
            case ItemType.Balloon:    return ItemAssets.Instance.balloonSprite;
            case ItemType.Present:    return ItemAssets.Instance.presentSprite;
            case ItemType.Watch:      return ItemAssets.Instance.watchSprite;
            case ItemType.DogSnack:   return ItemAssets.Instance.dogsnackSprite;
            case ItemType.wrapPaper:  return ItemAssets.Instance.wrapPaperSprite;
            case ItemType.chocolate:  return ItemAssets.Instance.chocolateSprite;
            case ItemType.bowl:       return ItemAssets.Instance.bowlSprite;
            case ItemType.breadBoard: return ItemAssets.Instance.breadBoardSprite;
            case ItemType.glove:      return ItemAssets.Instance.gloveSprite;
            case ItemType.stir:       return ItemAssets.Instance.stirSprite;
        }
    }
}
