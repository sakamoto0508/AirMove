using UnityEngine;

public class ScoreUpItem : ItemBase
{
    private float _scoreMultiplier;
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
        _scoreMultiplier = itemData.ScoreUpMultiplier;
    }

    public override void OnEffectStart()
    {
        base.OnEffectStart();
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScoreMultiplier(_scoreMultiplier);
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScoreMultiplier();
        }
    }

}
