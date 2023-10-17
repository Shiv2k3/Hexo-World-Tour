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
        [SerializeField] private float drag = 0.20f;
        [SerializeField] private float mass = 2.0f;
        [SerializeField] private float maxTorque = 3f;
        [SerializeField, Range(0,1)] private float maxSlope = 0.75f;

        [Space, Header("--------Acceleration")]
        [SerializeField] private float accelSpeed = 0.5f;
        [SerializeField] private float maxAccel = 1.5f;

        [Space, Header("--------Jumping")]
        [SerializeField] private float jumpForce = 1.5f;
        [SerializeField] private float jumpCooldown = 0.35f;

        [Space, Header("--------Exiting Platforms")]
        [SerializeField] private float exitSmoothness = 0.01f;
        [SerializeField] private Vector2 exitMagnitudeRange = Vector2.up;

        [Space, Header("--------Raycast")]
        [SerializeField] private float rayLength = 0.09f;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private LayerMask waterLayer;

        [HideInInspector] public Rigidbody rigidBody;
        private Platform platform;
        private Vector3 lastPlatformVelocity;
        private float lastJumped;
        private Vector2 accel = Vector2.one;

        public bool Grounded { get; private set; }
        public RaycastHit HitInfo { get; private set; }
        public Vector3 CheckPoint { get; set; }

        public void Respawn()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            transform.position = CheckPoint;
            accel = Vector2.zero;
            lastPlatformVelocity = Vector3.zero;
            lastJumped = 0;
        }

        void Awake()
        {
            gameObject.AddComponent<MeshCollider>().convex = true;
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
           
            rigidBody.drag = drag;
            rigidBody.mass = mass;
            rigidBody.maxAngularVelocity = maxTorque;
        }

        private void Start()
        {
            CheckPoint = transform.position;
        }

        void FixedUpdate()
        {
            // Find platform(if any), and set Grounded
            Ray downRay = new Ray(transform.position, Vector3.down);
            Physics.Raycast(downRay, out RaycastHit hitInfo, float.MaxValue, groundLayers);
            if (hitInfo.transform && hitInfo.distance < rayLength)
            {
                platform = hitInfo.transform.gameObject.GetComponent<Platform>();
                Grounded = true;
            }
            else if (hitInfo.transform && hitInfo.distance > rayLength)
            {
                platform = hitInfo.transform.gameObject.GetComponent<Platform>();
                Grounded = false;
            }
            else
            {
                platform = null;
                Grounded = false;
            }
            HitInfo = hitInfo;

            // Platform force
            if (platform)
            {
                rigidBody.position += platform.Velocity;
                lastPlatformVelocity = platform.Velocity;
            }
            else
            {
                lastPlatformVelocity = Vector3.Lerp(lastPlatformVelocity, Vector3.zero, exitSmoothness);
                float mag = lastPlatformVelocity.magnitude;
                if (mag > exitMagnitudeRange.x && mag < exitMagnitudeRange.y)
                    rigidBody.position += lastPlatformVelocity;
            }

            // Horizontal movement
            if (rigidBody.velocity.magnitude < terminalSpeed)
            {
                // Move right
                if (Input.GetKey(KeyCode.D))
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
                    Vector3 direction = Vector3.zero;
                    if(Grounded)
                    {
                        direction = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * Vector3.right;
                        if (hitInfo.normal.x < 0) direction.x *= hitInfo.normal.y > maxSlope ? 1 : -1;
                    }
                    Vector3 force = Time.deltaTime * speed * direction;
                    Vector3 accelration = Time.deltaTime * accel.x * accel.x * Vector3.right;
                    force = Vector3.ClampMagnitude(force, maxSpeed - rigidBody.velocity.magnitude) + accelration;
                    rigidBody.AddForce(force, ForceMode.Impulse);
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
                    Vector3 direction = Vector3.zero;
                    if (Grounded)
                    {
                        direction = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * Vector3.left;
                        if (hitInfo.normal.x > 0) direction.x *= hitInfo.normal.y > maxSlope ? 1 : -1;
                    }
                    Vector3 force = Time.deltaTime * speed * direction;
                    Vector3 accelration = Time.deltaTime * accel.y * accel.y * Vector3.left;
                    force = Vector3.ClampMagnitude(force, maxSpeed - rigidBody.velocity.magnitude) + accelration;
                    rigidBody.AddForce(force, ForceMode.Impulse);
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
                    rigidBody.AddForce(hitInfo.normal * jumpForce, ForceMode.Impulse);
                    lastJumped = Time.time;
                }
            }

            // Torque
            rigidBody.maxAngularVelocity = maxTorque + Mathf.Abs(rigidBody.velocity.x);
        }
    }
}
