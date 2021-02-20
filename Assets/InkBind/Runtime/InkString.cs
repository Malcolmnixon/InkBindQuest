using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    public enum InkStringState
    {
        [Tooltip("When the variable gets a new value.")]
        Any,

        [Tooltip("When the variable becomes equals to 'pattern'.")]
        Equals,

        [Tooltip("When the variable becomes not equals to 'pattern'.")]
        NotEquals,

        [Tooltip("When the variable matches the regex specified in 'pattern'.")]
        MatchesRegex,

        [Tooltip("When the variable is initialized.")]
        Initial,

        [Tooltip("When the variable is initialized equals to 'pattern'.")]
        InitialEquals,

        [Tooltip("When the variable is initialized not equals to 'pattern'.")]
        InitialNotEquals,

        [Tooltip("When the variable is initialized matching regex specified in 'pattern'.")]
        InitialMatchesRegex,

        [Tooltip("When the variable changes at runtime.")]
        Change,

        [Tooltip("When the variable changes at runtime equals to 'pattern'.")]
        ChangeEquals,

        [Tooltip("When the variable changes at runtime not equals to 'pattern'.")]
        ChangeNotEquals,

        [Tooltip("When the variable changes at runtime matching the regex specified in 'pattern'.")]
        ChangeMatchesRegex
    }

    [Serializable]
    public class InkStringEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class InkStringOption
    {
        [Tooltip("State to trigger actions")]
        public InkStringState state;

        [Tooltip("Optional value used for comparison")]
        public string pattern;

        [Tooltip("Actions to trigger when state occurs")]
        public InkStringEvent action;

        public void OnInitial(string value)
        {
            switch (state)
            {
                case InkStringState.Any:
                case InkStringState.Initial:
                    action.Invoke(value);
                    break;

                case InkStringState.Equals:
                case InkStringState.InitialEquals:
                    if (value == pattern)
                        action.Invoke(value);
                    break;

                case InkStringState.NotEquals:
                case InkStringState.InitialNotEquals:
                    if (value != pattern)
                        action.Invoke(value);
                    break;

                case InkStringState.MatchesRegex:
                case InkStringState.InitialMatchesRegex:
                    if (Regex.IsMatch(value, pattern))
                        action.Invoke(value);
                    break;
            }
        }

        public void OnChange(string value)
        {
            switch (state)
            {
                case InkStringState.Any:
                case InkStringState.Change:
                    action.Invoke(value);
                    break;

                case InkStringState.Equals:
                case InkStringState.ChangeEquals:
                    if (value == pattern)
                        action.Invoke(value);
                    break;

                case InkStringState.NotEquals:
                case InkStringState.ChangeNotEquals:
                    if (value != pattern)
                        action.Invoke(value);
                    break;

                case InkStringState.MatchesRegex:
                case InkStringState.ChangeMatchesRegex:
                    if (Regex.IsMatch(value, pattern))
                        action.Invoke(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Ink string variable monitor
    /// </summary>
    public class InkString : InkVariableMonitor
    {
        [Tooltip("Array of options")]
        public InkStringOption[] options;

        protected override void OnInitial(object initialValue)
        {
            // Get the value
            var value = Convert.ToString(initialValue);

            // Fire the options
            foreach (var option in options)
                option.OnInitial(value);
        }

        protected override void OnChange(object newValue)
        {
            // Get the value
            var value = Convert.ToString(newValue);

            // Fire the options
            foreach (var option in options)
                option.OnChange(value);
        }
    }
}
