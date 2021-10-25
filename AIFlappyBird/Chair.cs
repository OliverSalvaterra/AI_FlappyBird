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
            Rescale(rnd);
            X = graphics.Viewport.Width;

            if (top)
            {
                Y = 0 - (int)scale.Y*scalar;
            }
            else
            {
                Y = graphics.Viewport.Height - (imageHeight + (int)scale.Y*scalar);
            }
        }

        public void Rescale(Random rnd)
        {
            scale.Y = (float)rnd.NextDouble(0.8, 1.6);
        }
    }
}
