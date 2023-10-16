using UnityEngine;

namespace Core.Player
{
    public class Camera : MonoBehaviour
    {
        [SerializeField] private Character player;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothing = 1;

        private Rigidbody rb;
        private Vector3 lastPosition;

        private void Awake()
        {
            player = FindObjectOfType<Character>();
        }

        private void Start()
        {
            rb = player.GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            transform.position = rb.worldCenterOfMass + offset;

            Vector3 smoothWP = Vector3.Lerp(lastPosition, player.transform.position, smoothing);
            Quaternion rotation = transform.rotation;
            transform.LookAt(smoothWP);
            transform.rotation = Quaternion.Slerp(rotation, transform.rotation, smoothing);
            lastPosition = player.transform.position;
        }
    }
}
