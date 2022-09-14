using UnityEngine;
using Viroo.Interaction;

namespace Viroo.Examples.Actions
{
    public class RandomColorAction : BroadcastObjectAction
    {
        [SerializeField]
        private Renderer cubeRenderer = default;

        public override void LocalExecute(string data)
        {
            cubeRenderer.material.color = Color.red;
        }
    }
}