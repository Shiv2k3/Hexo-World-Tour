namespace Core.Triggers
{
    public class Die : Trigger
    {
        private void Update()
        {
            if(player)
            {
                player.Respawn();
            }
        }
    }
}
