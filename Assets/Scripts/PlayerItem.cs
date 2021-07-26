using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{

    //인벤토리
    private Inventory inventory;

    [SerializeField] private UI_Inventory uiInventory;

    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        ItemWorld.SpawnItemWorld(new Vector3(20, 20), new Items { itemType = Items.ItemType.Egg, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(0, -20), new Items { itemType = Items.ItemType.Butter, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-20, -20), new Items { itemType = Items.ItemType.Flour, amount = 1 });
    }

    private void Start()
    {
    }

    //아이템 먹기
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (inventory.itemList.Count == 5)
        {
            return;
        }

        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
}
