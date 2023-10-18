using UnityEngine;

namespace Core.Triggers
{
    public class Spring : Trigger
    {
        [SerializeField] private float drag = 0.65f;
        [SerializeField] private float maxVelocity = 0.5f;
        [SerializeField] private float buoyancyForce = 1f;

        private bool dragApplied;
        private float timeUnder;
        private void FixedUpdate()
        {
            if (player)
            {
                // Friction
                if (!dragApplied)
                {
                    player.rigidBody.drag += drag;
                    player.rigidBody.angularDrag += drag;
                    dragApplied = true;
                }

                // Buoyancy
                float difference = transform.position.y - player.transform.position.y;
                if (player.rigidBody.velocity.y < difference + maxVelocity)
                {
                    player.rigidBody.AddForce(buoyancyForce * timeUnder * difference * Vector3.up, ForceMode.Impulse);
                    timeUnder += Time.deltaTime;
                }
            }
            else
            {
                timeUnder = 0;
                if (dragApplied)
                {
                    lastPlayer.rigidBody.drag -= drag;
                    lastPlayer.rigidBody.angularDrag -= drag;
                    dragApplied = false;
                }
            }
        }
    }
}