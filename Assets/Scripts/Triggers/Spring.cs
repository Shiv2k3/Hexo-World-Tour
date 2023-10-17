using UnityEngine;

namespace Core.Triggers
{
    public class Spring : Trigger
    {
        [SerializeField] private float drag = 0.65f;
        [SerializeField] private float maxVelocity = 0.5f;
        [SerializeField] private float buoyancyForce = 1f;

        private bool dragApplied;
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
                if (player.transform.position.y <= transform.position.y)
                {
                    if (player.rigidBody.velocity.y < maxVelocity)
                    {
                        float difference = Mathf.Abs(player.transform.position.y - transform.position.y);
                        player.rigidBody.AddForce(buoyancyForce * difference * Vector3.up, ForceMode.Impulse);
                    }
                }
            }
            else
            {
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