using UnityEngine;

namespace Core.Platforms
{
    public class Platform : MonoBehaviour
    {
        public Vector3 Velocity { get; private set; }

        private Vector3 lastPosition;

        private void Awake()
        {
            gameObject.layer = 3;
            gameObject.AddComponent<MeshCollider>();
        }

        private void FixedUpdate()
        {
            Velocity = transform.position - lastPosition;
            lastPosition = transform.position;
        }
    }
}