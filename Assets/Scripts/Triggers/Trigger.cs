using Core.Player;
using UnityEngine;

namespace Core.Triggers
{
    public class Trigger : MonoBehaviour
    {
        protected Character player;
        private void OnTriggerEnter(Collider other)
        {
            player = other.gameObject.GetComponent<Character>();
        }
        private void OnTriggerStay(Collider other)
        {
            player = other.gameObject.GetComponent<Character>();
        }
        private void OnTriggerExit(Collider other)
        {
            player = other.gameObject.GetComponent<Character>();
            if (player)
            {
                player = null;
            }
        }
    }
}