using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item       // this is base class for item (you can create an object base on this class)
{
    public int id;
    public int maxItemPerStack;
    public bool isStackable;
    public string name;
    public ItemBuff[] buffs;
    /* if project not using ScriptableObject class then creating object should be like this
     * 
     *  public Item(int _id, string _name)
     *  {
     *      id = _id;
     *      name = _name;
     *  }
     *  
     *  method above is a constructor for Item class ( creating an object of Item )
     */
    public Item()
    {
        id = -1;
        isStackable = true;
        maxItemPerStack = 1;
        name = "";
        buffs = null;
    }
    public Item(ItemObject _item)
    {
        id = _item.id;
        maxItemPerStack = _item.maxItemPerStack;
        isStackable = _item.isStackable;
        name = _item.name;
        buffs = new ItemBuff[_item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(_item.buffs[i].min, _item.buffs[i].max)
            {
                attributes = _item.buffs[i].attributes
            };
        }
    }
}
