using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "Inventory/Item")]
public class Test : ScriptableObject          // this is object class, representing data for Item class ( hold data and value for creating Item )
{
    public int id;
    public bool isStackable;
    public int maxItemPerStack;
    public ItemType itemType = ItemType.Default;
    public Sprite itemIcon;
    [TextArea(10, 15)]
    public string itemDescription;
    public ItemBuff[] buffs;

    private void Awake()
    {
        itemDescription = $"This is a {name}";
    }
}

public enum ItemType
{
    Food,
    Equipment,
    Default
}