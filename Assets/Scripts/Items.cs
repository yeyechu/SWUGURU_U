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
        Spatula
    }

    public ItemType itemType;
    public int amount;
}
