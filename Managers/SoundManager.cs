using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingGame.Managers
{
    public class SoundManager
    {
        public Song BackgroundSong;
        public Dictionary<string, SoundEffect> SoundEffects;
        private readonly ContentManager _contentManager;

        public SoundManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void LoadBackgroundMusic(string backgroundMusicName)
        {
            BackgroundSong = _contentManager.Load<Song>(backgroundMusicName);
            MediaPlayer.Play(BackgroundSong);
            MediaPlayer.IsRepeating = true;
        }

        public void LoadSoundEffects()
        {
            SoundEffects = new Dictionary<string, SoundEffect>
            {
                { "select", _contentManager.Load<SoundEffect>("SFX/buttonClick") },
                { "newCustomer", _contentManager.Load<SoundEffect>("SFX/newcustomer") },
                {"grill", _contentManager.Load<SoundEffect>("SFX/grill") }
            };
        }
    }
}
