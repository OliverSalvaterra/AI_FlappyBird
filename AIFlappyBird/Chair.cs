using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIFlappyBird
{
    class Chair : Sprite
    {
        public bool destroy = false;

        public Chair(Texture2D image, Vector2 position)
            : base(image, position) { }

        public Chair(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint) { }

        public void destroyer()
        {
            destroy = true;
        }

        public void move()
        {
            X -= 16;
        }

        public void resetPosition(float pos)
        {
            X = pos;
        }

        public void init(GraphicsDevice g, bool top, Random rnd)
        {
            rescale(rnd);
            X = g.Viewport.Width;

            if (top)
            {
                Y = 0 - (int)scale.Y*20;
            }
            else
            {
                Y = g.Viewport.Height - (150 + (int)scale.Y*25);
            }
        }

        public void rescale(Random rnd)
        {
            scale.Y = (float)rnd.NextDouble(0.8, 1.6);
        }
    }
}
