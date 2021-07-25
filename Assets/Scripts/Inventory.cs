using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Items> itemList;

    public Inventory()
    {
        itemList = new List<Items>();

        AddItem(new Items { itemType = Items.ItemType.Egg, amount  = 1});
        AddItem(new Items { itemType = Items.ItemType.Butter, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Flour, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Milk, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Cream, amount = 1 });
        Debug.Log("Inventory");
    }

    public void AddItem(Items item)
    {
        itemList.Add(item);
    }

    public List<Items> GetItemList()
    {
        return itemList;
    }
}
