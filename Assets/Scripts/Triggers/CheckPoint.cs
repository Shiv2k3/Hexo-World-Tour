using UnityEngine;
using UnityEngine.VFX;

namespace Core.Triggers
{
    public class CheckPoint : Trigger
    {
        private VisualEffect confetti;
        private bool set;

        private void Awake()
        {
            confetti = GetComponentInChildren<VisualEffect>();
        }
        private void Start()
        {
            confetti.Stop();
        }
        private void Update()
        {
            if (player && !set)
            {
                Vector3 point = transform.position;
                point.z = player.transform.position.z;
                player.CheckPoint = point;
                confetti.Play();
                set = true;
            }
        }
    }
}