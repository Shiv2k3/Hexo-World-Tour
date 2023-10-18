using UnityEngine;

namespace Core.Triggers
{
    public class Trap : Trigger
    {
        [SerializeField] private float spawnCooldown = 10.0f;

        private float lastSpawn;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<MeshCollider>().convex = true;
                child.gameObject.AddComponent<Die>();
                
                var rb = child.gameObject.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;

                child.gameObject.SetActive(false);
            }

            lastSpawn = -spawnCooldown;
        }

        private void FixedUpdate()
        {
            if(player && Time.time - spawnCooldown >= lastSpawn)
            {
                foreach (Transform child in transform)
                {
                    Instantiate(child.gameObject, child.position, child.rotation).SetActive(true);
                    lastSpawn = Time.time;
                }
            }
        }
    }
}
