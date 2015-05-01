/// Class based on "Tiny" project example.
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
using Microsoft.Xna.Framework.Input;
#endregion

namespace XNA3Dapplication
{
    class Text
    {
        private SpriteFont Font;
        private Boolean help = false;
        private int lastKeyPressTime = 0;

        public void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("SpriteFont");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.H) && lastKeyPressTime > 200)
            {
                help = !help;
                lastKeyPressTime = 0;
            }
            lastKeyPressTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            Color c = Color.White;
            if (!Game1.light) c = Color.DarkGray;

            spriteBatch.Begin();

            //display score
            {
                string output = "Score : " + Game1.score;
                spriteBatch.DrawString(Font, output, new Vector2(20f, 20f), c, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0.5f);
            }

            //display score
            {
                string output = "Killed : " + Target.number_killed;
                spriteBatch.DrawString(Font, output, new Vector2(20f, 50f), c, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0.5f);
            }

            //Gun sights
            {
                string output = "+";
                spriteBatch.DrawString(Font, output, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 30), Color.DarkGray, 0, Font.MeasureString(output), 1.5f, SpriteEffects.None, 0.5f);
            }

            //display gameover
            if (Game1.gameover)
            {
                string output = "Game Over";
                spriteBatch.DrawString(Font, output, new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2-13), Color.Red, 0, Font.MeasureString(output) / 2, 2f, SpriteEffects.None, 0.5f);
            }

            //display help
            if (help)
            {
                string output = "Drag the Mouse (or use arrows) to move Left/Right, Left click to shoot.\n"
                    + "<N> to start a new game, <L> to switch light on/off, <M> to switch sound on/off\n"
                    + "<H> Hide help, <escape> to quit";
                spriteBatch.DrawString(Font, output, new Vector2(20f, 400f) + Font.MeasureString(output), c, 0, Font.MeasureString(output), 1.0f, SpriteEffects.None, 0.5f);
            }
            else
            {
                string output = "<H> Show help";
                spriteBatch.DrawString(Font, output, new Vector2(20f, 444f) + Font.MeasureString(output), c, 0, Font.MeasureString(output), 1.0f, SpriteEffects.None, 0.5f);
            }

            spriteBatch.End();

            // To get text on screen you need to reset so that depth buffer re-enabled
            DepthStencilState depthBufferState = new DepthStencilState();
            depthBufferState.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = depthBufferState;
        }
    }
}
