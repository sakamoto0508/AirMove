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
    [SerializeField] private int sfxPoolSize = 10; // 同時再生可能数（必要に応じて増やす）
    private Dictionary<string, AudioClip> _bgmDict=new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _seDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> _sfxPool = new List<AudioSource>();

    private void Awake()
    {
        // シングルトン確保
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // BGMを辞書に登録
        foreach (var bgm in bgmList)
        {
            if (!_bgmDict.ContainsKey(bgm.name) && bgm.clip != null)
            {
                _bgmDict.Add(bgm.name, bgm.clip);
            }
        }
        // SE を辞書に登録
        foreach (var se in seList)
        {
            if (!_seDict.ContainsKey(se.name) && se.clip != null)
            {
                _seDict.Add(se.name, se.clip);
            }
        }
        // プールを初期化
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var s = Instantiate(sfxSourcePrefab, transform);
            s.playOnAwake = false;
            _sfxPool.Add(s);
        }
    }

    /// <summary>
    /// BGM 再生
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
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
        bgmSource.Stop();
    }

    /// <summary>
    /// BGM 音量調整
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
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
            if (!s.isPlaying) return s;
        }

        // 全部埋まってたら新しく作る（安全策）
        var extra = Instantiate(sfxSourcePrefab, transform);
        extra.playOnAwake = false;
        _sfxPool.Add(extra);
        return extra;
    }

    /// <summary>
    /// SE 音量調整
    /// </summary>
    public void SetSEVolume(float volume)
    {
        foreach (var s in _sfxPool)
        {
            s.volume = Mathf.Clamp01(volume);
        }
    }
}
