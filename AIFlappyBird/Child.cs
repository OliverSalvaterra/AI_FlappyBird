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

        public void Init()
        {
            X = 250;
            Y = 200;
            unAlived = false;
            velocity = 0f;
            acceleration = .3f;
        }
        
        public void UnAlive()
        {
            unAlived = true;
        }

        public void ReAlive()
        {
            unAlived = false;
        }

        public void VerticalMovement()
        {
            velocity += acceleration;
        }

        public void Jump()
        {
            velocity = -5;
        }

        public void Move()
        {
            VerticalMovement();
            Y += velocity;
        }
    }
}
