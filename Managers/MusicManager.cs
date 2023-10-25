using GameEn;
using System;
using System.IO;

namespace SuperCoolFightingGame {
    public class MusicManager {
        AudioListener audioListener;
        string[] musicPaths;
        int[] musicsCooldown;

        int musicCooldown;

        /// <summary>
        /// Load music in musicFolder path and start a random music
        /// </summary>
        /// <param name="musicFolderPath"></param>
        public MusicManager(string musicFolderPath, bool playAtStart = true, int musicCooldown = 1) {
            //Load all music in a music folder
            string path = AppDomain.CurrentDomain.BaseDirectory + musicFolderPath;
            musicPaths = Directory.GetFiles(path);
            musicsCooldown = new int[musicPaths.Length];

            this.musicCooldown = musicCooldown;

            audioListener = new AudioListener(true);

            if(playAtStart)
                LoadMusic();
        }

        public void LoadMusic() {
            int newMusicId = SelectRandomMusicId();

            if (musicsCooldown[newMusicId] == 0)
            {
                musicsCooldown[newMusicId] += musicCooldown;
                audioListener.ChangeSound(musicPaths[newMusicId]);
                audioListener.Play();
            }
            else
                LoadMusic();

            UpdateCooldown(newMusicId);
        }



        public void StopMusic() {
            audioListener.Stop();
        }

        void UpdateCooldown(int ignoredId) {
            for(int i=0; i<musicsCooldown.Length; i++) {
                if (i != ignoredId) {
                    if (musicsCooldown[i] > 0)
                        musicsCooldown[i]--;
                }
            }
        }

        int SelectRandomMusicId() {
            Random rnd = new Random();
            return rnd.Next(musicPaths.Length);
        }
    }
}
