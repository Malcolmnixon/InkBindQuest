using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.InkBind.Runtime
{
    /// <summary>
    /// Choices for ink boolean states
    /// </summary>
    public enum InkBoolState
    {
        [Tooltip("When the variable gets a new value.")]
        Any,

        [Tooltip("When the variable becomes 'true'.")]
        True,

        [Tooltip("When the variable becomes 'false'.")]
        False,

        [Tooltip("When the variable is initialized.")]
        Initial,

        [Tooltip("When the variable is initialized 'true'.")]
        InitialTrue,

        [Tooltip("When the variable is initialized 'false'.")]
        InitialFalse,

        [Tooltip("When the variable is changed at runtime.")]
        Change,

        [Tooltip("When the variable is changed at runtime to 'true'.")]
        ChangeTrue,

        [Tooltip("When the variable is changed at runtime to 'false'.")]
        ChangeFalse
    }

    [Serializable]
    public class InkBoolEvent : UnityEvent<bool>
    {
    }

    [Serializable]
    public class InkBoolOption
    {
        [Tooltip("State to trigger actions")]
        public InkBoolState state;

        [Tooltip("Actions to trigger when state occurs")]
        public InkBoolEvent action;

        public void OnInitial(bool value)
        {
            switch (state)
            {
                case InkBoolState.Any:
                case InkBoolState.Initial:
                    action.Invoke(value);
                    break;

                case InkBoolState.True:
                case InkBoolState.InitialTrue:
                    if (value)
                        action.Invoke(true);
                    break;

                case InkBoolState.False:
                case InkBoolState.InitialFalse:
                    if (!value)
                        action.Invoke(false);
                    break;
            }
        }

        public void OnChange(bool value)
        {
            switch (state)
            {
                case InkBoolState.Any:
                case InkBoolState.Change:
                    action.Invoke(value);
                    break;

                case InkBoolState.True:
                case InkBoolState.ChangeTrue:
                    if (value)
                        action.Invoke(true);
                    break;

                case InkBoolState.False:
                case InkBoolState.ChangeFalse:
                    if (!value)
                        action.Invoke(false);
                    break;
            }
        }
    }

    /// <summary>
    /// Ink bool variable monitor
    /// </summary>
    public class InkBool : InkVariableMonitor
    {
        [Tooltip("Array of options")]
        public InkBoolOption[] options;

        protected override void OnInitial(object initialValue)
        {
            // Get the value
            var value = Convert.ToBoolean(initialValue);

            // Fire the options
            foreach (var option in options)
                option.OnInitial(value);
        }

        protected override void OnChange(object newValue)
        {
            // Get the value
            var value = Convert.ToBoolean(newValue);

            // Fire the options
            foreach (var option in options)
                option.OnChange(value);
        }
    }
}
