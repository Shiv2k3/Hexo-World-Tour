using UnityEngine;

namespace Core.Triggers
{
    public class Win : Trigger
    {
        private void Update()
        {
            if (player)
            {
                Application.Quit();
            }

        }
    }
}