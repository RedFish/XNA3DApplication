/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Concurrent;
#endregion

namespace XNA3Dapplication
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        SkyBox sky;
        CameraFirstPerson camera;
        Text text;
        SoundManager sound;
        GunFireParticles particles;
        Life life;
        Target target;
        NUIMSign sign;
        Collision collision;

        private int lastKeyPressTime = 0;

        // State of spot light
        public static Boolean light = true;

        //Score value
        public static int score = 0;

        //is GameOver
        public static Boolean gameover = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new CameraFirstPerson(graphics);
            sky = new SkyBox(graphics,Content);
            text = new Text();
            sound = new SoundManager();
            particles = new GunFireParticles();
            life = new Life();
            target = new Target();
            sign = new NUIMSign();
            collision = new Collision();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = new BasicEffect(graphics.GraphicsDevice);
            effect.TextureEnabled = true;

            camera.LoadContent(Content);
            sky.LoadContent();
            text.LoadContent(Content);
            sound.LoadContent(Content);
            particles.LoadContent(Content);
            life.LoadContent(Content);
            target.LoadContext(Content);
            sign.LoadContext(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game if "escape" pressed
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Escape)) this.Exit();

            // light
            Boolean oldlight = light;
            if (keys.IsKeyDown(Keys.L) && lastKeyPressTime > 200)
            {
                light = !light;
                lastKeyPressTime = 0;
            }
            lastKeyPressTime += gameTime.ElapsedGameTime.Milliseconds;
            if (light != oldlight) sky.LoadContent(); //change skybox if light change

            //new game
            if (keys.IsKeyDown(Keys.N))
            {
                life.Reset();
                target.Reset();
                camera.Reset();
                gameover = false;
                score = 0;
            }

            // Update camera position 
            camera.Update(gameTime);
            // Update sky
            sky.Update(gameTime);
            // Update help
            text.Update(gameTime);
            // play sound (if click detected)
            sound.Update(gameTime);
            // generate particles (if click detected)
            particles.Update(gameTime);
            // Update Target
            if (!gameover) target.Update(gameTime);
            //Collision detection
            if (!gameover) collision.Update(target,sound);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //sky
            sky.Draw(gameTime, effect);
            //life (heart)
            life.Draw(spriteBatch, GraphicsDevice);
            //Target
            ResetGraphic();//solution http://stackoverflow.com/questions/13342400/fbx-model-not-displaying-correctly-in-xna-4-0
            BeginRender3D();//3D settings after 2D particles
            if(!gameover)target.Draw();
            //NUIM sign
            ResetGraphic();
            BeginRender3D();
            sign.Draw();
            //gun fire
            if (!gameover) particles.Draw(spriteBatch);
            //camera
            ResetGraphic();
            BeginRender3D();
            camera.Draw(gameTime, effect);
            //Description (help)
            text.Draw(spriteBatch,GraphicsDevice);

            base.Draw(gameTime);
        }

        //solution http://stackoverflow.com/questions/13342400/fbx-model-not-displaying-correctly-in-xna-4-0
        public void ResetGraphic()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

        }
        public void BeginRender3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
