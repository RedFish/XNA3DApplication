/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace XNA3Dapplication
{
    class Target
    {
        private Model model;
        private Matrix[] boneTransforms;
        public static Matrix model_transform;
        public static Vector3 position;
        private float speed = 15.0f;
        public static Boolean alive = false;
        public static int number_killed = 0;
        private Random randomizer = new Random();
        public static Vector3 direction;

        public void Reset()
        {
            alive = false;
            number_killed = 0;
            speed = 15.0f;
        }

        public void Kill()
        {
            alive = false;
            number_killed++;
            speed += 0.8f;
        }

        public void LoadContext(ContentManager Content)
        {
            model = Content.Load<Model>("zombie");
            boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        }

        public void Update(GameTime gameTime)
        {
            if (alive)
            {
                float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                model_transform *= Matrix.CreateTranslation((direction * delta_time * speed));
            }
            else
            {
                float x = randomizer.Next(30) + 10;
                float z = randomizer.Next(30) + 10;
                if (randomizer.Next(100) > 50) x = -x;
                if (randomizer.Next(100) > 50) z = -z;

                position = new Vector3(x, -0.8f, z);

                direction = new Vector3(-x / (int)Math.Sqrt(x * x + z * z), 0, -z / (int)Math.Sqrt(x * x + z * z));

                //Console.WriteLine("position=" + position);
                //Console.WriteLine("direction=" + direction);

                model_transform = model.Root.Transform;
                model_transform = Matrix.Identity * Matrix.CreateTranslation(position);
                alive = true;
            }
        }

        public void Draw()
        {
            // Draw zommbie
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            model.Root.Transform = model_transform;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    if (Game1.light)
                    {
                        effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                        effect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                        effect.EmissiveColor = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                        effect.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f);
                        effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 1));
                        effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f);  // RGB
                        effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0, 0);
                    }
                    effect.View = CameraFirstPerson.view;
                    effect.Projection = CameraFirstPerson.projection;

                    Vector3 scale, translation;Quaternion rotation;
                    model_transform.Decompose(out scale, out rotation, out translation);
                    //Console.WriteLine(position+translation);
                    effect.World = Matrix.CreateFromAxisAngle(Vector3.Up, 3.1f)* Matrix.CreateConstrainedBillboard((position + translation), new Vector3(0, 0, 0), Vector3.Up, null, null);
                }
                mesh.Draw();
            }
        }
    }
}
