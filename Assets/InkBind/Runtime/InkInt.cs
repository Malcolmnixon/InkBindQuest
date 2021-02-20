using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    public enum InkIntState
    {
        [Tooltip("When the variable gets a new value.")]
        Any,

        [Tooltip("When the variable becomes equals to compare.")]
        Equals,

        [Tooltip("When the variable becomes not equals to compare.")]
        NotEquals,

        [Tooltip("When the variable becomes greater than compare.")]
        GreaterThan,

        [Tooltip("When the variable becomes less than compare.")]
        LessThan,

        [Tooltip("When the variable is initialized.")]
        Initial,

        [Tooltip("When the variable is initialized equals to compare.")]
        InitialEquals,

        [Tooltip("When the variable is initialized not equals to compare.")]
        InitialNotEquals,

        [Tooltip("When the variable is initialized greater than compare.")]
        InitialGreaterThan,

        [Tooltip("When the variable is initialized less than compare.")]
        InitialLessThan,

        [Tooltip("When the variable is changed at runtime.")]
        Change,

        [Tooltip("When the variable is changed at runtime to be equal to compare.")]
        ChangeEquals,

        [Tooltip("When the variable is changed at runtime to be not equal to compare.")]
        ChangeNotEquals,

        [Tooltip("When the variable is changed at runtime to be greater than compare.")]
        ChangeGreaterThan,

        [Tooltip("When the variable is changed at runtime to be less than compare.")]
        ChangeLessThan,
    }

    [Serializable]
    public class InkIntEvent : UnityEvent<int>
    {
    }

    [Serializable]
    public class InkIntOption
    {
        [Tooltip("State to trigger actions")]
        public InkIntState state;

        [Tooltip("Optional value used for comparison")]
        public int compare;

        [Tooltip("Actions to trigger when state occurs")]
        public InkIntEvent action;

        public void OnInitial(int value)
        {
            switch (state)
            {
                case InkIntState.Any:
                case InkIntState.Initial:
                    action.Invoke(value);
                    break;

                case InkIntState.Equals:
                case InkIntState.InitialEquals:
                    if (value == compare)
                        action.Invoke(value);
                    break;

                case InkIntState.NotEquals:
                case InkIntState.InitialNotEquals:
                    if (value != compare)
                        action.Invoke(value);
                    break;

                case InkIntState.GreaterThan:
                case InkIntState.InitialGreaterThan:
                    if (value > compare)
                        action.Invoke(value);
                    break;

                case InkIntState.LessThan:
                case InkIntState.InitialLessThan:
                    if (value > compare)
                        action.Invoke(value);
                    break;
            }
        }

        public void OnChange(int value)
        {
            switch (state)
            {
                case InkIntState.Any:
                case InkIntState.Change:
                    action.Invoke(value);
                    break;

                case InkIntState.Equals:
                case InkIntState.ChangeEquals:
                    if (value == compare)
                        action.Invoke(value);
                    break;

                case InkIntState.NotEquals:
                case InkIntState.ChangeNotEquals:
                    if (value != compare)
                        action.Invoke(value);
                    break;

                case InkIntState.GreaterThan:
                case InkIntState.ChangeGreaterThan:
                    if (value > compare)
                        action.Invoke(value);
                    break;

                case InkIntState.LessThan:
                case InkIntState.ChangeLessThan:
                    if (value > compare)
                        action.Invoke(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Ink int variable monitor
    /// </summary>
    public class InkInt : InkVariableMonitor
    {
        [Tooltip("Array of options")]
        public InkIntOption[] options;

        protected override void OnInitial(object initialValue)
        {
            // Get the value
            var value = Convert.ToInt32(initialValue);

            // Fire the options
            foreach (var option in options)
                option.OnInitial(value);
        }

        protected override void OnChange(object newValue)
        {
            // Get the value
            var value = Convert.ToInt32(newValue);

            // Fire the options
            foreach (var option in options)
                option.OnChange(value);
        }
    }
}
