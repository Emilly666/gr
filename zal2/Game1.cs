using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace test2
{
    public class Ball 
    {
        private static readonly float SPEED_INCREASE = 1.1f;
        public readonly int millisecondsPerFrame = 16;
        public readonly float SPEED_DEF = 4f;

        public Vector2 position, positionDef, movement = new Vector2(0, 0);

        public Point frameSize = new Point(64, 64);
        public Point currentFrame = new Point(0, 0);
        public Point sheetSize = new Point(16, 5);
        public int timeSinceLastFrame = 0;

        public Ball(Vector2 p) 
        { 
            positionDef = p;
            position = p;
        }

        public void Move() { position += movement; }
        public void Bounce(float hit, int player)
        {
            float temp;
            if (hit <= 30) { temp = -0.9f; }//top edge
            else if(hit <= 60 && hit > 30) { temp = -0.6f; }//top
            else if (hit < 90 && hit > 60) { temp = 0f; }//middle
            else if (hit < 120 && hit >= 90) { temp = 0.6f; }//bottom
            else { temp = 0.9f; }//edge bottom

            movement *= SPEED_INCREASE;
            movement = new Vector2(MathF.Cos(temp) * movement.Length(), MathF.Sin(temp) * movement.Length());
            movement.X *= player;
        }
        public void Reset()
        {
            position = positionDef;
            movement = new Vector2(0, 0);
        }
    }
    public class Player
    {
        public Rectangle rectangle;
        public int score = 0;
        public float colorTime = 0;

        public Player(int x, int y, int w, int h) 
        { 
            rectangle = new Rectangle(x, y, w, h);
        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D paddle1, paddle2, background, ballT, explosion;
        private Random random = new Random();

        private Ball ball;
        private Player player1, player2;

        private bool gameGoing = false;

        private static readonly float SPEED_DEF = 4f;
        private static readonly int PADDLE_DISTANCE = 5;
        private static readonly int BALL_MARGIN = 10;

        public Point eFrameSize = new Point(64, 64);
        public Point eCurrentFrame = new Point(0, 0);
        public Point eSheetSize = new Point(5, 5);
        public int eTimeSinceLastFrame = 0;
        public readonly int eMillisecondsPerFrame = 30;
        public bool displayExplosion = false;
        public Rectangle explosionRec;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            IsFixedTimeStep = false;
        }
        protected override void Initialize()
        {
            ball = new Ball(
                new Vector2((GraphicsDevice.Viewport.Width - 64) / 2, (GraphicsDevice.Viewport.Height - 64) / 2) 
                );
            player1 = new Player(
                0 + PADDLE_DISTANCE, (GraphicsDevice.Viewport.Height - 150) / 2, 
                20, 
                150);
            player2 = new Player(
                GraphicsDevice.Viewport.Width - PADDLE_DISTANCE - 20, (GraphicsDevice.Viewport.Height - 150) / 2, 
                20, 
                150);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>(@"pongBackground");
            paddle1 = Content.Load<Texture2D>(@"paddle1a");
            paddle2 = Content.Load<Texture2D>(@"paddle2a");
            ballT = Content.Load<Texture2D>(@"ball-anim");
            font = Content.Load<SpriteFont>(@"File");
            explosion = Content.Load<Texture2D>(@"explosion64");
        }
        protected override void Update(GameTime gameTime)
        {
            //press escape to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //paddle1 movement
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.Q)) { }
            else if (Keyboard.GetState().IsKeyDown(Keys.Q) && player1.rectangle.Top >= PADDLE_DISTANCE) 
                { player1.rectangle.Y -= (int)SPEED_DEF; } //up
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && player1.rectangle.Bottom <= GraphicsDevice.Viewport.Height - PADDLE_DISTANCE) 
                { player1.rectangle.Y += (int)SPEED_DEF; } //down
            
            //paddle2 movement
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.Q)) { }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && player2.rectangle.Top >= PADDLE_DISTANCE) 
                { player2.rectangle.Y -= (int)SPEED_DEF; } //up
            else if (Keyboard.GetState().IsKeyDown(Keys.L) && player2.rectangle.Bottom <= GraphicsDevice.Viewport.Height - PADDLE_DISTANCE) 
                { player2.rectangle.Y += (int)SPEED_DEF; } //down
            
            //press space to start game
            if (!gameGoing && Keyboard.GetState().IsKeyDown(Keys.Space)) { StartGame(); }

            //bounce ball form top and bottom
            if (ball.position.Y - BALL_MARGIN > Window.ClientBounds.Height - ball.frameSize.X || ball.position.Y + BALL_MARGIN < 0) 
                { ball.movement.Y *= -1; }

            //check if ball touched paddle1
            if (new Rectangle((int)ball.position.X + BALL_MARGIN, (int)ball.position.Y + BALL_MARGIN, 
                ball.frameSize.X - (2 * BALL_MARGIN), ball.frameSize.Y - (2 * BALL_MARGIN)).Intersects(player1.rectangle))
            {
                ball.Bounce(ball.position.Y - player1.rectangle.Y + ball.frameSize.Y / 2, 1);
                player1.colorTime = 0.3f;
                Window.Title = (ball.position.Y - player1.rectangle.Y + ball.frameSize.Y / 2).ToString();
            }//check if ball touched paddle2
            else if(new Rectangle((int)ball.position.X + BALL_MARGIN, (int)ball.position.Y + BALL_MARGIN, 
                ball.frameSize.X - (2 * BALL_MARGIN), ball.frameSize.Y - (2 * BALL_MARGIN)).Intersects(player2.rectangle)) 
            { 
                ball.Bounce(ball.position.Y - player2.rectangle.Y + ball.frameSize.Y / 2, -1);
                player2.colorTime = 0.3f;
            }
            //check if either player won
            else if (ball.position.X > Window.ClientBounds.Width - ball.frameSize.X) { EndGame(true); }
            else if (ball.position.X < 0) { EndGame(false); }

            if (gameGoing) { ball.Move();  }
            
            //update paddle colours
            player1.colorTime = MathHelper.Max(0, player1.colorTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
            player2.colorTime = MathHelper.Max(0, player2.colorTime - (float)gameTime.ElapsedGameTime.TotalSeconds);

            //animate ball
            ball.timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (ball.timeSinceLastFrame > ball.millisecondsPerFrame) 
            {
                ball.timeSinceLastFrame -= ball.millisecondsPerFrame;
                ball.currentFrame.X++;
                if (ball.currentFrame.X == 7 && ball.currentFrame.Y == ball.sheetSize.Y - 1)
                {
                    ball.currentFrame.X = 0;
                    ball.currentFrame.Y = 0;
                }
                else if (ball.currentFrame.X >= ball.sheetSize.X)
                {
                    ball.currentFrame.X = 0;
                    ball.currentFrame.Y++;
                    if (ball.currentFrame.Y >= ball.sheetSize.Y)
                    {
                        ball.currentFrame.Y = 0;
                    }
                }
            }
            //update explosion animation
            eTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (eTimeSinceLastFrame > eMillisecondsPerFrame)
            {
                eTimeSinceLastFrame -= eMillisecondsPerFrame;
                eCurrentFrame.X++;
                if (eCurrentFrame.X >= eSheetSize.X)
                {
                    eCurrentFrame.X = 0;
                    eCurrentFrame.Y++;
                    if (eCurrentFrame.Y >= ball.sheetSize.Y)
                    {
                        eCurrentFrame.Y = 0;
                        displayExplosion = false;
                    }
                }
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
                Color.White);
            //display paddle 1
            spriteBatch.Draw(
                paddle1,
                player1.rectangle,
                Color.White);
            //display paddle 1 coulor
            if (player1.colorTime > 0)
            {
                spriteBatch.Draw(
                paddle1,
                player1.rectangle,
                Color.Green * 0.5f);
            }
            //display paddle 2
            spriteBatch.Draw(
                paddle2,
                player2.rectangle,
                Color.White);
            //display paddle 2 coulor
            if(player2.colorTime > 0) 
            {
                spriteBatch.Draw(
                paddle2,
                player2.rectangle,
                Color.Green * 0.5f);
            }
            //display score
            spriteBatch.DrawString(
                font, 
                player1.score.ToString(), 
                new Vector2(GraphicsDevice.Viewport.Width / 2 - 30 - font.MeasureString(player1.score.ToString()).X, 2), 
                Color.White);
            spriteBatch.DrawString(
                font, 
                player2.score.ToString(), 
                new Vector2(GraphicsDevice.Viewport.Width / 2 + 30, 2), 
                Color.White);
            //display ball
            spriteBatch.Draw(
                ballT, 
                ball.position, 
                new Rectangle(ball.currentFrame.X * ball.frameSize.X, ball.currentFrame.Y * ball.frameSize.Y, ball.frameSize.X, ball.frameSize.Y),
                Color.White);
            //display explosion
            if (displayExplosion) 
            {
                spriteBatch.Draw(
                    explosion,
                    explosionRec,
                    new Rectangle(eCurrentFrame.X * eFrameSize.X, eCurrentFrame.Y * eFrameSize.Y, eFrameSize.X, eFrameSize.Y),
                    Color.White
                    );
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void StartGame()
        {
            gameGoing = true;
            float rnd;
            if (random.Next(2) == 0) { rnd = GetRandomNumber(-0.2f, 0.3f); } //start to the left
            else { rnd = GetRandomNumber(1.2f, 0.8f); } //start to the right
            ball.movement = new Vector2(MathF.Cos(rnd) * ball.SPEED_DEF, MathF.Sin(rnd) * ball.SPEED_DEF);
        }
        private void EndGame(bool player1Win)
        {
            displayExplosion = true;
            explosionRec = new Rectangle((int)ball.position.X, (int)ball.position.Y, ball.frameSize.X, ball.frameSize.Y);
            ball.Reset();
            gameGoing = false;
            if (player1Win) { player1.score++; } else { player2.score++; }
        }
        public float GetRandomNumber(float min, float max)
        {
            return (float)random.NextDouble() * (max * MathF.PI - min * MathF.PI) + min * MathF.PI;
        }
    }
}