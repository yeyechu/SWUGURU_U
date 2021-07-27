using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    public List<Items> itemList;
    private Action<Items> useItemAction;

    public Inventory(Action<Items> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Items>();
        /*
        AddItem(new Items { itemType = Items.ItemType.Egg, amount  = 1});
        AddItem(new Items { itemType = Items.ItemType.Butter, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Flour, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Milk, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Bread, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Fruit, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Spatula, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Firework, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Balloon, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Present, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.Watch, amount = 1 });
        AddItem(new Items { itemType = Items.ItemType.DogSnack, amount = 1 });*/
    }

    public void AddItem(Items item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Items item)
    {
        useItemAction(item);
    }

    public void RemoveItem(Items item)
    {
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Items> GetItemList()
    {
        return itemList;
    }
}
