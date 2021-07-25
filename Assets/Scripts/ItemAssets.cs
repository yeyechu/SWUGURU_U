using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite eggSprite;
    public Sprite butterSprite;
    public Sprite flourSprite;
    public Sprite milkSprite;
    public Sprite creamSprite;
    public Sprite breadSprite;
    public Sprite fruitSprite;
    public Sprite spatulaSprite;
    public Sprite fireworkSprite;
    public Sprite balloonSprite;
    public Sprite presentSprite;
    public Sprite gunSprite;
    public Sprite dogsnackSprite;
}
