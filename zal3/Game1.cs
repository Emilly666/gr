using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace zal3
{
    public class Grid
    {
        public List<VertexPositionColor> axisLines = new List<VertexPositionColor>();
        public int axisLenght = 25;
        public Grid()
        {
            for (int i = -25; i < 26; i++)
            {
                axisLines.Add(new VertexPositionColor(new Vector3(axisLenght, 0, i), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(-axisLenght, 0, i), Color.Gray));
            }
            for (int i = -25; i < 26; i++)
            {
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, axisLenght), Color.Gray));
                axisLines.Add(new VertexPositionColor(new Vector3(i, 0, -axisLenght), Color.Gray));
            }
        }
    }
    public class Cube
    {
        protected VertexPositionColor[] verticesDef = new VertexPositionColor[8];
        public VertexPositionColor[] vertices;
        public Vector3 position;
        public Vector3 positionDef = Vector3.Zero;
        public float distance;
        protected float selfRotationSpeed;
        protected float sunRotationSpeed;

        public short[] indices = {
            3, 2, 6, 7, 4, 2, 0, 3, 1, 6, 5, 4, 1, 0
        };
        public Cube(float distance, float size, Color color1, Color color2, float selfRotationSpeed, float sunRotationSpeed)
        {
            float h = size/2;
            verticesDef[0] = new VertexPositionColor(new Vector3(h, h, h), color2);
            verticesDef[1] = new VertexPositionColor(new Vector3(-h, h, h), color1);
            verticesDef[2] = new VertexPositionColor(new Vector3(h, h, -h), color1);
            verticesDef[3] = new VertexPositionColor(new Vector3(-h, h, -h), color2);
            verticesDef[4] = new VertexPositionColor(new Vector3(h, -h, h), color2);
            verticesDef[5] = new VertexPositionColor(new Vector3(-h, -h, h), color2);
            verticesDef[6] = new VertexPositionColor(new Vector3(-h, -h, -h), color1);
            verticesDef[7] = new VertexPositionColor(new Vector3(h, -h, -h), color2);

            this.selfRotationSpeed = selfRotationSpeed;
            this.sunRotationSpeed = sunRotationSpeed;
            positionDef.X = distance;
            position = positionDef;
            this.distance = distance;
            vertices = (VertexPositionColor[])verticesDef.Clone();
        }
        public virtual VertexPositionColor[] Move(float seconds, Vector3 earthPosition)
        {
            vertices = (VertexPositionColor[])verticesDef.Clone();

            Matrix selfRotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(seconds * selfRotationSpeed * 100));
            Matrix sunRotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(seconds * sunRotationSpeed * 15));
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = Vector3.Transform(verticesDef[i].Position, selfRotationMatrix); //self rotation=
                vertices[i].Position.X += distance; //move by distance
                vertices[i].Position = Vector3.Transform(vertices[i].Position, sunRotationMatrix); //sun rotation

                position = Vector3.Transform(positionDef, sunRotationMatrix);
            }
            return vertices;
        }
    }
    public class Sun : Cube
    {
        public Sun(float distance, float size, Color color1, Color color2) : base(0, size, color1, color2, 0, 0) { }
        
        override public VertexPositionColor[] Move(float seconds, Vector3 earthPosition)
        {
            float rotX = MathHelper.ToRadians(30f * MathF.Sin(3f * seconds));
            float rotY = MathHelper.ToRadians(30f * MathF.Sin(4f * seconds + MathHelper.PiOver2));
            Matrix lissajousMatrix = Matrix.CreateRotationX(rotX) * Matrix.CreateRotationY(rotY);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = Vector3.Transform(verticesDef[i].Position, lissajousMatrix);
            }
            return vertices;
        }
    }
    public class Moon : Cube
    {
        public Moon(float distance, float size, Color color1, Color color2, float selfRotationSpeed, float sunRotationSpeed) 
            : base(distance, size, color1, color2, selfRotationSpeed, sunRotationSpeed) { }

        override public VertexPositionColor[] Move(float seconds, Vector3 earthPosition)
        {
            vertices = (VertexPositionColor[])verticesDef.Clone();
            Matrix selfRotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(seconds * selfRotationSpeed * 100));
            Matrix earthRotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(seconds * sunRotationSpeed * 15));

            for (int i = 0; i < vertices.Length; i++)
            {

                vertices[i].Position = Vector3.Transform(verticesDef[i].Position, selfRotationMatrix); //self rotation
                vertices[i].Position.X += distance; //move by distance
                vertices[i].Position = Vector3.Transform(vertices[i].Position, earthRotationMatrix); //earth rotation

                vertices[i].Position += earthPosition; //move by earth distance
            }
            return vertices;
        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private Texture2D background;

        private Matrix worldMatrix, viewMatrix, projectionMatrix;


        private float viewAngleX = 0f, viewAngleY = 0f, viewDistance = 20f;
        private bool lastXState = false, displayGrid = true, lastBState = false, displayBackground = true;

        private Grid grid;
        private Cube[] cubes;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            grid = new Grid();
            cubes = new Cube[]{ 
                new Sun(0f, 4f, Color.Yellow, Color.Orange), //sun
                new Cube(6f, 0.4f, Color.Gray, Color.Brown, 0.9f, 3f), //Mercury
                new Cube(10f, 2f, Color.Orange, Color.Gray, 0.6f, 1f), //Venus
                new Cube(15f, 2f, Color.Green, Color.Blue, 1f, 5f), //Earth
                new Moon(2.5f, 0.5f, Color.White, Color.Gray, 3f, 10f), //Moon
                new Cube(20f, 1f, Color.Brown, Color.DarkRed, 1.3f, 5f) //Mars
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;

            background = Content.Load<Texture2D>(@"stars");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape)) { Exit(); }
            if (keyboard.IsKeyDown(Keys.Right)) { viewAngleY += 0.02f; }
            if (keyboard.IsKeyDown(Keys.Left)) { viewAngleY -= 0.02f; }
            if (keyboard.IsKeyDown(Keys.Up)) { viewAngleX += 0.02f; }
            if (keyboard.IsKeyDown(Keys.Down)) { viewAngleX -= 0.02f; }

            if (keyboard.IsKeyDown(Keys.A)) { viewDistance += 0.1f; }
            if (keyboard.IsKeyDown(Keys.Q)) { viewDistance -= 0.1f; }

            if (!displayBackground && lastBState == false && keyboard.IsKeyDown(Keys.B)) { displayBackground = true; }
            else if (displayBackground && lastBState == false && keyboard.IsKeyDown(Keys.B)) { displayBackground = false; }
            lastBState = keyboard.IsKeyDown(Keys.B);

            if (!displayGrid && lastXState == false && keyboard.IsKeyDown(Keys.X)) { displayGrid = true; }
            else if (displayGrid && lastXState == false && keyboard.IsKeyDown(Keys.X)) { displayGrid = false; }
            lastXState = keyboard.IsKeyDown(Keys.X);

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.CreateLookAt(new Vector3(viewDistance, viewDistance, viewDistance), Vector3.Zero, Vector3.Up);
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

            //display background
            if (displayBackground) {
                _spriteBatch.Draw(
                    background,
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                    Color.White
                );
            }
            
            _spriteBatch.End();

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _basicEffect.CurrentTechnique.Passes[0].Apply();

            //display grid
            if (displayGrid) {
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, grid.axisLines.ToArray(), 0, grid.axisLines.Count / 2);
            }

            //display cube
            for (int i = 0; i < cubes.Length; i++)
            {
                VertexPositionColor[] temp = cubes[i].Move((float)gameTime.TotalGameTime.TotalSeconds, cubes[3].position);
                GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleStrip,
                    temp, 
                    0,
                    temp.Length,
                    cubes[i].indices, 
                    0,
                    cubes[i].indices.Length - 2);
            }
            base.Draw(gameTime);
        }
    }
}