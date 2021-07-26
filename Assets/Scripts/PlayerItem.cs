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
        
    }

    private void Start()
    {
        uiInventory.SetInventory(inventory);
    }

    //아이템 먹기
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
}
