using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetworkLibrary;
using System;

namespace AIFlappyBird
{
    class Chair : Sprite
    {
        public bool destroy = false;
        public int moveSpeed = 16;
        private int imageHeight = 150;
        private int scalar = 20;

        public Chair(Texture2D image, Vector2 position)
            : base(image, position) { }

        public Chair(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint) { }

        public void Move()
        {
            X -= moveSpeed; // chair moves accross screen at moveSpeed per frame
        }

        public void ResetPosition(float pos)
        {
            X = pos;
        }

        public void Init(GraphicsDevice graphics, bool top, Random rnd)
        {
            Rescale(rnd, graphics);
            X = graphics.Viewport.Width;
            float screenScale = graphics.Viewport.Height / 400;

            if (top)
            {
                Y = 0 - 1.05f*(int)scale.Y*scalar*screenScale;
            }
            else
            {
                Y = graphics.Viewport.Height - 1.3f*(imageHeight + (int)scale.Y*scalar*screenScale);
            }
        }

        public void Rescale(Random rnd, GraphicsDevice graphics)
        {
            scale.Y = (float)rnd.NextDouble(0.8, 1.6) * (graphics.Viewport.Height / 380);
            scale.X = graphics.Viewport.Width / 800;
        }
    }
}
