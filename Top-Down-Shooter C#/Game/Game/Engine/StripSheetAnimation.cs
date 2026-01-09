using Raylib_cs;

namespace Game.Engine
{
    public sealed class StripSheetAnimation
    {
        public string Key { get; set; }
        public int Id { get; }
        public float Fps { get; set; } = 12;
        public bool Loop { get; set; } = true;
        public Texture2D Texture { get; set; }
        public int Tilesize { get; private set; }
        public int Framecount { get; private set; }

        private int _frameindex;
        private float _accumulator;
        private bool _finished;

        public StripSheetAnimation(string key, Texture2D texture, float fps = 12f, bool loop = true, int id = -1)
        {
            Id = id;
            Key = key;
            Texture = texture;
            Tilesize = Math.Max(1, texture.Height);
            Framecount = Math.Max(1, texture.Width / Tilesize);
            Fps = fps <= 0 ? 12 : fps;
            Loop = loop;

            if (id >= 0)
            {
                GameBus.AnimationChangeEvent += OnAnimationChange;
            }


        }

        private void OnAnimationChange(int id, string newKey)
        {
            if (id != Id) return;

            var baseAnim = TextureManager.Instance.Get(newKey);
            Key = baseAnim.Key;
            Texture = baseAnim.Texture;

            Tilesize = Texture.Height;
            Framecount = Texture.Width / Tilesize;

            Reset();

        }

        public void unregister()
        {
            if (Id >= 0)
            {
                GameBus.AnimationChangeEvent -= OnAnimationChange;
            }
        }
        public void Reset()
        {
            _frameindex = 0;
            _accumulator = 0;
            _finished = false;
        }

        public void Update(float deltaTime)
        {
            if (Framecount <= 1 || _finished) return;

            float frameTime = 1f / Math.Max(1f, Fps);
            _accumulator += deltaTime;

            while (_accumulator >= frameTime)
            {
                _accumulator -= frameTime;
                _frameindex++;

                if (_frameindex >= Framecount)
                {
                    if (Loop)
                    {
                        _frameindex = 0;
                    }
                    else
                    {
                        _frameindex = Framecount - 1;
                        _finished = true;
                        break;
                    }
                }
            }
        }

        public Rectangle Source =>
            new Rectangle(_frameindex * Tilesize, 0, Tilesize, Tilesize);

    }
}
