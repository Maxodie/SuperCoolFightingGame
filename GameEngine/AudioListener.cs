using System;
using System.Windows.Media;

namespace GameEn
{
    public class AudioListener {
        MediaPlayer mediaPlayer;

        bool isLooping = false;
        public AudioListener(bool isLooping, string musicDefaultLocation = "", bool playAtStart = false) {
            this.isLooping = isLooping;

            //create new media player and add event at the end of a music
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += OnMediaEnd;

            if (musicDefaultLocation != "") {
                //Load the music with the location
                mediaPlayer.Open(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + musicDefaultLocation));

                if (playAtStart) {
                    Play();
                } 
            }
        }
        
        public void Play() {
            mediaPlayer.Play();
        }

        /// <summary>
        /// event called on the end a of a media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMediaEnd(object sender, EventArgs e) {
            if(isLooping)
                mediaPlayer.Play();
        }

        public void Stop() {
            mediaPlayer.Stop();
        }

        public void ChangeSound(string musicLocation) {
            //Load a new sound
            Console.WriteLine(musicLocation);
            mediaPlayer.Open(new System.Uri(musicLocation));
        }
        
    }
}
