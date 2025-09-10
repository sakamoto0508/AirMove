using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _itemEffectTime;
    public string ItemName => _itemName;
    public GameObject ItemPrefab => _itemPrefab;
    public float EffectTime => _itemEffectTime;
}
