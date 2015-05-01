/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XNA3Dapplication
{
    class Life
    {
        private Texture2D heart;
        private Rectangle rectangle;
        public static int left = 3;

        public void Reset()
        {
            left = 3;
        }

        public static void looseOne()
        {
            left--;
            if(left<=0) Game1.gameover = true;
        }

        public void LoadContent(ContentManager Content)
        {
            heart = Content.Load<Texture2D>("heart");
            rectangle = new Rectangle(0, 0, 170, 150);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            float scale = 0.15f;
            spriteBatch.Begin();
            for(int i = 0; i<left; i++)
            {
                spriteBatch.Draw(heart, new Vector2(GraphicsDevice.Viewport.Width - 20 - 170 * scale - i * (170 * scale + 10), 20), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }
    }
}
