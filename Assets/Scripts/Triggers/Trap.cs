using System.Collections.Generic;
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
                child.gameObject.AddComponent<Die>();
                
                child.gameObject.AddComponent<Rigidbody>();

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
