using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Items> itemList;

    public Inventory()
    {
        itemList = new List<Items>();

        AddItem(new Items { itemType = Items.ItemType.});
        Debug.Log("Inventory");
    }

    public void AddItem(Items item)
    {
        itemList.Add(item);
    }
}
