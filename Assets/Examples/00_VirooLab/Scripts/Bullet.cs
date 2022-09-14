using Networking.Messages.Senders;
using System.Collections;
using UnityEngine;
using Virtualware.Networking.Client;

namespace VirooLab.examples
{
    public class Bullet : MonoBehaviour
    {
        private SessionObjectMessageSender sessionObjectMessageSender;

        private bool injectionDone;

        private void Awake()
        {
            this.QueueForInject();
        }

        protected void Inject(SessionObjectMessageSender sessionObjectMessageSender)
        {
            this.sessionObjectMessageSender = sessionObjectMessageSender;

            var rigid = GetComponent<Rigidbody>();
            rigid.AddForce(transform.forward * 1000, ForceMode.Acceleration);

            injectionDone = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(NetworkDestroy());
        }

        IEnumerator NetworkDestroy()
        {
            while (!injectionDone)
            {
                yield return null;
            }

            var networkObject = GetComponent<NetworkObject>();
            sessionObjectMessageSender.DestroyObject(networkObject.ObjectId);
        }
    }
}
