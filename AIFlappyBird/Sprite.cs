using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIFlappyBird
{
    public class Sprite
    {
        private Texture2D image;
        private Vector2 position;
        public Vector2 Position => position;
        public Color Tint { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public float Rotation { get; set; } = 0f;
        public Vector2 Origin { get; set; } = Vector2.Zero;

        protected Vector2 scale = Vector2.One;
        public Vector2 Scale => scale;

        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public Rectangle HitBox => new Rectangle((int)Position.X, (int)Position.Y, (int)(image.Width * Scale.X), (int)(image.Height * Scale.Y));

        public Sprite(Texture2D image, Vector2 position)
            : this(image, position, tint: Color.White) { }

        public Sprite(Texture2D image, Vector2 position, Color tint)
        {
            this.image = image;
            this.position = position;
            Tint = tint;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, null, Tint, Rotation, Origin, Scale, SpriteEffect, layerDepth: 0f);
        }
    }
}
