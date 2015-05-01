/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace XNA3Dapplication
{
    class Collision
    {
        private Boolean clicked = false;

        public void Update(Target target, SoundManager sound)
        {
            //get postion of target
            Vector3 scale, translation;Quaternion rotation;
            Target.model_transform.Decompose(out scale, out rotation, out translation);
            translation += Target.position;
            float distance = Vector3.Distance(Vector3.Zero, translation);

            if (distance<=2)
            {
                Life.looseOne();
                if(!Game1.gameover) sound.Zombie1();
                else sound.Zombie2();
                Target.alive = false;
            }

            //shoot (hit the target)
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) clicked = true;

            if (mouseState.LeftButton == ButtonState.Released && clicked)
            {
                Vector3 cross = Vector3.Cross(CameraFirstPerson.campos, Target.direction);
                float e = 1.5f / distance;//tolerance of the cross product
                if (cross.Y <= e && cross.Y >= -e)
                {
                    sound.Zombie3();
                    target.Kill();
                    Game1.score += (int)distance*10;
                }
                clicked = false;
            }
        }
    }
}
