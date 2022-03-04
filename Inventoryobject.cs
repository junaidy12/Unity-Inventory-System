using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new inventory", menuName ="Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
    public int space;
    public Inventory inventory;

    public (bool, int) AddItem(Item _item, int _amount)
    {
        int amountAfterAddToInventory = 0;
        //check if item is not stackable
        if (!_item.isStackable)
        {
            if (UpdateEmptySlot(_item, _amount) == null)
                return (false, 0);
        }

        //check if item exist
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            amountAfterAddToInventory = inventory.GetCurrentAmount(i) + _amount;
            // check if amount when added is more than a stack
            if(inventory.slots[i].item.id == _item.id && amountAfterAddToInventory >= inventory.slots[i].item.maxItemPerStack)
            {
                // check if current amount in current slot not a stack
                if(inventory.GetCurrentAmount(i) < inventory.slots[i].item.maxItemPerStack)
                {

                    int amountToAddToFullStack = inventory.slots[i].item.maxItemPerStack - inventory.GetCurrentAmount(i);
                    // add amount to full a stack
                    inventory.slots[i].AddAmount(amountToAddToFullStack);
                    int amountRemainder = _amount - amountToAddToFullStack;
                    // check if remainder is more than a stack
                    if (amountRemainder > _item.maxItemPerStack)
                    {
                        float addTimes = (float)amountRemainder / (float)_item.maxItemPerStack;
                        int loopCount = (int)Mathf.Floor(addTimes);
                        int remainder = amountRemainder - (int)(_item.maxItemPerStack * loopCount);
                        //loop through how many times to add full stack into inventory
                        for (int j = 0; j < loopCount; j++)
                        {
                            //add stacks of items
                            if (UpdateEmptySlot(_item, _item.maxItemPerStack) == null)
                                return (false, amountToAddToFullStack + (_item.maxItemPerStack * j));
                        }
                        if (remainder > 0)
                        {
                            //add the remainders
                            if (UpdateEmptySlot(_item, remainder) != null)
                            {
                                return (true,0);
                            }
                            else
                            {
                                return (false, _item.maxItemPerStack * loopCount);
                            }
                        }
                    }
                    // if the remainders is less than a stack
                    else if(amountRemainder <= _item.maxItemPerStack)
                    {
                        if (UpdateEmptySlot(_item, amountRemainder) != null)
                        {
                            return (true, 0);
                        }
                        else 
                        { 
                            return (false, amountToAddToFullStack);
                        }
                    }
                }
            }
            // check if amount to be add is less than a stack then just add amount ti current slot
            else if (inventory.slots[i].item.id == _item.id && amountAfterAddToInventory < inventory.slots[i].item.maxItemPerStack)
            {
                inventory.slots[i].AddAmount(_amount);
                return (true, 0);
            }
        }

        // add item if not exist
        if (_amount > _item.maxItemPerStack)
        {
            //adding item into multiple stack if amount to add is more than one stack
            float addTimes = (float)_amount / (float)_item.maxItemPerStack;
            int loopCount = (int)Mathf.Floor(addTimes);
            int remainder = _amount - (int)(_item.maxItemPerStack * loopCount);
            //loop through how many times to add full stack into inventory
            for (int j = 0; j < loopCount; j++)
            {
                if (UpdateEmptySlot(_item, _item.maxItemPerStack) == null)
                    return (false, _item.maxItemPerStack * j);
            }
            if (remainder > 0)
            {
                if (UpdateEmptySlot(_item, remainder) != null)
                {
                    return (true, 0);
                }
                else
                {
                    return (false, _item.maxItemPerStack * loopCount);
                }
            }
        }
        // adding item into empty slot if less than a stack
        else
        {
            if (UpdateEmptySlot(_item, _amount) != null)
            {
                return (true, 0);
            }
            else
            {
                return (false, 0);
            }
        }
        return (false, 0);   

    }

    public InventorySlot UpdateEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if(inventory.slots[i].item.id <= -1)
            {
                inventory.slots[i] = new InventorySlot(_item, _amount);
                return inventory.slots[i];
            }
        }
        //this is where to implement logic for inventory full
        Debug.Log("Inventory Full");
        return null;
    }

    [ContextMenu("Clear")]
    public void InitializeInventory()
    {
        inventory = new Inventory(space);
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public Inventory(int _space)
    {
        slots = new InventorySlot[_space];
    }
    public int GetCurrentAmount(int index)
    {
        return slots[index].amount;
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int _amount)
    {
        amount += _amount;
    }
}
