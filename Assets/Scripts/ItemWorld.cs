using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Items item)
    {
        Instantiate();
    }

    private ItemWorld item;

    public void SetItem(Items item)
    {
        this.item = item;
    }
}
