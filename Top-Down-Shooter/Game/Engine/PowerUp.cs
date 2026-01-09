using Game.Gameplay;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace Game.Engine
{
    public class PowerUp : Item
    {
        private StripSheetAnimation AnimationPowerUp;
        public PowerUp(int startX, int startY, int sizeX, int sizeY, uint health) : base(startX, startY, sizeX, sizeY, health)
        {
            this.ItemRectangle = new Raylib_cs.Rectangle(EntityVector2.X, EntityVector2.Y, SizeX, SizeY);

            this.AnimationPowerUp = TextureManager.Instance.CreateInstance(
                key: "powerup",
                fps: 10,
                loop: true,
                id: 3
                );
        }
        public void takeDamage(uint damage)
        {
            Health = Health - damage;
        }
        public override void DrawItemAnimated()
        {
            if (Health <= 0) return;
                
            ScaledRenderer.DrawScaled(
                AnimationPowerUp.Texture,
                AnimationPowerUp.Source,
                ItemRectangle.X,
                ItemRectangle.Y,
                ItemRectangle.Height,
                ItemRectangle.Width,
                Color.White
            );
        }

        public override void Update_Item(ref Player player)
        {
            AnimationPowerUp?.Update(GetFrameTime());
            
            if (Health <= 0)
            {
                ItemRectangle.Width = 0;
                ItemRectangle.Width = 0;
                //player.PlayerSpeed = 5;
                player.PowerUpAvailable = true;
                return;
            }
        }

    }
}
