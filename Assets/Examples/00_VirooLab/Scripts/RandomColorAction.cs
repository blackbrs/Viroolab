using UnityEngine;
using Viroo.Interaction;

namespace Viroo.Examples.Actions
{
    public class RandomColorAction : BroadcastObjectAction
    {
        [SerializeField]
        private Renderer cubeRenderer = default;

        [SerializeField]
        private Color color = default;

        public override void LocalExecute(string data)
        {
            color = Random.ColorHSV();
            cubeRenderer.material.color = color;
        }
    }
}