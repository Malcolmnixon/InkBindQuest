using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    /// <summary>
    /// Choices for ink boolean states
    /// </summary>
    public enum InkTagState
    {
        [Tooltip("When the tag fires with any value.")]
        Any,

        [Tooltip("When the tag fires with a value equals to 'pattern'.")]
        Equals,

        [Tooltip("When the tag fires with any value other than 'pattern'.")]
        NotEquals,

        [Tooltip("When the tag fires with a value starting with 'pattern'.")]
        StartsWith,

        [Tooltip("When the tag fires with a value matching the regex specified in 'pattern'.")]
        MatchesRegex,
    }

    [Serializable]
    public class InkTagEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class InkTagOption
    {
        [Tooltip("State to trigger actions")]
        public InkTagState state;

        [Tooltip("Optional value used for comparison")]
        public string pattern;

        [Tooltip("Actions to trigger when state occurs")]
        public InkTagEvent action;

        public void OnTag(string inkTag)
        {
            switch (state)
            {
                case InkTagState.Any:
                    action.Invoke(inkTag);
                    break;

                case InkTagState.Equals:
                    if (inkTag == pattern)
                        action.Invoke(inkTag);
                    break;

                case InkTagState.NotEquals:
                    if (inkTag != pattern)
                        action.Invoke(inkTag);
                    break;

                case InkTagState.StartsWith:
                    if (inkTag.StartsWith(pattern))
                        action.Invoke(inkTag);
                    break;

                case InkTagState.MatchesRegex:
                    if (Regex.IsMatch(inkTag, pattern))
                        action.Invoke(inkTag);
                    break;
            }
        }
    }

    /// <summary>
    /// Ink Tag monitor class
    /// </summary>
    public class InkTag : MonoBehaviour
    {
        [Tooltip("Array of options")]
        public InkTagOption[] options;

        public void OnTag(string inkTag)
        {
            // Fire the options
            foreach (var option in options)
                option.OnTag(inkTag);
        }
    }
}
