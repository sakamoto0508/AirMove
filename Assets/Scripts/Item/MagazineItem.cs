using UnityEngine;

public class MagazineItem : ItemBase
{
    private int _magazineSizeUp;
    private int _magazineSize;
    private PlayerData _playerData;
    private void Start()
    {
        _playerData=FindAnyObjectByType<PlayerData>();
        SetMagazineSize(_playerData.MagazineSize);
    }

    public override void SetUp(ItemData itemData)
    {
        base.SetUp(itemData);
        _magazineSizeUp = itemData.MagazineSizeUp;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();
        var playerFire = FindAnyObjectByType<PlayerFire>();
        if (playerFire != null)
        {
            playerFire.SetMagazineSizeUp(_magazineSizeUp);
        }
    }

    public void SetMagazineSize(int magazineSize)
    {
        _magazineSize = magazineSize;
    }
}
