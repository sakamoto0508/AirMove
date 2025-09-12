using System;
using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public Action ItemHitNoArgAction;
    public Action<ItemBase, string, ItemData.itemType> ItemHitAction;
    public string ItemName { get; private set; }
    public ItemData.itemType ItemType { get; private set; }
    public float ItemEffectTime { get; private set; }
    private bool _hasBeenUsed = false;
    private Coroutine _effectCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_hasBeenUsed) return;
        if (other.CompareTag("Player"))
        {
            ItemHitNoArgAction?.Invoke();
            if (ItemType == ItemData.itemType.Temporary)
            {
                _effectCoroutine = StartCoroutine(TemporaryEffectCoroutine());
            }
            else
            {
                ItemEffect();
            }
            this.gameObject.SetActive(false);
            ItemHitAction?.Invoke(this, ItemName, ItemType);
            _hasBeenUsed = true;
        }
    }

    private IEnumerator TemporaryEffectCoroutine()
    {
        OnEffectStart();
        yield return new WaitForSeconds(ItemEffectTime);
        OnEffectEnd();
        _effectCoroutine = null;
    }

    /// <summary>
    /// 永続バフ効果
    /// </summary>
    public virtual void ItemEffect()
    {

    }

    /// <summary>
    /// 一時バフ効果の開始
    /// </summary>
    public virtual void OnEffectStart()
    {

    }

    /// <summary>
    /// 一時効果バフの終了
    /// </summary>
    public virtual void OnEffectEnd()
    {

    }

    public virtual void SetUp(ItemData itemData)
    {
        ItemName = itemData.ItemName;
        ItemType = itemData.ItemType;
        ItemEffectTime = itemData.EffectTime;
    }
}
