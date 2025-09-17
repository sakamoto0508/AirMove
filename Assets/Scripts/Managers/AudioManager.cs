using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string name;      // �Ăяo���p�̖��O
    public AudioClip clip;   // ���ۂ� AudioClip
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("BGM �p AudioSource")]
    [SerializeField] private AudioSource bgmSource;
    [Header("SE �p AudioSource �v���n�u")]
    [SerializeField] private AudioSource sfxSourcePrefab;
    [Header("BGM���X�g")]
    [SerializeField] private List<SoundData> bgmList = new List<SoundData>();
    [Header("���ʉ����X�g")]
    [SerializeField] private List<SoundData> seList = new List<SoundData>();
    [Header("���ʉ��v�[���ݒ�")]
    [SerializeField] private int sfxPoolSize = 10; // �����Đ��\���i�K�v�ɉ����đ��₷�j
    private Dictionary<string, AudioClip> _bgmDict=new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _seDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> _sfxPool = new List<AudioSource>();

    private void Awake()
    {
        // �V���O���g���m��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // BGM�������ɓo�^
        foreach (var bgm in bgmList)
        {
            if (!_bgmDict.ContainsKey(bgm.name) && bgm.clip != null)
            {
                _bgmDict.Add(bgm.name, bgm.clip);
            }
        }
        // SE �������ɓo�^
        foreach (var se in seList)
        {
            if (!_seDict.ContainsKey(se.name) && se.clip != null)
            {
                _seDict.Add(se.name, se.clip);
            }
        }
        // �v�[����������
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var s = Instantiate(sfxSourcePrefab, transform);
            s.playOnAwake = false;
            _sfxPool.Add(s);
        }
    }

    /// <summary>
    /// BGM �Đ�
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// BGM �Đ��i���O�w��j
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        if (_bgmDict.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽ BGM '{bgmName}' �͓o�^����Ă��܂���B");
        }
    }

    /// <summary>
    /// BGM ��~
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// BGM ���ʒ���
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// ���ʉ��Đ��i���O�w��j
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
            Debug.LogWarning($"�w�肳�ꂽ SE '{seName}' �͓o�^����Ă��܂���B");
        }
    }

    /// <summary>
    /// �󂢂Ă��� SE �p AudioSource ��T��
    /// </summary>
    private AudioSource GetAvailableSfxSource()
    {
        foreach (var s in _sfxPool)
        {
            if (!s.isPlaying) return s;
        }

        // �S�����܂��Ă���V�������i���S��j
        var extra = Instantiate(sfxSourcePrefab, transform);
        extra.playOnAwake = false;
        _sfxPool.Add(extra);
        return extra;
    }

    /// <summary>
    /// SE ���ʒ���
    /// </summary>
    public void SetSEVolume(float volume)
    {
        foreach (var s in _sfxPool)
        {
            s.volume = Mathf.Clamp01(volume);
        }
    }
}
