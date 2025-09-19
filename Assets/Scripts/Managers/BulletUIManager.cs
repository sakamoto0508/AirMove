using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUIManager : MonoBehaviour
{
    [Header("弾丸UI設定")]
    [SerializeField] private GameObject bulletIconPrefab; // 弾丸のImageプレハブ
    [SerializeField] private Transform bulletContainer;   // 弾丸アイコンの親オブジェクト
    [SerializeField] private Sprite bulletFullSprite;     // 残弾ありの画像
    [SerializeField] private Sprite bulletEmptySprite;    // 残弾なしの画像
    [SerializeField] private Sprite bulletReloadSprite;   // リロード中の画像
    [SerializeField] private Color bulletFullColor = Color.white;
    [SerializeField] private Color bulletEmptyColor = Color.gray;
    [Header("レイアウト設定")]
    [SerializeField] private float bulletSpacing = 5f;    // 弾丸間のスペース
    [SerializeField] private bool useHorizontalLayout = true; // 横並びにするか

    private List<Image> bulletImages = new List<Image>();
    private PlayerFire playerFire;
    private int maxBullets = 0;
    private bool isInitialized = false;


    private void Start()
    {
        // PlayerFireの参照を取得
        playerFire = FindAnyObjectByType<PlayerFire>();
        if (playerFire == null)
        {
            Debug.LogError("PlayerFireが見つかりません！");
            return;
        }
        playerFire.OnMaxBulletsChanged += OnMaxBulletsChanged;
        // 初期化を少し遅らせる（PlayerFireの初期化を待つ）
        Invoke(nameof(InitializeBulletUI), 0.1f);

    }

    private void Update()
    {
        if (isInitialized && playerFire != null)
        {
            UpdateBulletDisplay();
        }
    }

    /// <summary>
    /// 弾丸UIの初期化
    /// </summary>
    private void InitializeBulletUI()
    {
        if (playerFire == null) return;

        // 最大弾数を取得（現在の弾数から推定）
        maxBullets = playerFire.GetMaxBullets();

        if (maxBullets <= 0)
        {
            Debug.LogWarning("最大弾数が0以下です。初期化を再試行します。");
            Invoke(nameof(InitializeBulletUI), 0.5f);
            return;
        }

        CreateBulletIcons();
        SetupLayout();
        isInitialized = true;

        Debug.Log($"弾丸UI初期化完了 - 最大弾数: {maxBullets}");
    }

    /// <summary>
    /// 弾丸アイコンを作成
    /// </summary>
    private void CreateBulletIcons()
    {
        // 既存のアイコンをクリア
        ClearBulletIcons();

        if (bulletIconPrefab == null)
        {
            Debug.LogWarning("bulletIconPrefabが設定されていません。デフォルトで作成します。");
            CreateDefaultBulletIcons();
            return;
        }

        // 弾丸アイコンを作成
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bulletObj = Instantiate(bulletIconPrefab, bulletContainer);
            Image bulletImage = bulletObj.GetComponent<Image>();

            if (bulletImage == null)
            {
                bulletImage = bulletObj.AddComponent<Image>();
            }

            bulletImages.Add(bulletImage);
        }
    }

    /// <summary>
    /// デフォルトの弾丸アイコンを作成（プレハブが設定されていない場合）
    /// </summary>
    private void CreateDefaultBulletIcons()
    {
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bulletObj = new GameObject($"Bullet_{i}");
            bulletObj.transform.SetParent(bulletContainer);

            Image bulletImage = bulletObj.AddComponent<Image>();
            bulletImage.sprite = bulletFullSprite;
            bulletImage.color = bulletFullColor;

            // デフォルトサイズを設定
            RectTransform rectTransform = bulletObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(20, 20);

            bulletImages.Add(bulletImage);
        }
    }

    /// <summary>
    /// レイアウトを設定
    /// </summary>
    private void SetupLayout()
    {
        if (bulletContainer == null) return;

        // Horizontal Layout Groupがある場合は設定
        HorizontalLayoutGroup horizontalLayout = bulletContainer.GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayout != null && useHorizontalLayout)
        {
            horizontalLayout.spacing = bulletSpacing;
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childForceExpandWidth = false;
            horizontalLayout.childForceExpandHeight = false;
        }

        // Vertical Layout Groupがある場合は設定
        VerticalLayoutGroup verticalLayout = bulletContainer.GetComponent<VerticalLayoutGroup>();
        if (verticalLayout != null && !useHorizontalLayout)
        {
            verticalLayout.spacing = bulletSpacing;
            verticalLayout.childControlWidth = false;
            verticalLayout.childControlHeight = false;
            verticalLayout.childForceExpandWidth = false;
            verticalLayout.childForceExpandHeight = false;
        }
    }

    /// <summary>
    /// 弾丸表示を更新
    /// </summary>
    private void UpdateBulletDisplay()
    {
        if (bulletImages.Count == 0) return;

        int currentBullets = playerFire.GetCurrentBullets();
        bool isReloading = playerFire.IsReloading();

        for (int i = 0; i < bulletImages.Count; i++)
        {
            if (bulletImages[i] == null) continue;

            if (isReloading)
            {
                // リロード中は全て黄色
                bulletImages[i].sprite = bulletReloadSprite;
                bulletImages[i].color = Color.white;
            }
            else if (i < currentBullets)
            {
                // 残弾あり
                bulletImages[i].sprite = bulletFullSprite;
                bulletImages[i].color = bulletFullColor;
            }
            else
            {
                // 残弾なし
                bulletImages[i].sprite = bulletEmptySprite ?? bulletFullSprite;
                bulletImages[i].color = bulletEmptyColor;
            }
        }
    }

    /// <summary>
    /// 弾丸アイコンをクリア
    /// </summary>
    private void ClearBulletIcons()
    {
        foreach (var bulletImage in bulletImages)
        {
            if (bulletImage != null)
            {
                Destroy(bulletImage.gameObject);
            }
        }
        bulletImages.Clear();
    }

    /// <summary>
    /// 最大弾数が変更されたときの処理
    /// </summary>
    public void OnMaxBulletsChanged()
    {
        if (playerFire != null)
        {
            int newMaxBullets = playerFire.GetMaxBullets();
            if (newMaxBullets != maxBullets)
            {
                maxBullets = newMaxBullets;
                CreateBulletIcons();
                SetupLayout();
            }
        }
    }

    /// <summary>
    /// UI設定を変更
    /// </summary>
    public void SetBulletSprites(Sprite fullSprite, Sprite emptySprite)
    {
        bulletFullSprite = fullSprite;
        bulletEmptySprite = emptySprite;
    }

    public void SetBulletColors(Color fullColor, Color emptyColor, Color reloadColor)
    {
        bulletFullColor = fullColor;
        bulletEmptyColor = emptyColor;
    }
}