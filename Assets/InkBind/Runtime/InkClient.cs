using Ink.Runtime;
using UnityEngine;

namespace Assets.InkBind.Runtime
{
    public class InkClient : MonoBehaviour
    {
        /// <summary>
        /// Ink manager
        /// </summary>
        protected InkManager inkManager;

        /// <summary>
        /// Ink story
        /// </summary>
        protected Story inkStory;

        protected virtual void Start()
        {
            // Get the ink manager and story
            inkManager = FindObjectOfType<InkManager>();
            inkStory = inkManager?.Story;
        }
    }
}
