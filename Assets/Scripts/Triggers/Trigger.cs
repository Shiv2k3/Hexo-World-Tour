using Core.Player;
using UnityEngine;

namespace Core.Triggers
{
    public class Trigger : MonoBehaviour
    {
        protected Character player;
        protected Character lastPlayer;
        private void OnTriggerEnter(Collider other)
        {
            player = other.gameObject.GetComponent<Character>();
        }
        private void OnTriggerExit(Collider other)
        {
            player = other.gameObject.GetComponent<Character>();
            if (player)
            {
                lastPlayer = player;
                player = null;
            }
        }
    }
}