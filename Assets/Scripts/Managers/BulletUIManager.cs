using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUIManager : MonoBehaviour
{
    [Header("�e��UI�ݒ�")]
    [SerializeField] private GameObject bulletIconPrefab; // �e�ۂ�Image�v���n�u
    [SerializeField] private Transform bulletContainer;   // �e�ۃA�C�R���̐e�I�u�W�F�N�g
    [SerializeField] private Sprite bulletFullSprite;     // �c�e����̉摜
    [SerializeField] private Sprite bulletEmptySprite;    // �c�e�Ȃ��̉摜
    [SerializeField] private Sprite bulletReloadSprite;   // �����[�h���̉摜
    [SerializeField] private Color bulletFullColor = Color.white;
    [SerializeField] private Color bulletEmptyColor = Color.gray;
    [Header("���C�A�E�g�ݒ�")]
    [SerializeField] private float bulletSpacing = 5f;    // �e�ۊԂ̃X�y�[�X
    [SerializeField] private bool useHorizontalLayout = true; // �����тɂ��邩

    private List<Image> bulletImages = new List<Image>();
    private PlayerFire playerFire;
    private int maxBullets = 0;
    private bool isInitialized = false;


    private void Start()
    {
        // PlayerFire�̎Q�Ƃ��擾
        playerFire = FindAnyObjectByType<PlayerFire>();
        if (playerFire == null)
        {
            Debug.LogError("PlayerFire��������܂���I");
            return;
        }
        playerFire.OnMaxBulletsChanged += OnMaxBulletsChanged;
        // �������������x�点��iPlayerFire�̏�������҂j
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
    /// �e��UI�̏�����
    /// </summary>
    private void InitializeBulletUI()
    {
        if (playerFire == null) return;

        // �ő�e�����擾�i���݂̒e�����琄��j
        maxBullets = playerFire.GetMaxBullets();

        if (maxBullets <= 0)
        {
            Debug.LogWarning("�ő�e����0�ȉ��ł��B���������Ď��s���܂��B");
            Invoke(nameof(InitializeBulletUI), 0.5f);
            return;
        }

        CreateBulletIcons();
        SetupLayout();
        isInitialized = true;

        Debug.Log($"�e��UI���������� - �ő�e��: {maxBullets}");
    }

    /// <summary>
    /// �e�ۃA�C�R�����쐬
    /// </summary>
    private void CreateBulletIcons()
    {
        // �����̃A�C�R�����N���A
        ClearBulletIcons();

        if (bulletIconPrefab == null)
        {
            Debug.LogWarning("bulletIconPrefab���ݒ肳��Ă��܂���B�f�t�H���g�ō쐬���܂��B");
            CreateDefaultBulletIcons();
            return;
        }

        // �e�ۃA�C�R�����쐬
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
    /// �f�t�H���g�̒e�ۃA�C�R�����쐬�i�v���n�u���ݒ肳��Ă��Ȃ��ꍇ�j
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

            // �f�t�H���g�T�C�Y��ݒ�
            RectTransform rectTransform = bulletObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(20, 20);

            bulletImages.Add(bulletImage);
        }
    }

    /// <summary>
    /// ���C�A�E�g��ݒ�
    /// </summary>
    private void SetupLayout()
    {
        if (bulletContainer == null) return;

        // Horizontal Layout Group������ꍇ�͐ݒ�
        HorizontalLayoutGroup horizontalLayout = bulletContainer.GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayout != null && useHorizontalLayout)
        {
            horizontalLayout.spacing = bulletSpacing;
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childForceExpandWidth = false;
            horizontalLayout.childForceExpandHeight = false;
        }

        // Vertical Layout Group������ꍇ�͐ݒ�
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
    /// �e�ە\�����X�V
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
                // �����[�h���͑S�ĉ��F
                bulletImages[i].sprite = bulletReloadSprite;
                bulletImages[i].color = Color.white;
            }
            else if (i < currentBullets)
            {
                // �c�e����
                bulletImages[i].sprite = bulletFullSprite;
                bulletImages[i].color = bulletFullColor;
            }
            else
            {
                // �c�e�Ȃ�
                bulletImages[i].sprite = bulletEmptySprite ?? bulletFullSprite;
                bulletImages[i].color = bulletEmptyColor;
            }
        }
    }

    /// <summary>
    /// �e�ۃA�C�R�����N���A
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
    /// �ő�e�����ύX���ꂽ�Ƃ��̏���
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
    /// UI�ݒ��ύX
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