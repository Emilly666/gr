using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.DirectoryServices.ActiveDirectory;

namespace test2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D paddle1, paddle2, background, ball;
        private int score1 = 0, score2 = 0;
        private bool gameGoing = false;
        private Vector2 ballPos, ballVec = new Vector2(0,0);
        private int paddle1Y, paddle2Y;
        Random random = new Random();
        private float speed;

        private float speedDef = 4f;
        private float speedIncrease = 1.1f;
        private int PADDLE_DISTANCE = 5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            paddle1Y = GraphicsDevice.Viewport.Height / 2 - 50;
            paddle2Y = GraphicsDevice.Viewport.Height / 2 - 50;
            ballPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - 20, GraphicsDevice.Viewport.Height / 2 - 20);
            speed = speedDef;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>(@"pongBackground");
            paddle1 = Content.Load<Texture2D>(@"paddle1");
            paddle2 = Content.Load<Texture2D>(@"paddle2");
            ball = Content.Load<Texture2D>(@"pongBall");
            font = Content.Load<SpriteFont>(@"File");
        }

        protected override void Update(GameTime gameTime)
        {
            //press escape to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //paddle1 movement
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.Q)) { }
            else if (Keyboard.GetState().IsKeyDown(Keys.Q) && paddle1Y >= PADDLE_DISTANCE) { paddle1Y -= (int)speedDef; } //up
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && paddle1Y <= GraphicsDevice.Viewport.Height - 100 - PADDLE_DISTANCE) { paddle1Y += (int)speedDef; } //down
            
            //paddle2 movement
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.Q)) { }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && paddle2Y >= PADDLE_DISTANCE) { paddle2Y -= (int)speedDef; } //up
            else if (Keyboard.GetState().IsKeyDown(Keys.L) && paddle2Y <= GraphicsDevice.Viewport.Height - 100 - PADDLE_DISTANCE) { paddle2Y += (int)speedDef; } //down
            
            //press space to start game
            if (!gameGoing && Keyboard.GetState().IsKeyDown(Keys.Space)) { startGame(); }

            //move ball
            ballPos += ballVec;

            //bounce ball form top and bottom
            if (ballPos.Y > Window.ClientBounds.Height - ball.Height || ballPos.Y < 0) { ballVec.Y *= -1; }

            //check if either player won
            if (ballPos.X > Window.ClientBounds.Width - ball.Width - PADDLE_DISTANCE) { endGame(true); }
            else if (ballPos.X < PADDLE_DISTANCE) { endGame(false); }

            //check if ball touched paddle1
            if (new Rectangle((int)ballPos.X, (int)ballPos.Y, ball.Width, ball.Height).Intersects(new Rectangle(PADDLE_DISTANCE, paddle1Y, paddle1.Width, paddle1.Height)) ||
                new Rectangle((int)ballPos.X, (int)ballPos.Y, ball.Width, ball.Height).Intersects(new Rectangle(GraphicsDevice.Viewport.Width - PADDLE_DISTANCE - paddle2.Width, paddle2Y, paddle2.Width, paddle2.Height)))
            {
                ballVec.X *= -1;
                ballVec *= speedIncrease;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //display background
            spriteBatch.Draw(
                background,
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                Color.White
            );
            //display paddle 1
            spriteBatch.Draw(
                paddle1,
                new Rectangle(PADDLE_DISTANCE, paddle1Y, 10, 100),
                Color.White
            );
            //display paddle 2
            spriteBatch.Draw(
                paddle2,
                new Rectangle(GraphicsDevice.Viewport.Width - PADDLE_DISTANCE - 10, paddle2Y, 10, 100),
                Color.White
            );
            //display score
            spriteBatch.DrawString(font, score1.ToString(), new Vector2(GraphicsDevice.Viewport.Width / 2 - 30 - font.MeasureString(score1.ToString()).X, 2), Color.White);
            spriteBatch.DrawString(font, score2.ToString(), new Vector2(GraphicsDevice.Viewport.Width / 2 + 30, 2), Color.White);

            //display ball
            spriteBatch.Draw(ball, ballPos, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void startGame()
        {
            gameGoing = true;
            float rnd;
            if (random.Next(2) == 0) { rnd = getRandomNumber(-0.2f, 0.3f); } //start to the left
            else { rnd = getRandomNumber(1.2f, 0.8f); } //start to the right
            ballVec = new Vector2(MathF.Cos(rnd) * speed, MathF.Sin(rnd) * speed);
        }
        private void endGame(bool player1Win)
        {
            ballPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - 20, GraphicsDevice.Viewport.Height / 2 - 20);
            ballVec = new Vector2(0, 0);
            gameGoing = false;
            speed = speedDef;
            if (player1Win) { score1++; } else { score2++; }
        }

        public float getRandomNumber(float min, float max)
        {
            return (float)random.NextDouble() * (max * MathF.PI - min * MathF.PI) + min * MathF.PI;
        }
    }
}