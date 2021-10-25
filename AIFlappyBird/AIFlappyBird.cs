using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NeuralNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIFlappyBird  
{
    public class AIFlappyBird : Game
    {
        int generation = 0;
        static int batchSize = 1000;
        GeneticTrain brains = new GeneticTrain(Fitness, new int[4] { 2, 6, 2, 1}, batchSize);
        double[][] fitnesses = Enumerable.Range(0, batchSize).Select(n => new double[1]).ToArray();
        int unAlivedCount = 0;

        public void Reset()
        {
            unAlivedCount = 0;
            watch.Restart();
            fitnessTimer.Restart();
            foreach(Child c in children)
            {
                c.Init(GraphicsDevice);
            }
            chairSpacing = 1000;
            
            topChairs.Clear();
            bottomChairs.Clear();
            topChairs.Enqueue(new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero) { SpriteEffect = SpriteEffects.FlipVertically });
            bottomChairs.Enqueue(new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero));
            topChairs.Peek().Init(GraphicsDevice, true, rnd);
            bottomChairs.Peek().Init(GraphicsDevice, false, rnd);
        }
        
        public static double Fitness(double[] timeTraveled)
        {
            return timeTraveled[0];
        }

        Random rnd = new Random();
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Child[] children = new Child[batchSize];
        Queue<Chair> topChairs = new Queue<Chair>();
        Queue<Chair> bottomChairs = new Queue<Chair>();
        int chairSpacing = 1000;
        double mutationRate = 0.1;

        Stopwatch watch = new Stopwatch();
        Stopwatch fitnessTimer = new Stopwatch();
        public AIFlappyBird()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 450;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            watch.Start();
            fitnessTimer.Start();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            topChairs.Enqueue(new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero) { SpriteEffect = SpriteEffects.FlipVertically });
            bottomChairs.Enqueue(new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero));

            topChairs.Peek().Init(GraphicsDevice, true, rnd);
            bottomChairs.Peek().Init(GraphicsDevice, false, rnd);

            for(int i = 0; i < children.Length; i++)
            {
                children[i] = new Child(Content.Load<Texture2D>("child"), Vector2.Zero);
                children[i].Init(GraphicsDevice);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            Chair topChair = topChairs.Peek();
            Chair bottomChair = bottomChairs.Peek();

            if (topChairs.Count < 7)
            {
                watch.Restart();
                float pos = topChairs.Last().Position.X + chairSpacing;

                Chair top = new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero) { SpriteEffect = SpriteEffects.FlipVertically };
                top.Init(GraphicsDevice, true, rnd);
                top.ResetPosition(pos);
                Chair bottom = new Chair(Content.Load<Texture2D>("highChair"), Vector2.Zero);
                bottom.Init(GraphicsDevice, false, rnd);
                bottom.ResetPosition(pos);

                chairSpacing -= chairSpacing < 300 ? 0 : 50;  

                topChairs.Enqueue(top);
                bottomChairs.Enqueue(bottom);
            }
            if (topChair.HitBox.Right < 0)
            {
                float pos = topChairs.Last().Position.X + chairSpacing;
                topChair.ResetPosition(pos);
                bottomChair.ResetPosition(pos);
                chairSpacing -= chairSpacing < 300 ? 0 : 50;

                topChairs.Dequeue();
                topChair.Rescale(rnd);
                topChairs.Enqueue(topChair);
                bottomChairs.Dequeue();
                bottomChair.Rescale(rnd);
                bottomChairs.Enqueue(bottomChair);
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Vector2 chairMidpoint = new Vector2(topChair.HitBox.Center.X, (topChair.HitBox.Center.Y + bottomChair.HitBox.Center.Y) / 2);

            for(int i = 0; i < children.Length; i++)
            {
                double[] distance = new double[] { children[i].Position.X - chairMidpoint.X, children[i].Position.Y - chairMidpoint.Y };

                double compute = brains.networks[i].Compute(distance)[0];
                bool toJumpornottoJump = compute > 0.91 ? true : false;
                

                if (children[i].HitBox.Bottom > GraphicsDevice.Viewport.Height || children[i].HitBox.Top < 0 || children[i].HitBox.Intersects(topChair.HitBox) || children[i].HitBox.Intersects(bottomChair.HitBox))
                {
                    children[i].UnAlive();
                    fitnesses[i][0] = fitnessTimer.ElapsedMilliseconds;
                }

                if (toJumpornottoJump)
                {
                    children[i].Jump();
                }

                foreach (Chair tChair in topChairs)
                {
                    if (children[i].HitBox.Intersects(tChair.HitBox))
                    {
                        children[i].UnAlive();
                    }
                }
                foreach (Chair bChair in bottomChairs)
                {
                    if (children[i].HitBox.Intersects(bChair.HitBox))
                    {
                        children[i].UnAlive();
                    }
                }
                children[i].Move();
            }

            foreach(Child c in children)
            {
                if (c.unAlived)
                {
                    unAlivedCount++;
                }
            }

            if(unAlivedCount >= batchSize)
            {
                Reset();
                brains.SetFitness(fitnesses);
                brains.Train(rnd, mutationRate);

                generation++;
                Window.Title = generation.ToString();
            }
            unAlivedCount = 0;

            foreach(Chair moveTop in topChairs)
            {
                moveTop.Move();
            }
            foreach(Chair moveBottom in bottomChairs)
            {
                moveBottom.Move();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach(Child childToDraw in children)
            {
                if (!childToDraw.unAlived)
                {
                    childToDraw.Draw(spriteBatch);
                }
            }

            foreach (Chair bottomChairToDraw in bottomChairs)
            {
                bottomChairToDraw.Draw(spriteBatch);
            }
            foreach (Chair topChairToDraw in topChairs)
            {
                topChairToDraw.Draw(spriteBatch);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
