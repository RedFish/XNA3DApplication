/// from tutorial : http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Particles.php
/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
#endregion

namespace XNA3Dapplication
{
    class GunFireParticles
    {

        public struct ParticleData
        {
            public float BirthTime;
            public float MaxAge;
            public Vector2 OrginalPosition;
            public Vector2 Accelaration;
            public Vector2 Direction;
            public Vector2 Position;
            public float Scaling;
            public Color ModColor;
        }

        private List<ParticleData> particleList = new List<ParticleData>();
        private Random randomizer = new Random();
        private Texture2D explosionTexture;
        private Boolean clicked = false;

        public void LoadContent(ContentManager Content)
        {
            explosionTexture = Content.Load<Texture2D>("explosion");
        }

        private void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
                AddExplosionParticle(explosionPos, size, maxAge, gameTime);
        }

        private void AddExplosionParticle(Vector2 explosionPos, float explosionSize, float maxAge, GameTime gameTime)
        {
            ParticleData particle = new ParticleData();

            particle.OrginalPosition = explosionPos;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge = maxAge;
            particle.Scaling = 0.25f;
            particle.ModColor = Color.White;

            float particleDistance = (float)randomizer.NextDouble() * explosionSize;
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(randomizer.Next(360));
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));

            particle.Direction = displacement;
            particle.Accelaration = 3.0f * particle.Direction;

            particleList.Add(particle);
        }

        private void UpdateParticles(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            for (int i = particleList.Count - 1; i >= 0; i--)
            {
                ParticleData particle = particleList[i];
                float timeAlive = now - particle.BirthTime;

                if (timeAlive > particle.MaxAge)
                {
                    particleList.RemoveAt(i);
                }
                else
                {
                    float relAge = timeAlive / particle.MaxAge;
                    particle.Position = 0.5f * particle.Accelaration * relAge * relAge + particle.Direction * relAge + particle.OrginalPosition;

                    float invAge = 1.0f - relAge;
                    particle.ModColor = new Color(new Vector4(invAge, invAge, invAge, invAge));

                    Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    particle.Scaling = (50.0f + distance) / 200.0f;

                    particleList[i] = particle;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) clicked = true;

            if (mouseState.LeftButton == ButtonState.Released && clicked)
            {
                AddExplosion(new Vector2(437, 274), 4, 20.0f, 500.0f, gameTime);
                clicked = false;
            }

            if (particleList.Count > 0)
                UpdateParticles(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < particleList.Count; i++)
            {
                ParticleData particle = particleList[i];
                spriteBatch.Draw(explosionTexture, particle.Position, null, particle.ModColor, i, new Vector2(256, 256), particle.Scaling, SpriteEffects.None, 1);
            }
            spriteBatch.End();
        }
    }
}
