using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SpeechEntry
{
    [Tooltip("Name of the speech.")]
    public string name;

    [Tooltip("Audio clip to play.")]
    public AudioClip clip;

    [Tooltip("Event to fire when this speech has finished.")]
    public UnityEvent onFinish;
}

[RequireComponent(typeof(AudioSource))]
public class SpeechManager : MonoBehaviour
{
    [Tooltip("Array of speeches.")]
    public SpeechEntry[] _speeches;

    [Tooltip("Event to fire on the start of any speech.")]
    public UnityEvent onStart;

    [Tooltip("Event to fire on the end of any speech.")]
    public UnityEvent onFinish;

    /// <summary>
    /// Audio source
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Current speech (if any)
    /// </summary>
    private SpeechEntry _currentSpeech;

    /// <summary>
    /// Play a speech
    /// </summary>
    /// <param name="speechName">Name of speech</param>
    public void Play(string speechName)
    {
        // Stop current playback
        _audioSource.Stop();

        // Get the new speech
        _currentSpeech = _speeches.FirstOrDefault(s => s.name == speechName);

        // Start playing
        if (_currentSpeech != null)
        {
            _audioSource.PlayOneShot(_currentSpeech.clip);
            onStart.Invoke();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get the audio source. Don't try to dynamically create as there
        // are too many settings.
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Skip if no current speech
        if (_currentSpeech == null)
            return;

        // Skip if still playing
        if (_audioSource.isPlaying)
            return;

        // Detach finished speech
        var finished = _currentSpeech;
        _currentSpeech = null;

        // Fire finished events
        finished.onFinish.Invoke();
        onFinish.Invoke();
    }
}
