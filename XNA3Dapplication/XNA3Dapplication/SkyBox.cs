/// SkyBox class based on XN4_Skybox.pdf
/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 3

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
#endregion

namespace XNA3Dapplication
{
    class SkyBox
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;

        // Textures for sky box
        private Texture2D[] sky_textures = new Texture2D[6];
        // Skybox vertices
        private VertexPositionTexture[] TempList;
        private VertexPositionTexture[,] skyboxVertices;

        public SkyBox(GraphicsDeviceManager graphics, ContentManager content)
        {
            this.content = content;
            this.graphics = graphics;
        }

        public void LoadContent()
        {
            // loading the skybox textures regarding the light
            string dark_extention = "";
            if (!Game1.light) dark_extention = "dark_";

            // The skybox, (inside the cube)
            // 6 Sides each created using 2 triangles each triangle has 3 verices = 36 vertices.
            skyboxVertices = new VertexPositionTexture[6, 6];

            sky_textures[0] = content.Load<Texture2D>(dark_extention + "front");
            sky_textures[1] = content.Load<Texture2D>(dark_extention + "back");
            sky_textures[2] = content.Load<Texture2D>(dark_extention + "right");
            sky_textures[3] = content.Load<Texture2D>(dark_extention + "left");
            sky_textures[4] = content.Load<Texture2D>(dark_extention + "ground");
            sky_textures[5] = content.Load<Texture2D>(dark_extention + "sky");

            Vector3 position = new Vector3(0, -4, 0);

            // Front
            skyboxVertices[0, 0] = new VertexPositionTexture(new Vector3(200, -200, 200) + position, new Vector2(0, 1));
            skyboxVertices[0, 1] = new VertexPositionTexture(new Vector3(200, 200, 200) + position, new Vector2(0, 0));
            skyboxVertices[0, 2] = new VertexPositionTexture(new Vector3(-200, -200, 200) + position, new Vector2(1, 1));
            skyboxVertices[0, 3] = new VertexPositionTexture(new Vector3(-200, 200, 200) + position, new Vector2(1, 0));
            skyboxVertices[0, 4] = new VertexPositionTexture(new Vector3(-200, -200, 200) + position, new Vector2(1, 1));
            skyboxVertices[0, 5] = new VertexPositionTexture(new Vector3(200, 200, 200) + position, new Vector2(0, 0));


            // Back
            skyboxVertices[1, 0] = new VertexPositionTexture(new Vector3(-200, -200, -200) + position, new Vector2(0, 1));
            skyboxVertices[1, 1] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(1, 0));
            skyboxVertices[1, 2] = new VertexPositionTexture(new Vector3(200, -200, -200) + position, new Vector2(1, 1));
            skyboxVertices[1, 3] = new VertexPositionTexture(new Vector3(-200, 200, -200) + position, new Vector2(0, 0));
            skyboxVertices[1, 4] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(1, 0));
            skyboxVertices[1, 5] = new VertexPositionTexture(new Vector3(-200, -200, -200) + position, new Vector2(0, 1));


            // Right
            skyboxVertices[2, 0] = new VertexPositionTexture(new Vector3(200, -200, 200) + position, new Vector2(1, 1));
            skyboxVertices[2, 1] = new VertexPositionTexture(new Vector3(200, -200, -200) + position, new Vector2(0, 1));
            skyboxVertices[2, 2] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(0, 0));
            skyboxVertices[2, 3] = new VertexPositionTexture(new Vector3(200, -200, 200) + position, new Vector2(1, 1));
            skyboxVertices[2, 4] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(0, 0));
            skyboxVertices[2, 5] = new VertexPositionTexture(new Vector3(200, 200, 200) + position, new Vector2(1, 0));

            // Left
            skyboxVertices[3, 0] = new VertexPositionTexture(new Vector3(-200, 200, 200) + position, new Vector2(0, 0));
            skyboxVertices[3, 1] = new VertexPositionTexture(new Vector3(-200, 200, -200) + position, new Vector2(1, 0));
            skyboxVertices[3, 2] = new VertexPositionTexture(new Vector3(-200, -200, -200) + position, new Vector2(1, 1));
            skyboxVertices[3, 3] = new VertexPositionTexture(new Vector3(-200, 200, 200) + position, new Vector2(0, 0));
            skyboxVertices[3, 4] = new VertexPositionTexture(new Vector3(-200, -200, -200) + position, new Vector2(1, 1));
            skyboxVertices[3, 5] = new VertexPositionTexture(new Vector3(-200, -200, 200) + position, new Vector2(0, 1));

            // Ground
            skyboxVertices[4, 0] = new VertexPositionTexture(new Vector3(200, -200, -200) + position, new Vector2(1, 0));
            skyboxVertices[4, 1] = new VertexPositionTexture(new Vector3(-200, -200, 200) + position, new Vector2(0, 1));
            skyboxVertices[4, 2] = new VertexPositionTexture(new Vector3(-200, -200, -200) + position, new Vector2(0, 0));
            skyboxVertices[4, 3] = new VertexPositionTexture(new Vector3(-200, -200, 200) + position, new Vector2(0, 1));
            skyboxVertices[4, 4] = new VertexPositionTexture(new Vector3(200, -200, -200) + position, new Vector2(1, 0));
            skyboxVertices[4, 5] = new VertexPositionTexture(new Vector3(200, -200, 200) + position, new Vector2(1, 1));


            // Sky
            skyboxVertices[5, 0] = new VertexPositionTexture(new Vector3(-200, 200, 200) + position, new Vector2(1, 0));
            skyboxVertices[5, 1] = new VertexPositionTexture(new Vector3(200, 200, 200) + position, new Vector2(1, 1));
            skyboxVertices[5, 2] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(0, 1));
            skyboxVertices[5, 3] = new VertexPositionTexture(new Vector3(-200, 200, 200) + position, new Vector2(1, 0));
            skyboxVertices[5, 4] = new VertexPositionTexture(new Vector3(200, 200, -200) + position, new Vector2(0, 1));
            skyboxVertices[5, 5] = new VertexPositionTexture(new Vector3(-200, 200, -200) + position, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            // Set the camera's field of view
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            (float)this.graphics.GraphicsDevice.Viewport.Width /
            (float)this.graphics.GraphicsDevice.Viewport.Height, 1.0f, 400.0f);

            // Draw the skybox
            TempList = new VertexPositionTexture[6];
            for (int side = 0; side < 6; side++)
            {
                TempList[0] = skyboxVertices[side, 0];
                TempList[1] = skyboxVertices[side, 1];
                TempList[2] = skyboxVertices[side, 2];
                TempList[3] = skyboxVertices[side, 3];
                TempList[4] = skyboxVertices[side, 4];
                TempList[5] = skyboxVertices[side, 5];

                effect.Texture = sky_textures[side];
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, TempList, 0, 2);
                }

            }
        }
    }
}
