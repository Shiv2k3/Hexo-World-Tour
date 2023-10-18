namespace Core.Triggers
{
    public class Die : Trigger
    {
        private void FixedUpdate()
        {
            if(player)
            {
                player.Respawn();
            }
        }
    }
}
