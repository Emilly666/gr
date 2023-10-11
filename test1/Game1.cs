using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;


namespace test1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background, ghost, ghostDead;
        private int ghostsSize = 50;
        private Random rand = new Random();
        private List<Rectangle> ghostsRectangles = new List<Rectangle>();
        private List<float> ghostsTimers = new List<float>();
        private List<float> ghostsTimeRemaining = new List<float>();

        private List<Rectangle> deadGhosts = new List<Rectangle>();
        float timeRemaining = 0.0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //_graphics.PreferredBackBufferWidth = 1920;
            //_graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            this.Window.AllowUserResizing = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            ghostsRectangles.Add(new Rectangle(rand.Next(0, GraphicsDevice.Viewport.Width - ghostsSize),
                    rand.Next(0, GraphicsDevice.Viewport.Height - ghostsSize), ghostsSize, ghostsSize));
            float time = (float)rand.Next(0, 20) / 10;
            ghostsTimers.Add(time);
            ghostsTimeRemaining.Add(time);
            timeRemaining = 0.5f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            background = Content.Load<Texture2D>(@"street");
            ghost = Content.Load<Texture2D>(@"ghost");
            ghostDead = Content.Load<Texture2D>(@"ghost-foot");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (timeRemaining == 0.0f) 
            {
                ghostsRectangles.Add(new Rectangle(rand.Next(0, GraphicsDevice.Viewport.Width - ghostsSize),
                   rand.Next(0, GraphicsDevice.Viewport.Height - ghostsSize), ghostsSize, ghostsSize));
                float time = (float)rand.Next(0, 20) / 10;
                ghostsTimers.Add(time);
                ghostsTimeRemaining.Add(time);
                timeRemaining = 0.5f;
            }
            timeRemaining = MathHelper.Max(0, timeRemaining - (float)gameTime.ElapsedGameTime.TotalSeconds);

            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < ghostsRectangles.Count; i++)
            {
                if (ghostsTimeRemaining[i] == MathHelper.Max(0, ghostsTimeRemaining[i] - (float)gameTime.ElapsedGameTime.TotalSeconds)) 
                {
                    ghostsRectangles.Remove(ghostsRectangles[i]);
                    ghostsTimers.Remove(ghostsTimers[i]);
                    ghostsTimeRemaining.Remove(ghostsTimeRemaining[i]);
                    continue;
                }
                if ((mouse.LeftButton == ButtonState.Pressed) && (ghostsRectangles[i].Contains(mouse.X, mouse.Y)))
                {
                    deadGhosts.Add(ghostsRectangles[i]);
                    ghostsTimeRemaining[i] = 0.0f;
                    timeRemaining = 0.0f;
                    ghostsRectangles.Remove(ghostsRectangles[i]);
                    ghostsTimers.Remove(ghostsTimers[i]);
                    ghostsTimeRemaining.Remove(ghostsTimeRemaining[i]);
                    continue;
                }
                ghostsTimeRemaining[i] = MathHelper.Max(0, ghostsTimeRemaining[i] - (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            
          

            this.Window.Title = "score: " + deadGhosts.Count.ToString();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(background,
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 
                Color.White);


            for (int i = 0; i < deadGhosts.Count; i++)
            {
                _spriteBatch.Draw(ghostDead, deadGhosts[i], Color.White);
            }

            for (int i = 0; i < ghostsRectangles.Count; i++)
            {
                _spriteBatch.Draw(ghost, ghostsRectangles[i], Color.White);
            };

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}