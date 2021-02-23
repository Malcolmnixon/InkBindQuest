using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    public enum InkAnimationPropertyOption
    {
        SetTrigger,
        ResetTrigger,
        SetBool,
        ClearBool,
        Play
    };

    [Serializable]
    public class InkCharacterAnimation
    {
        [Tooltip("Name of the character animation in the Ink story tag.")]
        public string inkName;

        [Tooltip("Character animator property to modify.")]
        public string animatorProperty;

        [Tooltip("Animator property option.")]
        public InkAnimationPropertyOption option;
    }

    [Serializable]
    public class InkCharacterSpeech
    {
        [Tooltip("Name of the speech in the Ink story tag.")]
        public string inkName;

        [Tooltip("Audio clip to play.")]
        public AudioClip clip;
    }

    public class InkCharacter : InkClient
    {
        [Tooltip("Name of the character in the Ink story tags.")]
        public string inkName;

        [Tooltip("Name of the character in speech bubbles.")]
        public string speechName;

        [Tooltip("Transform for locating the chat canvas.")]
        public Transform canvasTransform;

        [Tooltip("Ink canvas to use for chatting.")]
        public InkCanvas chatCanvas;

        [Tooltip("Animator for character animations.")]
        public Animator animator;

        [Tooltip("Audio source for speech.")]
        public AudioSource audio;

        [Tooltip("Character animations.")]
        public InkCharacterAnimation[] animations;

        [Tooltip("Character speeches.")]
        public InkCharacterSpeech[] speeches;

        [Tooltip("Event when character starts chatting.")]
        public UnityEvent chatStart;

        [Tooltip("Event when character stops chatting.")]
        public UnityEvent chatStop;

        /// <summary>
        /// Current speech (if any)
        /// </summary>
        private InkCharacterSpeech _currentSpeech;

        /// <summary>
        /// Attempt to trigger a chat with the character
        /// </summary>
        public void TriggerInkChat()
        {
            // Attempt to continue the story with the choice to chat with the character
            inkManager.Continue($"chat:{inkName}");
        }

        public void OnChatStart()
        {
            chatCanvas.speaker.text = speechName;
            chatCanvas.ShowCanvasAt(canvasTransform);
            chatStart.Invoke();
        }

        public void OnChatStop()
        {
            chatCanvas.HideCanvas();
            chatStop.Invoke();
        }

        public void OnTag(string value)
        {
            // Split the tag into key:value
            var items = value.Split(new[] {':'}, 2);
            if (items.Length != 2)
                return;

            switch (items[0])
            {
                case "animate":
                    OnAnimate(items[1]);
                    break;

                case "says":
                    OnSpeech(items[1]);
                    break;
            }
        }

        public void OnAnimate(string value)
        {
            // Get the animation entry
            var entry = animations.FirstOrDefault(a => a.inkName == value);
            if (entry == null)
                return;

            switch (entry.option)
            {
                case InkAnimationPropertyOption.SetTrigger:
                    animator.SetTrigger(entry.animatorProperty);
                    break;

                case InkAnimationPropertyOption.ResetTrigger:
                    animator.ResetTrigger(entry.animatorProperty);
                    break;

                case InkAnimationPropertyOption.SetBool:
                    animator.SetBool(entry.animatorProperty, true);
                    break;

                case InkAnimationPropertyOption.ClearBool:
                    animator.SetBool(entry.animatorProperty, false);
                    break;

                case InkAnimationPropertyOption.Play:
                    animator.Play(entry.animatorProperty);
                    break;
            }
        }

        public void OnSpeech(string value)
        {
            // Get the speech entry
            var entry = speeches.FirstOrDefault(s => s.inkName == value);
            if (entry == null)
                return;

            // Start playing the speech
            _currentSpeech = entry;
            chatCanvas.DisableChoices();
            audio.PlayOneShot(_currentSpeech.clip);
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            inkManager.RegisterCharacter(this);
        }

        protected virtual void Update()
        {
            // Skip if no speech
            if (_currentSpeech == null)
                return;

            // Skip if still playing
            if (audio.isPlaying)
                return;

            // Enable choices at the end of the speech
            _currentSpeech = null;
            chatCanvas.EnableChoices();
        }

        protected virtual void OnDestroy()
        {
            inkManager.UnregisterCharacter(this);
        }
    }
}