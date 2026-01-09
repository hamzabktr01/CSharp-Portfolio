using Raylib_cs;
using System.Numerics;
using Game.Engine;



namespace Game.Gameplay
{
    public class Projectile
    {
        public Rectangle Rect;
        public Vector2 Velocity;
        public Color ProjectileColor;
        public int Damage { get; private set; }

        public bool projectileIsActive { get; private set; }

        private StripSheetAnimation _animation;
        private float _rotation;

        public Projectile(Vector2 startPosition, Vector2 direction)
        {
            Rect = new Rectangle(startPosition.X, startPosition.Y, 8, 8);
            ProjectileColor = Color.White;
            Damage = 1;
            projectileIsActive = true;

            int speed = 10;
            Velocity = Vector2.Normalize(direction) * speed;
            _animation = TextureManager.Instance.CreateInstance("Projectile");
            _rotation = (float)(Math.Atan2(direction.Y,direction.X)*(180.0/Math.PI));
        }

        public void UpdateProjectile()
        {
            Rect.X += Velocity.X;
            Rect.Y += Velocity.Y;
            _animation.Update(Raylib.GetFrameTime());

            if(Rect.X <0 || Rect.X > ScaledRenderer.VIRTUAL_WIDTH||Rect.Y <0 || Rect.Y > ScaledRenderer.VIRTUAL_HEIGHT)
            {
                DeactivateProjectile();
            }
        }

        public void Draw()
        {
            if (projectileIsActive)
            {

              ScaledRenderer.DrawScaled(
                _animation.Texture,
                _animation.Source,
                Rect.X -6,
                Rect.Y-6,
                20,
                20,
                ProjectileColor
              );
            }
        }

        public void DeactivateProjectile()
        {
            projectileIsActive = false;
        }
    }
}