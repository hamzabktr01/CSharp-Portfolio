using static Raylib_cs.Raylib;
using Game.Gameplay;
using Raylib_cs;


namespace Game.Engine
{
    public class Coin : Item
    {
        public int points;



        private StripSheetAnimation Animation;
        public Coin(int points, int startX, int startY, int sizeX, int sizeY, uint health) : base(startX, startY, sizeX, sizeY, health)
        {
            this.ItemRectangle = new Raylib_cs.Rectangle(EntityVector2.X, EntityVector2.Y, SizeX, SizeY);
            this.points = points;

            this.Animation = TextureManager.Instance.CreateInstance(
                key: "coin_spin",
                fps: 12,
                loop: true,
                id: 1
                );
        }
        public override void Update_Item(ref Player player)
        {
            Animation?.Update(GetFrameTime());

            if (Health <= 0)
            {
                ItemRectangle.Width = 0;
                ItemRectangle.Width = 0;

                return;
            }

            if (CheckCollisionCircleRec(player.Position, player.Radius, ItemRectangle))
            {
                Health = 0;
                player.Points = player.Points + points;
                GameBus.Emit("collected");
                GameBus.Emit(1, "coin_rainbow");

            }


        }

        public override void DrawItemAnimated()
        {
            if (Health <= 0) return;

            ScaledRenderer.DrawScaled(
                Animation.Texture,
                Animation.Source,
                ItemRectangle.X,
                ItemRectangle.Y,
                ItemRectangle.Height,
                ItemRectangle.Width,
                Color.White
            );
        }

    }
}


