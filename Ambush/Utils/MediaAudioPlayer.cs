using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ambush.Utils
{
    public class MediaAudioPlayer

    {
        public string filepath;
        public MediaAudioPlayer(string filpath)
        {
            mediaPlayer.MediaOpened += new EventHandler(Play);
            this.mediaPlayer.Open(new Uri(filepath));
        }
        private MediaPlayer mediaPlayer = new MediaPlayer();


        public void Play(Object sender, EventArgs e)
        {
            mediaPlayer.Play();
            mediaPlayer.Close();
        }
    }
}
