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
#endregion

namespace XNA3Dapplication
{
    class NUIMSign
    {
        private Model model;
        private Matrix[] boneTransforms;
        private Matrix model_transform;
        private Vector3 position;


        public void LoadContext(ContentManager Content)
        {
            model = Content.Load<Model>("nuimsign");
            boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            position = new Vector3(-10, -1.5f, 5f);

            model_transform = model.Root.Transform * Matrix.CreateScale(0.8f);
            model_transform *= Matrix.CreateFromYawPitchRoll(1.0f, 0, 0);
            model_transform *= Matrix.CreateTranslation(position);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
            // Draw Sign
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            model.Root.Transform = model_transform;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    if (Game1.light)
                    {
                        effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                        effect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 1));
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.1f, 0.0f, 0.0f);  // RGB
                        effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0, 0);
                    }
                    else
                    {
                        effect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
                        effect.DiffuseColor = new Vector3(0.0f, 0.0f, 0.0f);
                        effect.EmissiveColor = new Vector3(0.1f, 0.1f, 0.1f);
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 1));
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.1f, 0.0f, 0.0f);  // RGB
                        effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0, 0);
                    }
                    effect.View = CameraFirstPerson.view;
                    effect.Projection = CameraFirstPerson.projection;

                    Vector3 scale, translation;Quaternion rotation;
                    model_transform.Decompose(out scale, out rotation, out translation);
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                }
                mesh.Draw();
            }
        }
    }
}
