using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _itemEffectTime;
    [SerializeField] private float _spawnProbability;
    [SerializeField] private itemType _itemType;
    [Header("magazine")]
    [SerializeField] private int _magazineSizeUp = 1;
    [Header("animation")]
    [SerializeField] private float _animationSpeed = 0.1f;
    public enum itemType
    {
        Forever,
        Temporary
    }
    public string ItemName => _itemName;
    public GameObject ItemPrefab => _itemPrefab;
    public float EffectTime => _itemEffectTime;
    public float SpawnProbability => _spawnProbability;
    public itemType ItemType => _itemType;
    public int MagazineSizeUp => _magazineSizeUp;
    public float AnimationSpeed => _animationSpeed;
}
