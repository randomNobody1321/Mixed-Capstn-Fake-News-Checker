using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources (Auto-created if left empty)")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource voiceSource; // Dedicated channel so Granny's voice never cuts off SFX

    [Header("1. Background Music (1 Clip)")]
    public AudioClip bgMusicClip;               // Drag 'BG Muisc' here

    [Header("2. General UI & System SFX (4 Clips)")]
    public AudioClip buttonClickClip;           // Drag 'Button click sound' here
    public AudioClip buttonHoverClip;           // Drag 'Button hover sound' here
    public AudioClip onboardingClickClip;       // Drag 'Onboarding sound per click' here
    public AudioClip helperClickClip;           // Drag 'Helper click sound' here

    [Header("3. Granny Answer Reactions (4 Clips)")]
    public AudioClip grannyRightAnswerA;        // Drag 'Granny right Answer A' here
    public AudioClip grannyRightAnswerB;        // Drag 'Granny Right Answer B' here
    public AudioClip grannyWrongOptionA;        // Drag 'Granny Wrong Option A (1)' here
    public AudioClip grannyWrongOptionB;        // Drag 'Granny Wrong Option B' here

    [Header("4. End of Day & Final Endings (7 Clips)")]
    public AudioClip dayEndSoundClip;           // Drag 'Day End sound' here
    public AudioClip lowReliabilityEndClip;     // Drag 'Low Reliability Endgame' here
    public AudioClip lowReliabilityGrannyClip;  // Drag 'Low reliability granny endgame' here
    public AudioClip midReliabilityEndClip;     // Drag 'Mid Reliability Endgame' here
    public AudioClip midReliabilityGrannyClip;  // Drag 'Mid Reliablity Endgame granny' here
    public AudioClip highReliabilityEndClip;    // Drag 'High Reliability end game' here
    public AudioClip highReliabilityGrannyClip; // Drag 'High Reliability Granny Endgame' here

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
        AttachHoverAndClickSoundsToAllSceneButtons();
    }

    private void SetupAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = 0.45f;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (voiceSource == null)
        {
            voiceSource = gameObject.AddComponent<AudioSource>();
            voiceSource.loop = false;
            voiceSource.playOnAwake = false;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (bgmSource != null && bgMusicClip != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = bgMusicClip;
            bgmSource.Play();
        }
    }

    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickClip != null)
            sfxSource.PlayOneShot(buttonClickClip);
    }

    public void PlayButtonHover()
    {
        if (sfxSource != null && buttonHoverClip != null)
            sfxSource.PlayOneShot(buttonHoverClip);
    }

    public void PlayOnboardingClick()
    {
        if (sfxSource != null && onboardingClickClip != null)
            sfxSource.PlayOneShot(onboardingClickClip);
    }

    public void PlayHelperClick()
    {
        if (sfxSource != null && helperClickClip != null)
            sfxSource.PlayOneShot(helperClickClip);
    }

    public void PlayGrannyRightAnswer()
    {
        AudioClip chosen = (Random.value < 0.5f) ? grannyRightAnswerA : grannyRightAnswerB;
        if (chosen == null) chosen = grannyRightAnswerA != null ? grannyRightAnswerA : grannyRightAnswerB;

        if (voiceSource != null && chosen != null)
        {
            voiceSource.Stop();
            voiceSource.PlayOneShot(chosen);
        }
    }

    public void PlayGrannyWrongAnswer()
    {
        AudioClip chosen = (Random.value < 0.5f) ? grannyWrongOptionA : grannyWrongOptionB;
        if (chosen == null) chosen = grannyWrongOptionA != null ? grannyWrongOptionA : grannyWrongOptionB;

        if (voiceSource != null && chosen != null)
        {
            voiceSource.Stop();
            voiceSource.PlayOneShot(chosen);
        }
    }

    public void PlayDayEndSound()
    {
        if (sfxSource != null && dayEndSoundClip != null)
            sfxSource.PlayOneShot(dayEndSoundClip);
    }

    public void PlayEndingAudio(int finalScore)
    {
        if (voiceSource != null) voiceSource.Stop();

        if (finalScore <= 50) // Low Reliability Ending
        {
            if (sfxSource != null && lowReliabilityEndClip != null)
                sfxSource.PlayOneShot(lowReliabilityEndClip);
            if (voiceSource != null && lowReliabilityGrannyClip != null)
                voiceSource.PlayOneShot(lowReliabilityGrannyClip);
        }
        else if (finalScore <= 100) // Mid Reliability Ending
        {
            if (sfxSource != null && midReliabilityEndClip != null)
                sfxSource.PlayOneShot(midReliabilityEndClip);
            if (voiceSource != null && midReliabilityGrannyClip != null)
                voiceSource.PlayOneShot(midReliabilityGrannyClip);
        }
        else // High Reliability Ending
        {
            if (sfxSource != null && highReliabilityEndClip != null)
                sfxSource.PlayOneShot(highReliabilityEndClip);
            if (voiceSource != null && highReliabilityGrannyClip != null)
                voiceSource.PlayOneShot(highReliabilityGrannyClip);
        }
    }

    // Automatically wires Hover & Click SFX to every UI Button in the scene (including inactive panels)
    public void AttachHoverAndClickSoundsToAllSceneButtons()
    {
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in allButtons)
        {
            if (btn.gameObject.scene.IsValid())
            {
                RegisterButtonAudio(btn);
            }
        }
    }

    // Call this on dynamically spawned feed article row buttons too
    public void RegisterButtonAudio(Button btn, bool skipStandardClickSound = false)
    {
        if (btn == null) return;

        // Prevent duplicate registration
        if (btn.GetComponent<UIButtonAudioHook>() != null) return;
        UIButtonAudioHook hook = btn.gameObject.AddComponent<UIButtonAudioHook>();

        if (!skipStandardClickSound)
        {
            btn.onClick.AddListener(PlayButtonClick);
        }

        EventTrigger trigger = btn.GetComponent<EventTrigger>();
        if (trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        hoverEntry.callback.AddListener((_) =>
        {
            if (btn.interactable) PlayButtonHover();
        });
        trigger.triggers.Add(hoverEntry);
    }
}

public class UIButtonAudioHook : MonoBehaviour { }
