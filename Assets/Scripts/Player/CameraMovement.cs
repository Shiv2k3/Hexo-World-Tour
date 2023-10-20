using UnityEditor;
using UnityEngine;

namespace Core.Player
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Character player;
        [SerializeField] private Vector2 restAngle;
        [SerializeField] private float distance = 8;
        [SerializeField] private float sensitivity = 1;
        [SerializeField] private float smoothing = 1;

        private Vector3 lastPosition;
        private Vector2 input;
        private float lastMoved;
        private Transform target;
        private float restY;

        private void Awake()
        {
            target = new GameObject().transform;
            player = FindObjectOfType<Character>();
            restY = restAngle.y;
            input = restAngle;
        }

        private void FixedUpdate()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                distance -= scroll * sensitivity;

            float xIn = Input.GetAxis("Mouse X");
            float yIn = -Input.GetAxis("Mouse Y");
            bool moving = Mathf.Abs(xIn) + Mathf.Abs(yIn) > 0;
            float difference = player.transform.position.x - lastPosition.x;
            float direction = Mathf.Sign(Mathf.Abs(difference) > float.Epsilon ? difference : 0);

            if (Time.time <= lastMoved + 1.5f || moving)
            {
                input += new Vector2(yIn, xIn);
                input.x = Mathf.Clamp(input.x, -89, 89);

                if (moving)
                    lastMoved = Time.time;
            }
            else
            {
                restAngle.y = direction == 0 ? restY : direction * restY;
                input = Vector2.Lerp(input, restAngle, smoothing);
            }

            target.position = player.transform.position;
            Vector3 smoothTarget = Vector3.Lerp(lastPosition, target.position, smoothing);

            if (Cursor.lockState == CursorLockMode.Locked)
                target.rotation = Quaternion.Euler(input);
            transform.position = smoothTarget + target.forward * -distance;
            lastPosition = target.position;

            transform.LookAt(smoothTarget);
        }
    }
}
