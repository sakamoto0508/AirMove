using UnityEngine;

public class PlayerAnimationItem : ItemBase
{
    private float _animationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetUp(ItemData itemData)
    {
        base.SetUp(itemData);
        _animationSpeed = itemData.AnimationSpeed;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();
        var playerAnimation = FindAnyObjectByType<PlayerAnimation>();
        if(playerAnimation != null)
        {
            playerAnimation.AddCombatAnimationSpeedUP(_animationSpeed);
            Debug.Log(_animationSpeed);
        }
    }
}
