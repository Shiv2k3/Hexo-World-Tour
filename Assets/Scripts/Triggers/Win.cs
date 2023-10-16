using UnityEngine;

namespace Core.Triggers
{
    public class Win : Trigger
    {
        private void Update()
        {
            if (player)
            {
                Debug.Log("Player Won");
            }

        }
    }
}