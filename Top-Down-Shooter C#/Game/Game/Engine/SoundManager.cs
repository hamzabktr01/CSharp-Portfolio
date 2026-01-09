using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raylib_cs;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Game.Engine
{
    public class SoundManager
    {
        
        private Dictionary<string, Sound> sounds = new();
        private Dictionary<string, Music> musics = new();


        private static Music? lastMusic = null;
        private static bool isFading = false;
        public SoundManager()
        {   
            GameBus.GameEvent += HandleGameEvent;
        }
        public void LoadFromFolder()
        {
            string basedirectory = AppContext.BaseDirectory;
            string soundfolder = Path.Combine(basedirectory, "../../../Assets/sounds");
            string musicfolder = Path.Combine(basedirectory, "../../../Assets/music");


            if (!Directory.Exists(soundfolder))
            {
                Console.WriteLine($"SoundManager found no nothing in '{soundfolder}'. Soundloading stopped");
                return;
            }

            
            string[] soundFiles = Directory.GetFiles(soundfolder, "*.*")
                        .Where(f => f.EndsWith(".wav") || f.EndsWith(".ogg"))
                        .ToArray();

            foreach (var file in soundFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                Sound s = Raylib.LoadSound(file);
                sounds[name] = s;
                Console.WriteLine($"[SoundManager] Loaded: {name}");
            }

            if (!Directory.Exists(musicfolder))
            {
                Console.WriteLine($"SoundManager found no nothing in '{soundfolder}'. Musicloading stopped ");
                return;
            }

            string[] musicFiles = Directory.GetFiles(musicfolder, "*.*")
                        .Where(f => f.EndsWith(".ogg") || f.EndsWith(".mp3") || f.EndsWith(".wav"))
                        .ToArray();
                
            foreach (var file in musicFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                Music m = Raylib.LoadMusicStream(file);
                m.Looping = true;

                musics[name] = m;
                Console.WriteLine($"[Soundmanager] Loaded Music: {name}");
            }

        }

        public void Play(string name)
        {
          
            if (!(sounds.TryGetValue(name, out Sound s)))
            {
                Console.WriteLine($"Sound {name} was not found");
            }
            if (Raylib.IsSoundPlaying(s))
                return;

            Raylib.PlaySound(s);
        }

        //Plays Music tracks for the game. Fades into next Song. CAN`T FADE INTO SAME SONG!!!
        public async Task Playmusic(string name)
        {
            if (!(musics.TryGetValue(name, out Music newMusic)))
            {
                Console.WriteLine($"Music {name} was not found");
                return;
            }

            while (isFading)
                await Task.Delay(5);

            isFading = true;

            if (lastMusic == null)
            {
                lastMusic = newMusic;

                Raylib.SetMusicVolume(newMusic, 0f);
                Raylib.PlayMusicStream(newMusic);

                double duration = 0.6;
                DateTime start = DateTime.Now;

                while ((DateTime.Now - start).TotalSeconds < duration)
                {
                    double t = (DateTime.Now - start).TotalSeconds / duration;
                    Raylib.SetMusicVolume(newMusic, (float)t);
                    
                    Raylib.UpdateMusicStream(newMusic);
                    await Task.Delay(5);

                }

                Raylib.SetMusicVolume(newMusic, 0.7f);

                isFading = false;
                return;
            }

            Music oldMusic = (Music)lastMusic;
            lastMusic = newMusic;

            Raylib.SetMusicVolume(newMusic, 0f);
            Raylib.PlayMusicStream(newMusic);

            double fadeTime = 1.5;
            DateTime fadeStart = DateTime.Now;

            while ((DateTime.Now - fadeStart).TotalSeconds < fadeTime)
            {
                double t = (DateTime.Now - fadeStart).TotalSeconds / fadeTime;

                Raylib.SetMusicVolume(oldMusic, (float)(0.7 - t));
                Raylib.SetMusicVolume(newMusic, (float)t);

                Raylib.UpdateMusicStream(oldMusic);
                Raylib.UpdateMusicStream(newMusic);
                await Task.Delay(5);
            }
            Raylib.StopMusicStream(oldMusic);
            Raylib.SetMusicVolume(newMusic, 0.7f);

            isFading = false;
        }

        public void HandleGameEvent(string eventName, Soundtype soundtyp)
        {
            if(soundtyp == Soundtype.effekt)
            {
                Play(eventName);
            }
            if(soundtyp == Soundtype.music)
            {
                Playmusic(eventName);
            }

        }


        public void Update()
        {
            if (lastMusic != null)
            {
                Raylib.UpdateMusicStream(lastMusic.Value);
            }
        }
        public void UnloadAll()
        {
            foreach (var sound in sounds.Values)
            {
                Raylib.UnloadSound(sound);
            }
            foreach (var music in musics.Values)
            {
                Raylib.UnloadMusicStream(music);
            }
            sounds.Clear();
            musics.Clear();
            GameBus.GameEvent -= HandleGameEvent;
        }
    }
}
