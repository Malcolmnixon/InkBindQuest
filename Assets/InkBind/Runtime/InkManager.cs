using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    public class InkManager : MonoBehaviour
    {
        /// <summary>
        /// Json asset for story
        /// </summary>
        [Tooltip("Ink JSON story.")]
        public TextAsset inkJsonAsset;

        /// <summary>
        /// Story instance
        /// </summary>
        private static Story _inkStory;

        /// <summary>
        /// Story advanced event
        /// </summary>
        public UnityEvent onAdvance;

        /// <summary>
        /// Gets (creating if necessary) the ink story instance
        /// </summary>
        public Story Story
        {
            get
            {
                EnsureStory(inkJsonAsset.text);
                return _inkStory;
            }
        }

        public void Continue(int choice = 0)
        {
            do
            {
                // If the story needs a choice then make the choice
                if (_inkStory.currentChoices.Any())
                {
                    Debug.Log($"InkManager: Making choice {_inkStory.currentChoices[choice].text}");
                    _inkStory.ChooseChoiceIndex(choice);
                }

                // If the story can continue then continue
                if (_inkStory.canContinue)
                {
                    Debug.Log("InkManager: Continuing story");
                    _inkStory.Continue();
                }

                Debug.Log($"InkManager: Dialog: {_inkStory.currentText}");

                // Dispatch all tags
                foreach (var inkTag in _inkStory.currentTags)
                    foreach (var handler in FindObjectsOfType<InkTag>())
                        handler.OnTag(inkTag);

                // Report story advanced
                onAdvance.Invoke();
            } while (_inkStory.canContinue && string.IsNullOrEmpty(_inkStory.currentText.Trim()));
        }

        public void Continue(string choice)
        {
            var index = _inkStory.currentChoices.FindIndex(c => c.text == choice);
            if (index >= 0)
                Continue(index);
        }

        private void Start()
        {
            EnsureStory(inkJsonAsset.text);

            Continue();
        }

        private static void EnsureStory(string text)
        {
            // Skip if we have a story
            if (_inkStory != null)
                return;

            // Construct the story
            _inkStory = new Story(text);
        }
    }
}
