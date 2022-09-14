using UnityEngine;
using Viroo.Interaction;
using Virtualware.Serialization;

namespace Viroo.Examples.Additional.Actions
{
    public class RandomPositionAction : BroadcastObjectAction
    {
        [SerializeField]
        private float radius = 1;

        private Vector3 initialPosition;
        private ISerializer serializer;

        protected void Inject(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.position;
        }

        public override void Execute(string data)
        {
            Vector3 target = Random.insideUnitSphere * radius;
            serializer.Serialize(target, out string serialized);
            base.Execute(serialized);
        }

        public override void LocalExecute(string data)
        {
            Vector3 target = serializer.Deserialize<Vector3>(System.Text.Encoding.UTF8.GetBytes(data));
            transform.position = initialPosition + target;
        }
    }
}