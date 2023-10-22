using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test4
{
    public class Axes
    {
        public VertexPositionColor[] points = new VertexPositionColor[6];
        public Axes(float lenght, Vector3 point)
        {
            points[0] = new VertexPositionColor(-lenght * Vector3.UnitX + point, Color.Red);
            points[1] = new VertexPositionColor(lenght * Vector3.UnitX + point, Color.Red);
            points[2] = new VertexPositionColor(-lenght * Vector3.UnitY + point, Color.Green);
            points[3] = new VertexPositionColor(lenght * Vector3.UnitY + point, Color.Green);
            points[4] = new VertexPositionColor(-lenght * Vector3.UnitZ + point, Color.Blue);
            points[5] = new VertexPositionColor(lenght * Vector3.UnitZ + point, Color.Blue);
        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private BasicEffect _basicEffect;

        private Matrix worldMatrix, viewMatrix, projectionMatrix;
        private Axes mainAxes;
        private float viewAngleX = 0f, viewAngleY = 0f, viewDistance = 5f;

        private Texture2D grassTexture, wallTexture, roofingTexture;

        VertexPositionTexture[] grass = new VertexPositionTexture[6], walls = new VertexPositionTexture[12];

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            mainAxes = new Axes(5, Vector3.Zero);
            grass[0] = new VertexPositionTexture(new Vector3(5,  0,  5), new Vector2(1, 0));
            grass[1] = new VertexPositionTexture(new Vector3(5,  0, -5), new Vector2(1, 1));
            grass[2] = new VertexPositionTexture(new Vector3(-5, 0,  5), new Vector2(0, 0));
            grass[3] = new VertexPositionTexture(new Vector3(-5, 0, -5), new Vector2(0, 1));
            grass[4] = new VertexPositionTexture(new Vector3(5,  0,  5), new Vector2(1, 0));
            grass[5] = new VertexPositionTexture(new Vector3(5,  0, -5), new Vector2(1, 1));

            walls[0] = new VertexPositionTexture(new Vector3(1, 0, 0.5f), new Vector2(2, 0));
            walls[1] = new VertexPositionTexture(new Vector3(-1, 1,  0.5f), new Vector2(0, 1));
            walls[2] = new VertexPositionTexture(new Vector3(-1, 0,  0.5f), new Vector2(0, 0));

            walls[3] = new VertexPositionTexture(new Vector3(-1,  1,  0.5f), new Vector2(0, 1));
            walls[4] = new VertexPositionTexture(new Vector3(1,  0, 0.5f), new Vector2(2, 0));
            walls[5] = new VertexPositionTexture(new Vector3(1,  1, 0.5f), new Vector2(2, 1));

            
            walls[6] = new VertexPositionTexture(new Vector3(1, 1, 0.5f), new Vector2(1, 1));
            walls[7] = new VertexPositionTexture(new Vector3(1, 0, 0.5f), new Vector2(1, 0));
            walls[8] = new VertexPositionTexture(new Vector3(1, 1, -0.5f), new Vector2(0, 1));

            walls[9] = new VertexPositionTexture(new Vector3(1, 0, 0.5f), new Vector2(0, 1));
            walls[10] = new VertexPositionTexture(new Vector3(1, 0, -0.5f), new Vector2(1, 1));
            walls[11] = new VertexPositionTexture(new Vector3(1, 1, -0.5f), new Vector2(1, 0));


            base.Initialize();
        }

        protected override void LoadContent()
        {
            grassTexture = Content.Load<Texture2D>(@"grass1");
            roofingTexture = Content.Load<Texture2D>(@"roofing1");
            wallTexture = Content.Load<Texture2D>(@"wall1");

            _basicEffect = new BasicEffect(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape)) { Exit(); }
            if (keyboard.IsKeyDown(Keys.Right)) { viewAngleY += 0.02f; }
            if (keyboard.IsKeyDown(Keys.Left)) { viewAngleY -= 0.02f; }
            if (keyboard.IsKeyDown(Keys.Up)) { viewAngleX += 0.02f; }
            if (keyboard.IsKeyDown(Keys.Down)) { viewAngleX -= 0.02f; }

            if (keyboard.IsKeyDown(Keys.S) && viewDistance <= 10) { viewDistance += 0.1f; }
            if (keyboard.IsKeyDown(Keys.W) && viewDistance >= 2) { viewDistance -= 0.1f; }

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.CreateLookAt(new Vector3(viewDistance, viewDistance, viewDistance), Vector3.Zero, Vector3.Up);
            viewMatrix = Matrix.CreateRotationX(viewAngleX) * Matrix.CreateRotationY(viewAngleY) * viewMatrix;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //display main axes lines
            _basicEffect.World = worldMatrix;
            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.TextureEnabled = false;
            foreach(EffectPass pass in _basicEffect.CurrentTechnique.Passes) 
            { 
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, mainAxes.points, 0, 3);
            }
            //display grass
            _basicEffect.World = worldMatrix;
            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;
            _basicEffect.VertexColorEnabled = false;
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = grassTexture;
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, grass, 0, grass.Length - 2);
            }
            //display walls
            _basicEffect.World = worldMatrix;
            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;
            _basicEffect.VertexColorEnabled = false;
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = wallTexture;
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, walls, 0, walls.Length / 3);
            }
            
            base.Draw(gameTime);
        }
    }
}