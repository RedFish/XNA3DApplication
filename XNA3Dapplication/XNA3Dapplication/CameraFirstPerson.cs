/// class based on "Quaternations" project example
/// and from http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2/Quaternions.php
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace XNA3Dapplication
{
    class CameraFirstPerson
    {
        private GraphicsDeviceManager graphics;
        // Rifle
        // Data structures required to store rifle model data
        private Model rifle;
        private Matrix[] rifle_boneTransforms;
        private Matrix rifle_transform;

        // Rifle position (Camera located above and behind rifle)
        private Vector3 riflePosition = new Vector3(0, 0, 0);
        private Quaternion rifleRotation = Quaternion.Identity;

        // Control inputs
        private float leftrightRotation = 0, updownRotation = 0, dz = 0;

        private Boolean deadposition = false;
        //const float moveSpeed = 30.0f;

        // Camera 
        public static Matrix view, projection;
        public static Vector3 campos;
        private Vector3 camup;

        public CameraFirstPerson(GraphicsDeviceManager graphics)
        {
            //Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            this.graphics = graphics;
        }

        public void LoadContent(ContentManager Content)
        {
            // Load model from run-time directory (debug/release)
            rifle = Content.Load<Model>(@"ak-47");

            rifle_boneTransforms = new Matrix[rifle.Bones.Count];
            rifle.CopyAbsoluteBoneTransformsTo(rifle_boneTransforms);
            rifle_transform = rifle.Root.Transform * Matrix.CreateScale(0.0045f) * Matrix.CreateRotationY(-MathHelper.PiOver2 + 0.05f) * Matrix.CreateRotationX(-0.07f) * Matrix.CreateTranslation(-0.60f, -0.4f, -0.25f);

        }

        public void Reset()
        {
            rifleRotation = Quaternion.Identity;
            deadposition = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Game1.gameover)
            {
                float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                MouseState currentMouseState = Mouse.GetState();
                float xDifference = graphics.GraphicsDevice.Viewport.Width / 2 - currentMouseState.X;
                float yDifference = currentMouseState.Y - graphics.GraphicsDevice.Viewport.Height / 2;
                Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
                leftrightRotation = xDifference * timeDifference / 20;
                //updownRotation = yDifference * timeDifference;

                KeyboardState keys = Keyboard.GetState();
                // Left/Right
                if (keys.IsKeyDown(Keys.Left)) leftrightRotation = 0.05f;
                if (keys.IsKeyDown(Keys.Right)) leftrightRotation = -0.05f;
                // Up/Down
                //if (keys.IsKeyDown(Keys.Up)) updownRotation = -0.05f;
                //if (keys.IsKeyDown(Keys.Down)) updownRotation = 0.05f;
            }

            //rifleRotation *= Quaternion.CreateFromAxisAngle(Vector3.Right, updownRotation);
            rifleRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, leftrightRotation);
            //Dead position
            if (Game1.gameover && !deadposition)
            {
                rifleRotation *= Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.PiOver4);
                deadposition = true;
                leftrightRotation = 0;
            }

            //  Camera relative to rifle
            campos = Vector3.Transform(new Vector3(0.25f, 0f, -1.45f), Matrix.CreateFromQuaternion(rifleRotation));
            campos += riflePosition;

            camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(rifleRotation));

            view = Matrix.CreateLookAt(campos, riflePosition, camup);

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            (float)this.graphics.GraphicsDevice.Viewport.Width /
            (float)this.graphics.GraphicsDevice.Viewport.Height, 1.0f, 400.0f);

        }



        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            effect.View = view;

            // Draw rifle
            rifle.CopyAbsoluteBoneTransformsTo(rifle_boneTransforms);
            rifle.Root.Transform = rifle_transform * Matrix.CreateFromQuaternion(rifleRotation) * Matrix.CreateTranslation(riflePosition);
            foreach (ModelMesh mesh in rifle.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.EnableDefaultLighting();
                    if (Game1.light)
                    {
                        eff.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                        eff.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                        eff.EmissiveColor = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        eff.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                        eff.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f);
                        eff.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
                        eff.DirectionalLight0.Enabled = true;
                        eff.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 1));
                        eff.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f);  // RGB
                        eff.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0, 0);
                    }
                    eff.View = view;
                    eff.Projection = projection;
                    eff.World = rifle_boneTransforms[mesh.ParentBone.Index];
                }
                mesh.Draw();
            }
        }
    }
}
