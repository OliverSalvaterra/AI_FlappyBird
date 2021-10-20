using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIFlappyBird
{
    public class Child : Sprite
    {
        public bool unAlived = false;
        public float velocity;
        public float acceleration;

        public Child(Texture2D image, Vector2 position)
            : base(image, position) { }

        public Child(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint) { }

        public void init()
        {
            X = 250;
            Y = 200;
            unAlived = false;
            velocity = 0f;
            acceleration = .3f;
        }
        
        public void unAlive()
        {
            unAlived = true;
        }

        public void reAlive()
        {
            unAlived = false;
        }

        public void verticalMovement()
        {
            velocity += acceleration;
        }

        public void jump()
        {
            velocity = -5;
        }

        public void move()
        {
            verticalMovement();
            Y += velocity;
        }
    }
}
