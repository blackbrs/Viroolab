using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Viroo.Arena;
using Viroo.Context;
using Viroo.Interaction;
using Viroo.Networking;

namespace VirooLab.Examples
{
    public class Lift : MonoBehaviour
    {
        [SerializeField]
        private Collider buttonUp = default;

        [SerializeField]
        private Collider buttonDown = default;

        [SerializeField]
        private Material lightRed = default;

        [SerializeField]
        private Material lightGreen = default;

        [SerializeField]
        private Material buttonRed = default;

        [SerializeField]
        private Material buttonGreen = default;

        [SerializeField]
        private MeshRenderer buttonUpDisplayRenderer = default;

        [SerializeField]
        private MeshRenderer buttonDownDisplayRenderer = default;

        [SerializeField]
        private MeshRenderer lightRenderer = default;

        [SerializeField]
        private ObjectAction playBeepAction = default;

        private IContextProvider contextProvider;
        private readonly List<string> playersInLift = new List<string>();

        private bool allPlayersIn;

        private bool liftIsDown = true;

        private void Awake()
        {
            this.QueueForInject();
        }

        void Inject(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void SetLiftState(bool isDown)
        {
            liftIsDown = isDown;
        }

        private void SetState()
        {
            if (liftIsDown)
            {
                SetButtonUp(allPlayersIn);
            }
            else
            {
                SetButtonDown(allPlayersIn);
            }

            lightRenderer.material = allPlayersIn ? lightGreen : lightRed;

            if (allPlayersIn)
            {
                playBeepAction.LocalExecute(string.Empty);
            }
        }

        public void SetButtonUp(bool enable)
        {
            buttonUp.enabled = enable;
            buttonUpDisplayRenderer.material = enable ? buttonGreen : buttonRed;
        }

        public void SetButtonDown(bool enable)
        {
            buttonDown.enabled = enable;
            buttonDownDisplayRenderer.material = enable ? buttonGreen : buttonRed;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<IPlayer>();

            if (player != null)
            {
                var arena = GetArena(player.ArenaId);
                if (arena != null)
                {
                    arena.SetParent(transform);

                    if (!playersInLift.Contains(player.ClientId))
                    {
                        playersInLift.Add(player.ClientId);
                        Debug.Log("Add Player To Lift: " + player.ClientId);
                    }

                    CheckAllPlayersIn(arena);
                }
            }
        }

        private void CheckAllPlayersIn(Transform arena)
        {
            var playersInTheSameArena = arena.GetComponentsInChildren<IPlayer>().Select(p => p.ClientId);

            allPlayersIn = playersInTheSameArena.All(player => playersInLift.Contains(player));

            SetState();
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<IPlayer>();

            if (player != null)
            {
                var arena = GetArena(player.ArenaId);
                if (arena != null)
                {
                    arena.SetParent(contextProvider.ArenaRoot.transform);

                    if (playersInLift.Contains(player.ClientId))
                    {
                        playersInLift.Remove(player.ClientId);
                        Debug.Log("Remove Player From Lift: " + player.ClientId);
                    }
                }

                CheckAllPlayersIn(arena);
            }
        }

        private Transform GetArena(string arenaId)
        {
            var arenaNodeRoot = contextProvider.ArenaRoot;
            var arenaNodePool = arenaNodeRoot.GetComponent<InternalArenaNodePool>();
            var arenaNode = arenaNodePool.GetArena(arenaId);

            return arenaNode.transform;
        }
    }
}
