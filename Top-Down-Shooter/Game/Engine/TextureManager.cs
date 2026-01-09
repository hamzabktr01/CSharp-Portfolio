using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    public class TextureManager : IDisposable
    {
        public static TextureManager Instance { get; private set; }
        private readonly Dictionary<string, StripSheetAnimation> _animations =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly List<Texture2D> _textures = new();
        private readonly string _spriteFolder;
        public bool Has(string key) => _animations.ContainsKey(key);

        public TextureManager()
        {
            Instance = this;

            string baseDirectory = AppContext.BaseDirectory;

            _spriteFolder = Path.Combine(baseDirectory,
                "../../../Assets/sprites");
            Load();
        }
        public StripSheetAnimation Get(string key)
        {
            if (!_animations.TryGetValue(key, out var animation))
                throw new KeyNotFoundException($"Animation '{key}' not found.");
            return animation;
        }

        public StripSheetAnimation CreateInstance(string key, float fps = 12f, bool loop = true, int id = -1)
        {
            var baseAnim = Get(key);
            return new StripSheetAnimation(key, baseAnim.Texture, fps, loop, id);
        }

        public void Load(float defaultFps = 12f, bool defaultLoop = true)
        {
            if (!Directory.Exists(_spriteFolder))
                throw new DirectoryNotFoundException(
                    $"Sprite folder not found: {_spriteFolder}"
                );

            foreach (var path in Directory.EnumerateFiles(_spriteFolder, "*.png", SearchOption.AllDirectories)
                    .OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
            {
                string key = Path.GetFileNameWithoutExtension(path);

                var texture = Raylib.LoadTexture(path);
                _textures.Add(texture);

                _animations[key] = new StripSheetAnimation(key, texture, defaultFps, defaultLoop);
            }
        }

        public void Dispose()
        {
            foreach (var texture in _textures)
            {
                if (texture.Id != 0)
                    Raylib.UnloadTexture(texture);
            }
            _textures.Clear();
            _animations.Clear();
        }




    }
}
