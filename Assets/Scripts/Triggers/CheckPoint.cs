namespace Core.Triggers
{
    public class CheckPoint : Trigger
    {
        private void Update()
        {
            if (player)
            {
                player.CheckPoint = transform.position;
            }
        }
    }
}