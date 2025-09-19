using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string name;      // 呼び出し用の名前
    public AudioClip clip;   // 実際の AudioClip
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM 用 AudioSource")]
    [SerializeField] private AudioSource bgmSource;
    [Header("SE 用 AudioSource プレハブ")]
    [SerializeField] private AudioSource sfxSourcePrefab;
    [Header("BGMリスト")]
    [SerializeField] private List<SoundData> bgmList = new List<SoundData>();
    [Header("効果音リスト")]
    [SerializeField] private List<SoundData> seList = new List<SoundData>();
    [Header("効果音プール設定")]
    [SerializeField] private int sfxPoolSize = 10;

    private Dictionary<string, AudioClip> _bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _seDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> _sfxPool = new List<AudioSource>();

    private void Awake()
    {
        // シングルトン確保
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioManager()
    {
        // BGM用 AudioSource の確保
        EnsureBGMSource();

        // BGMを辞書に登録
        _bgmDict.Clear();
        foreach (var bgm in bgmList)
        {
            if (!_bgmDict.ContainsKey(bgm.name) && bgm.clip != null)
            {
                _bgmDict.Add(bgm.name, bgm.clip);
            }
        }

        // SE を辞書に登録
        _seDict.Clear();
        foreach (var se in seList)
        {
            if (!_seDict.ContainsKey(se.name) && se.clip != null)
            {
                _seDict.Add(se.name, se.clip);
            }
        }

        // プールを初期化
        CreateSFXPool();
    }

    private void EnsureBGMSource()
    {
        // 既存のbgmSourceが有効かチェック
        if (bgmSource == null || bgmSource.Equals(null))
        {
            // BGM用のAudioSourceを子オブジェクトとして作成
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }
    }

    private void CreateSFXPool()
    {
        // 既存のプールをクリア
        foreach (var source in _sfxPool)
        {
            if (source != null)
            {
                DestroyImmediate(source.gameObject);
            }
        }
        _sfxPool.Clear();

        // 新しいプールを作成
        for (int i = 0; i < sfxPoolSize; i++)
        {
            if (sfxSourcePrefab != null)
            {
                var sfxSource = Instantiate(sfxSourcePrefab, transform);
                sfxSource.playOnAwake = false;
                sfxSource.loop = false; // ループ禁止（効果音用）
                sfxSource.clip = null;  // clip を空にしておく
                _sfxPool.Add(sfxSource);
            }
        }
    }


    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // BGM専用のAudioSourceを確実に用意
        EnsureBGMSource();

        if (bgmSource == null)
        {
            Debug.LogError("bgmSource の作成に失敗しました。");
            return;
        }

        // SE用AudioSourceでは再生しない（bgmSource専用）
        foreach (var sfx in _sfxPool)
        {
            if (sfx != null && sfx.isPlaying && sfx.clip == clip)
            {
                sfx.Stop();
                sfx.clip = null;
            }
        }

        // 同じBGMが再生中なら何もしない
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true; // 必ずループ
        bgmSource.Play();
    }


    /// <summary>
    /// BGM 再生（名前指定）
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        if (_bgmDict.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"指定された BGM '{bgmName}' は登録されていません。");
        }
    }

    /// <summary>
    /// BGM 停止
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource != null && !bgmSource.Equals(null))
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }
    }

    /// <summary>
    /// BGM 音量調整
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        EnsureBGMSource();
        if (bgmSource != null)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// 効果音再生（名前指定）
    /// </summary>
    public void PlaySE(string seName, float volume = 1f)
    {
        if (_seDict.TryGetValue(seName, out var clip))
        {
            var src = GetAvailableSfxSource();
            if (src != null)
            {
                src.clip = clip;
                src.volume = Mathf.Clamp01(volume);
                src.Play();
            }
        }
        else
        {
            Debug.LogWarning($"指定された SE '{seName}' は登録されていません。");
        }
    }

    /// <summary>
    /// 空いている SE 用 AudioSource を探す
    /// </summary>
    private AudioSource GetAvailableSfxSource()
    {
        foreach (var s in _sfxPool)
        {
            if (s != null && !s.isPlaying) return s;
        }

        // 全部埋まってたら新しく作る
        if (sfxSourcePrefab != null)
        {
            var extra = Instantiate(sfxSourcePrefab, transform);
            extra.playOnAwake = false;
            _sfxPool.Add(extra);
            return extra;
        }

        return null;
    }

    /// <summary>
    /// SE 音量調整
    /// </summary>
    public void SetSEVolume(float volume)
    {
        foreach (var s in _sfxPool)
        {
            if (s != null)
            {
                s.volume = Mathf.Clamp01(volume);
            }
        }
    }
}
