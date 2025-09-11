using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Item/Item DataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> ItemList;
}
