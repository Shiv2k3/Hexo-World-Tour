using UnityEngine;
using Core.Platforms;
using UnityEditor;

namespace Core.Player
{
    public class Character : MonoBehaviour
    {
        [Header("--------Speed")]
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float maxSpeed = 10.0f;
        [SerializeField] private float terminalSpeed = 10.0f;
        [Header("--------RigidBody")]
        [SerializeField] private float maxTorque = 7.0f;
        [SerializeField] private float drag = 0.20f;
        [SerializeField] private float mass = 2.0f;
        [Space, Header("--------Acceleration")]
        [SerializeField] private float accelSpeed = 0.5f;
        [SerializeField] private float maxAccel = 1.5f;
        [Space, Header("--------Jumping")]
        [SerializeField] private float jumpForce = 1.5f;
        [SerializeField] private float jumpCooldown = 0.35f;
        [Space, Header("--------Raycast")]
        [SerializeField] private float rayLength = 0.09f;
        [SerializeField] private LayerMask groundLayers;

        private Rigidbody rb;
        private Platform platform;
        private float lastJumped;
        private Vector2 accel = Vector2.one;

        public bool Grounded { get; private set; }
        public Vector3 CheckPoint { get; set; }


        void Awake()
        {
            gameObject.AddComponent<MeshCollider>().convex = true;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
           
            rb.drag = drag;
            rb.mass = mass;
        }

        private void Start()
        {
            CheckPoint = transform.position;
        }

        void FixedUpdate()
        {
            // Find platform(if any), and set Grounded
            Ray downRay = new Ray(transform.position, Vector3.down);
            bool rayHit = Physics.Raycast(downRay, out RaycastHit hitInfo, float.MaxValue, groundLayers);
            if (hitInfo.transform && hitInfo.distance < rayLength)
            {
                platform = hitInfo.transform.gameObject.GetComponent<Platform>();
                Grounded = true;
            }
            else if (hitInfo.transform && hitInfo.distance > rayLength)
            {
                platform = hitInfo.transform.gameObject.GetComponent<Platform>();
                Grounded = false;
                rayHit = false;
            }
            else
            {
                platform = null;
                Grounded = false;
            }

            // Platform force
            if (platform)
            {
                rb.position += platform.Velocity;
            }

            // Horizontal movement
            if (rb.velocity.magnitude < terminalSpeed)
            {
                // Move right
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    // Accel
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        accel.x += accelSpeed;
                        accel.x = Mathf.Clamp(accel.x, 1, 1 + maxAccel);
                    }
                    else
                    {
                        // Decelerate
                        accel.x -= accelSpeed;
                        accel.x = Mathf.Clamp(accel.x, 1, 1 + maxAccel);
                    }

                    // Move
                    Vector3 direction = Quaternion.FromToRotation(Vector3.up, rayHit ? hitInfo.normal  : Vector3.up) * Vector3.right;
                    Vector3 force = Time.deltaTime * speed * direction;
                    float accelration = accel.x * accel.x;
                    force = Vector3.ClampMagnitude(force, maxSpeed - rb.velocity.magnitude) * accelration;
                    rb.AddForce(force, ForceMode.Impulse);
                    rb.AddTorque(Vector3.back * force.magnitude);

}
                else
                {
                    // Decelerate
                    accel.x -= accelSpeed;
                    accel.x = Mathf.Clamp(accel.x, 1, 1 + maxAccel);
                }

                // Move left
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    // Accel
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        accel.y += accelSpeed;
                        accel.y = Mathf.Clamp(accel.y, 1, 1 + maxAccel);
                    }
                    else
                    {
                        // Decelerate
                        accel.y -= accelSpeed;
                        accel.y = Mathf.Clamp(accel.y, 1, 1 + maxAccel);
                    }

                    // Move
                    Vector3 direction = Quaternion.FromToRotation(Vector3.up, rayHit ? hitInfo.normal : Vector3.up) * Vector3.left;
                    Vector3 force = Time.deltaTime * speed * direction;
                    float accelration = accel.y * accel.y;
                    force = Vector3.ClampMagnitude(force, maxSpeed - rb.velocity.magnitude) * accelration;
                    rb.AddForce(force, ForceMode.Impulse);
                    rb.AddTorque(Vector3.forward * force.magnitude);

                }
                else
                {
                    // Decelerate
                    accel.y -= accelSpeed;
                    accel.y = Mathf.Clamp(accel.y, 1, 1 + maxAccel);
                }
            }

            // Vertical
            if (Grounded)
            {
                if ((Time.time - lastJumped) > jumpCooldown && Input.GetKey(KeyCode.Space))
                {
                    rb.AddForce(hitInfo.normal * jumpForce, ForceMode.Impulse);
                    lastJumped = Time.time;
                }
            }

            // Torque
            rb.maxAngularVelocity = maxTorque * rb.velocity.magnitude;
        }

        public void Respawn()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = CheckPoint;
        }
    }
}
