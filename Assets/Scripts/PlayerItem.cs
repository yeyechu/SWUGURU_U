using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{

    //�κ��丮
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

    //������ �Ա�
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
