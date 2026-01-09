using System.Numerics;
using Raylib_cs;

namespace Game.Gameplay
{
    public class GroupWall
    {

        public Rectangle Rect;
        public Texture2D Texture;
        public bool IsVertical;

        public Vector2 StartPos;
        public Vector2 targetPos;
        private bool isMoving = false;
        private float moveProgress = 0f;



        public GroupWall(Rectangle rect, Texture2D texture, bool isVertical = true)
        {
            Rect = rect;
            StartPos = new Vector2(rect.X, rect.Y);
            Texture = texture;
            targetPos = StartPos;
            IsVertical = isVertical;
        }

        public void SetTarget(Vector2 target)
        {
            StartPos = new Vector2(Rect.X, Rect.Y);
            targetPos = target;
            isMoving = true;
            moveProgress = 0f;
        }

        public void Update(float dt, float speed = 1f)
        {
            if (!isMoving) return;

            moveProgress += dt * speed;
            if (moveProgress >= 1f)
            {
                moveProgress = 1f;
                isMoving = false;

                IsVertical = !IsVertical;
            }

            Rect.X = StartPos.X + (targetPos.X - StartPos.X) * moveProgress;
            Rect.Y = StartPos.Y + (targetPos.Y - StartPos.Y) * moveProgress;
        }
        public void Draw()
        {
            ScaledRenderer.DrawScaled(Texture,Rect.X, Rect.Y, Rect.Width, Rect.Height, Color.Red);
        }

    }
}
