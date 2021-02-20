using Ink.Runtime;
using UnityEngine;

namespace Assets.InkBind.Runtime
{
    /// <summary>
    /// Base class for Ink variable monitor
    /// </summary>
    public abstract class InkVariableMonitor : InkClient
    {
        [Tooltip("Name of the Ink variable to monitor.")]
        public string variableName;

        protected override void Start()
        {
            base.Start();

            // Observe the variable
            inkStory.ObserveVariable(variableName, Observer);

            // Fire the initial value
            OnInitial(inkStory.variablesState[variableName]);
        }

        protected virtual void OnDestroy()
        {
            inkStory.RemoveVariableObserver(Observer);
        }

        /// <summary>
        /// Observer function called when the ink variable changes
        /// </summary>
        /// <param name="varName">Ink variable name</param>
        /// <param name="newValue">New value</param>
        private void Observer(string varName, object newValue)
        {
            OnChange(newValue);
        }

        /// <summary>
        /// Handler for the initial Ink value
        /// </summary>
        /// <param name="initialValue">Initial value</param>
        protected abstract void OnInitial(object initialValue);

        /// <summary>
        /// Handler for Ink variable change
        /// </summary>
        /// <param name="newValue">New value</param>
        protected abstract void OnChange(object newValue);
    }
}
