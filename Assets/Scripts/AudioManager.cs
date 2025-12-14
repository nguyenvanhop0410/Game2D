using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSfxSource; // Riêng cho UI ?? không b? lag

    [Header("Background Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    [Header("Sound Effects - Player")]
    [SerializeField] private AudioClip playerWalkSound;
    [SerializeField] private AudioClip playerDamageSound;
    [SerializeField] private AudioClip playerHealSound;

    [Header("Sound Effects - Chest")]
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip chestCollectSound;
    [SerializeField] private AudioClip trapSound;

    [Header("Sound Effects - UI")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip correctAnswerSound;
    [SerializeField] private AudioClip wrongAnswerSound;
    [SerializeField] private AudioClip questPopupSound;

    [Header("Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            PreloadUISounds(); // Preload UI sounds ?? gi?m lag l?n ??u
            
            // ??ng ký s? ki?n khi scene thay ??i
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnDestroy()
    {
        // H?y ??ng ký s? ki?n khi AudioManager b? destroy
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    // ???c g?i m?i khi scene m?i ???c load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"AudioManager: Scene '{scene.name}' ???c load");
        
        // Phát nh?c t??ng ?ng v?i scene
        if (scene.name.Contains("MainMenu") || scene.buildIndex == 0)
        {
            PlayMenuMusic();
        }
        else if (scene.name.Contains("Gameplay") || scene.name.Contains("SampleScene") || scene.buildIndex == 1)
        {
            PlayGameplayMusic();
        }
    }

    private void InitializeAudioSources()
    {
        // T?o AudioSource cho music n?u ch?a có
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        // T?o AudioSource cho SFX n?u ch?a có
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;

        // T?o AudioSource riêng cho UI SFX (gi?m lag)
        if (uiSfxSource == null)
        {
            uiSfxSource = gameObject.AddComponent<AudioSource>();
        }
        uiSfxSource.loop = false;
        uiSfxSource.playOnAwake = false;
        uiSfxSource.volume = sfxVolume;
        uiSfxSource.priority = 0; // ?u tiên cao nh?t ?? phát ngay l?p t?c
    }

    private void Start()
    {
        // Phát nh?c menu khi b?t ??u
        PlayMusic(menuMusic);
    }

    // Preload UI sounds ?? gi?m lag l?n ??u tiên phát
    private void PreloadUISounds()
    {
        if (uiSfxSource != null)
        {
            // Load các UI sounds vào memory
            if (buttonClickSound != null) uiSfxSource.clip = buttonClickSound;
            if (correctAnswerSound != null) _ = correctAnswerSound.loadState;
            if (wrongAnswerSound != null) _ = wrongAnswerSound.loadState;
            if (questPopupSound != null) _ = questPopupSound.loadState;
        }
    }

    #region Music Methods

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = true; // Menu music loop liên t?c
        }
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = true; // Gameplay music loop liên t?c
        }
        PlayMusic(gameplayMusic);
    }

    public void PlayWinMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = false; // Win music ch? phát 1 l?n
        }
        PlayMusic(winMusic);
    }

    public void PlayLoseMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = false; // Lose music ch? phát 1 l?n
        }
        PlayMusic(loseMusic);
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    #endregion

    #region SFX Methods

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
    }

    // Player sounds
    public void PlayPlayerWalk()
    {
        if (playerWalkSound != null && !sfxSource.isPlaying)
        {
            PlaySFX(playerWalkSound, 0.3f);
        }
    }

    public void PlayPlayerDamage()
    {
        PlaySFX(playerDamageSound);
    }

    public void PlayPlayerHeal()
    {
        PlaySFX(playerHealSound);
    }

    // Chest sounds
    public void PlayChestOpen()
    {
        PlaySFX(chestOpenSound);
    }

    public void PlayChestCollect()
    {
        PlaySFX(chestCollectSound);
    }

    public void PlayTrap()
    {
        PlaySFX(trapSound);
    }

    // UI sounds - T?i ?u ?? gi?m lag
    public void PlayButtonClick()
    {
        if (buttonClickSound != null && uiSfxSource != null)
        {
            uiSfxSource.PlayOneShot(buttonClickSound, sfxVolume);
        }
    }

    public void PlayCorrectAnswer()
    {
        if (correctAnswerSound != null && uiSfxSource != null)
        {
            uiSfxSource.PlayOneShot(correctAnswerSound, sfxVolume);
        }
    }

    public void PlayWrongAnswer()
    {
        if (wrongAnswerSound != null && uiSfxSource != null)
        {
            uiSfxSource.PlayOneShot(wrongAnswerSound, sfxVolume);
        }
    }

    public void PlayQuestPopup()
    {
        if (questPopupSound != null && uiSfxSource != null)
        {
            uiSfxSource.PlayOneShot(questPopupSound, sfxVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    #endregion

    #region Helper Methods

    public void MuteAll()
    {
        if (musicSource != null) musicSource.mute = true;
        if (sfxSource != null) sfxSource.mute = true;
        if (uiSfxSource != null) uiSfxSource.mute = true;
    }

    public void UnmuteAll()
    {
        if (musicSource != null) musicSource.mute = false;
        if (sfxSource != null) sfxSource.mute = false;
        if (uiSfxSource != null) uiSfxSource.mute = false;
    }

    public void ToggleMute()
    {
        if (musicSource != null && sfxSource != null && uiSfxSource != null)
        {
            bool isMuted = musicSource.mute;
            musicSource.mute = !isMuted;
            sfxSource.mute = !isMuted;
            uiSfxSource.mute = !isMuted;
        }
    }

    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    #endregion
}
