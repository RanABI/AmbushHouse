using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static Ambush.Enums;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ambush.Components
{
    class TargetClientHandler
    {

        //Turn on led lights Source:Id:True/False (send to MicroController)
        //Invoke sound
        //Add points to player (according to strength of hit)
        //Wait few seconds and turn off led lights
        public void ChangeTargetState(int targetID, Direction newState, int physicalID)
        {//AR:TR:SET:ID:STATE:PHYID
            CPX cpx = Play.getCpxByPhysicalId(physicalID);
            StringBuilder builder = new StringBuilder();
            builder.Append("TR:");
            builder.Append("SET:");
            builder.Append(targetID.ToString());
            builder.Append(":");
            builder.Append(newState.ToString());
            System.Windows.MessageBox.Show(builder.ToString());
            //using (TCPClient client = new TCPClient(builder.ToString(), cpx)) ;
        }
        public void TargetLedControl(Direction state)
        {
            // StringBuilder builder = new StringBuilder();
            // builder.Append("TR:");
            //// builder.Append(this.id.ToString());
            // builder.Append(":");
            // builder.Append(state.ToString());
            // //SEND TO MICROCONTROLLER TARGET STRING TO TURN ON LEDS

        }

        public void InvokeSound()
        {
            //Invoke sound in case of HIT
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //mediaPlayer.Open(new Uri(openFileDialog.FileName));
                //mediaPlayer.Play();
            }
        }


    }
}
