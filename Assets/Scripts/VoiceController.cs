using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VoiceController : MonoBehaviour
{
    [System.Serializable]
    public class VoiceClip
    {
        [SerializeField] string key;
        [SerializeField] AudioClip clip;

        public string getKey()
        {
            return key;
        }

        public AudioClip getAudioClip()
        {
            return clip;
        }
    }

    [System.Serializable]
    public class Voice
    {
        [SerializeField] string voice;
        [SerializeField] VoiceClip[] voiceClips;

        public VoiceClip FindVoiceClip(string clipKey)
        {
            foreach (VoiceClip clip in voiceClips)
            {
                if (clip.getKey() == clipKey)
                {
                    return clip;
                }
            }

            return null;
        }

        public VoiceClip[] getVoiceClips()
        {
            return voiceClips;
        }
    }


    [SerializeField] private KeyPressListener keyPressHandler;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private Voice[] voices;

    AudioSource audioSource;
    Voice correctVoice;
    Voice currentVoice;
    VoiceClip currentVoiceClip;
    int points = 0;
    const int CLIP_ITERATIONS = 5;
    int clipsPlayed = CLIP_ITERATIONS;
    bool keyPressed;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

        // Subscribe to the events
        KeyPressListener.OnLeftArrowPressed += HandleLeftArrowPressed;
        KeyPressListener.OnRightArrowPressed += HandleRightArrowPressed;

        StartCoroutine(PlayIntro());
    }

    void OnDisable()
    {
        // Unsubscribe from the events to avoid memory leaks
        KeyPressListener.OnLeftArrowPressed -= HandleLeftArrowPressed;
        KeyPressListener.OnRightArrowPressed -= HandleRightArrowPressed;
    }

    void HandleLeftArrowPressed()
    {
        keyPressHandler.enabled = false;
        keyPressed = true;

        if (currentVoiceClip.getKey() == "Left" && currentVoice == correctVoice)
        {
            points++;
            StartCoroutine(CorrectAudio());
        }
        else
        {
            StaticData.FinalPoints = points;
            SceneManager.LoadScene("EndScene");
        }
    }

    void HandleRightArrowPressed()
    {
        keyPressHandler.enabled = false;
        keyPressed = true;

        if (currentVoiceClip.getKey() == "Right" && currentVoice == correctVoice)
        {
            points++;
            StartCoroutine(CorrectAudio());
        }
        else
        {
            StaticData.FinalPoints = points;
            SceneManager.LoadScene("EndScene");
        }
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);

        audioSource.panStereo = 0;
        correctVoice = voices[Random.Range(0, voices.Length)];
        currentVoiceClip = correctVoice.FindVoiceClip("MyTurn");
        audioSource.clip = currentVoiceClip.getAudioClip();
        audioSource.Play();

        yield return new WaitForSeconds(2f);

        StartCoroutine(PlayDirection());
    }

    IEnumerator PlayDirection()
    {
        PickEar();
        currentVoice = voices[Random.Range(0, voices.Length)];
        currentVoiceClip = PickVoiceClip(currentVoice);
        audioSource.clip = currentVoiceClip.getAudioClip();
        audioSource.Play();
        clipsPlayed--;
        keyPressHandler.enabled = true;

        yield return new WaitForSeconds(1.2f);

        if (!keyPressed && currentVoice == correctVoice)
        {
            StaticData.FinalPoints = points;
            SceneManager.LoadScene("EndScene");
        }
        else if (keyPressed) yield return new WaitUntil(() => keyPressed == false);
        else points++;

        if (clipsPlayed <= 0)
        {
            clipsPlayed = CLIP_ITERATIONS;
            StartCoroutine(PlayIntro());
        }
        else StartCoroutine(PlayDirection());
    }

    IEnumerator CorrectAudio()
    {
        audioSource.panStereo = 0;
        audioSource.clip = correctSound;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);
        keyPressed = false;
    }

    /*IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(1f);

        currentVoiceClip = correctVoice.FindVoiceClip("MyTurn");
        audioSource.clip = currentVoiceClip.getAudioClip();
        audioSource.Play();

        yield return new WaitForSeconds(3f);

        for (int i = 1; i <= 3; i++)
        {
            currentVoice = voices[Random.Range(0, voices.Length)];
            currentVoiceClip = PickVoiceClip(currentVoice);
            audioSource.clip = currentVoiceClip.getAudioClip();
            audioSource.Play();
            keyPressHandler.enabled = true;

            yield return new WaitForSeconds(1f);

            if (currentVoice == correctVoice && !keyPressed) SceneManager.LoadScene("EndScene");
            keyPressed = false;
        }

        StartCoroutine(PlayAudio());
    }*/

    VoiceClip PickVoiceClip(Voice voice)
    {
        string direction = Random.Range(0, 2) == 0 ? "Left" : "Right";
        VoiceClip voiceClip = voice.FindVoiceClip(direction);

        return voiceClip;
    }

    void PickEar()
    {
        int ear = Random.Range(0, 2) == 0 ? -1 : 1;
        audioSource.panStereo = ear;
    }
}
