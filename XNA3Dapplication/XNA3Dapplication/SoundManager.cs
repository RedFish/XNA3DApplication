/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
#endregion

namespace XNA3Dapplication
{
    class SoundManager
    {
        private SoundEffect sound_gun_fire;
        private SoundEffect sound_zombie1;
        private SoundEffect sound_zombie2;
        private SoundEffect sound_zombie3;
        private Song sound_background;
        private SoundEffectInstance sound;
        private Boolean clicked = false;
        private Boolean muted = false;
        private int lastKeyPressTime = 0;

        public void LoadContent(ContentManager content)
        {
            sound_gun_fire = content.Load<SoundEffect>("gun-gunshot-01");
            sound_zombie1 = content.Load<SoundEffect>("zombie1");
            sound_zombie2 = content.Load<SoundEffect>("zombie2");
            sound_zombie3 = content.Load<SoundEffect>("zombie3");
            sound_background = content.Load<Song>("backgroundsound");

            Background();
        }

        private void Background()
        {
            MediaPlayer.Play(sound_background);
            MediaPlayer.IsRepeating = true;
        }

        private void Shoot()
        {
            if (!muted)
            {
                sound = sound_gun_fire.CreateInstance();
                sound.Play();
            }
        }

        public void Zombie1()
        {
            if (!muted)
            {
                SoundEffectInstance s;
                s = sound_zombie1.CreateInstance();
                s.Play();
            }
        }

        public void Zombie2()
        {
            if (!muted)
            {
                SoundEffectInstance s;
                s = sound_zombie2.CreateInstance();
                s.Play();
            }
        }

        public void Zombie3()
        {
            if (!muted)
            {
                SoundEffectInstance s;
                s = sound_zombie3.CreateInstance();
                s.Play();
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) clicked = true;

            if (mouseState.LeftButton == ButtonState.Released &&  clicked)
            {
                if (!Game1.gameover) Shoot();
                clicked = false;
            }

            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.M) && lastKeyPressTime > 200)
            {
                muted = !muted;
                MediaPlayer.IsMuted = muted;
                lastKeyPressTime = 0;
            }
            lastKeyPressTime += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
