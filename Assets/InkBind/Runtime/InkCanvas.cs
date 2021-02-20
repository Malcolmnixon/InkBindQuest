using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.InkBind.Runtime
{
    public class InkCanvas : InkClient
    {
        [Tooltip("Chat canvas to show/hide on request.")]
        public GameObject canvas;

        [Tooltip("Component in the chat canvas holding the dialog text.")]
        public TMP_Text dialog;

        [Tooltip("Component in the chat canvas holding the choice buttons.")]
        public GameObject choiceParent;

        [Tooltip("Template choice button")]
        public Button choiceTemplate;

        private readonly List<Button> _choiceButtons = new List<Button>();

        private readonly List<TMP_Text> _choiceText = new List<TMP_Text>();

        private bool _disableChoices;


        protected override void Start()
        {
            base.Start();

            // Initially hide canvas
            canvas.SetActive(false);

            // Subscribe to continues
            inkManager.onAdvance.AddListener(UpdateCanvas);
        }

        protected virtual void OnDestroy()
        {
            // Unsubscribe from continues
            inkManager.onAdvance.RemoveListener(UpdateCanvas);
        }

        public void ShowCanvasAt(Transform target)
        {
            transform.parent = target;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            canvas.SetActive(true);
        }

        public void HideCanvas()
        {
            canvas.SetActive(false);
        }

        public void EnableChoices()
        {
            _disableChoices = false;
            UpdateCanvas();
        }

        public void DisableChoices()
        {
            _disableChoices = true;
            UpdateCanvas();
        }

        private void UpdateCanvas()
        {
            // Get the array of choices
            string[] choices;
            if (_disableChoices)
                choices = new string[] { };
            else if (inkStory.currentChoices.Count != 0)
                choices = inkStory.currentChoices.Select(c => c.text).ToArray();
            else if (inkStory.canContinue)
                choices = new[] {"Continue ..."};
            else
                choices = new string[] { };

            // Set the dialog text
            dialog.text = inkStory.currentText;

            // Create as many buttons as needed
            while (_choiceButtons.Count < choices.Length)
            {
                // Create the new button and get its text
                var newButton = Instantiate(choiceTemplate, choiceParent.transform);
                var newText = newButton.GetComponentInChildren<TMP_Text>();

                // Have the button continue the Ink story
                var index = _choiceButtons.Count; // Create temporary for lambda-capture
                newButton.onClick.AddListener(() => { inkManager.Continue(index); });

                // Add to the lists
                _choiceButtons.Add(newButton);
                _choiceText.Add(newText);
            }

            // Enable and set the choices needed to display
            for (var i = 0; i < choices.Length; ++i)
            {
                _choiceText[i].text = choices[i];
                _choiceButtons[i].gameObject.SetActive(true);
            }

            // Disable all remaining buttons
            for (var i = choices.Length; i < _choiceButtons.Count; ++i)
                _choiceButtons[i].gameObject.SetActive(false);
        }
    }
}
