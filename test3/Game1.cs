using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Transactions;

namespace wpr3d
{
    public class Grid
    {
        public VertexPositionColor[] mainAxisLines = {
            new VertexPositionColor(-5 * Vector3.UnitX, Color.Red),
            new VertexPositionColor(5 * Vector3.UnitX, Color.Red),
            new VertexPositionColor(-5 * Vector3.UnitY, Color.Green),
            new VertexPositionColor(5 * Vector3.UnitY, Color.Green),
            new VertexPositionColor(-5 * Vector3.UnitZ, Color.Blue),
            new VertexPositionColor(5 * Vector3.UnitZ, Color.Blue)
        };
        public List<VertexPositionColor> axisLines = new List<VertexPositionColor>();
        public int axisLenght = 5;
        public Grid() 
        {
            for (int i = 1; i < 6; i++) 
            {
                axisLines.Add(new VertexPositionColor(new Vector3(axisLenght, 0, i), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(-axisLenght, 0, i), Color.Gray));
            }
            for (int i = -1; i > -6; i--)
            {
                axisLines.Add(new VertexPositionColor(new Vector3(axisLenght, 0, i), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(-axisLenght, 0, i), Color.Gray));
            }
            for (int i = 1; i < 6; i++)
            {
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, axisLenght), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, -axisLenght), Color.Gray));
            }
            for (int i = -1; i > -6; i--)
            {
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, axisLenght), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, -axisLenght), Color.Gray));
            }
        }
    }
    public class Cube
    {
        public Vector3 position = new Vector3(0,0,0);
        public VertexPositionColor[] verticesDef = {
            // Front face
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), Color.Red),
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), Color.Red),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), Color.Red),
            new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), Color.Red),
            // Back face
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), Color.Orange),
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), Color.Orange),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), Color.Orange),
            new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), Color.Orange),
            // Top face
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), Color.Yellow),
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), Color.Yellow),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), Color.Yellow),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), Color.Yellow),
            // Bottom face
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), Color.Green),
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), Color.Green),
            new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), Color.Green),
            new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), Color.Green),
            // Left face
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), Color.Blue),
            new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), Color.Blue),
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), Color.Blue),
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), Color.Blue),
            // Right face
            new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), Color.Indigo),
            new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), Color.Indigo),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), Color.Indigo),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), Color.Indigo)
        };
         public short[] indices = {
            0, 1, 2, 2, 3, 0, // Front face
            4, 6, 5, 6, 4, 7, // Back face
            8, 9, 10, 10, 11, 8, // Top face
            12, 14, 13, 14, 12, 15, // Bottom face
            16, 17, 18, 18, 19, 16, // Left face
            20, 22, 21, 22, 20, 23  // Right face
        };
        public VertexPositionColor[] vertices;

        public Cube()
        {
            vertices = (VertexPositionColor[])verticesDef.Clone();
        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private SpriteFont _font;

        private Matrix worldMatrix, viewMatrix, projectionMatrix;

        
        private float viewAngleX = 0f, viewAngleY = 0f;
        private string keys = "";
        private bool rotation = false;
        private bool lastRState = false;
        private Cube cube;
        private Grid grid;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _font = Content.Load<SpriteFont>(@"File");

            grid = new Grid();
            cube = new Cube();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            keys = "";
            if (keyboard.IsKeyDown(Keys.Escape)) { Exit(); }
            if (keyboard.IsKeyDown(Keys.Right)) { viewAngleY += 0.02f; keys += "Right, "; }
            if (keyboard.IsKeyDown(Keys.Left)) { viewAngleY -= 0.02f; keys += "Left, "; }
            if (keyboard.IsKeyDown(Keys.Up)) { viewAngleX += 0.02f; keys += "Up, "; }
            if (keyboard.IsKeyDown(Keys.Down)) { viewAngleX -= 0.02f; keys += "Down, "; }

            if (keyboard.IsKeyDown(Keys.W)) { cube.position.Z += 0.02f; keys += "W, "; }
            if (keyboard.IsKeyDown(Keys.S)) { cube.position.Z -= 0.02f; keys += "S, "; }
            if (keyboard.IsKeyDown(Keys.A)) { cube.position.X += 0.02f; keys += "A, "; }
            if (keyboard.IsKeyDown(Keys.D)) { cube.position.X -= 0.02f; keys += "D, "; }
            if (keyboard.IsKeyDown(Keys.Q)) { cube.position.Y += 0.02f; keys += "Q, "; }
            if (keyboard.IsKeyDown(Keys.E)) { cube.position.Y -= 0.02f; keys += "E, "; }

            for (int i = 0; i < cube.vertices.Length; i++)
            {
                cube.vertices[i].Position = cube.verticesDef[i].Position + cube.position;
            }

            if (keys.Length > 0) { keys = keys.Substring(0, keys.Length - 2); }

            if (!rotation && lastRState == false && keyboard.IsKeyDown(Keys.R)) { rotation = true; }
            else if (rotation && lastRState == false && keyboard.IsKeyDown(Keys.R)) { rotation = false; }
            lastRState = keyboard.IsKeyDown(Keys.R);

            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(7f, 7f, 7f), Vector3.Zero, Vector3.Up);
            viewMatrix = Matrix.CreateRotationX(viewAngleX) * Matrix.CreateRotationY(viewAngleY) * viewMatrix;
            
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);



            _basicEffect.World = worldMatrix;
            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //display keys
            _spriteBatch.DrawString(_font, "keys: " + keys, new Vector2(5, 0), Color.Black);
            //display position
            _spriteBatch.DrawString(_font, "pos = " + cube.position.ToString(), new Vector2(5, _font.LineSpacing), Color.Black);
            //display rotation
            _spriteBatch.DrawString(_font, rotation.ToString(), new Vector2(5, 2*_font.LineSpacing), Color.Black);

            _spriteBatch.End();

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            //display main axis lines
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, grid.mainAxisLines, 0, 3);
            //display grid
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, grid.axisLines.ToArray(), 0, grid.axisLines.Count / 2);
            //display cube
            GraphicsDevice.DrawUserIndexedPrimitives(
            PrimitiveType.TriangleList, cube.vertices, 0, cube.vertices.Length,
            cube.indices, 0, cube.indices.Length / 3);

            base.Draw(gameTime);
        }
    }
}