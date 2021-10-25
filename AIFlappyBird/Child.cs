using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIFlappyBird
{
    public class Child : Sprite
    {
        public bool unAlived = false;
        public float velocity;
        public float acceleration;
        public int jumpStrength = -5;

        public Child(Texture2D image, Vector2 position)
            : base(image, position) { }

        public Child(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint) { }

        public void Init(GraphicsDevice graphics)
        {
            X = (graphics.Viewport.Width / 2) - 200; // initializes x position some amount away from center
            Y = graphics.Viewport.Height / 2; // initializes y position at center
            unAlived = false;
            velocity = 0f; // initializes velocity as 0
            acceleration = .3f; // sets acceleration to .3
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
            velocity += acceleration; // updates velocity by acceleration
        }

        public void Jump()
        {
            velocity = jumpStrength;
        }

        public void Move()
        {
            VerticalMovement();
            Y += velocity; // updates position by velocity
        }
    }
}
