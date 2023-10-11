using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace zal1
{
    public class Tile 
    {
        public int number;
        public int rotation;
        public Rectangle rec;

        public Tile(Rectangle r){
            rec = r;
            number = 10;
            rotation = 0;
        }
    }
    public class MenuItem
    {
        public Rectangle menuRec;
        public Rectangle menuSprite;
        public MenuItem(Rectangle r, Rectangle s){ 
            menuRec = r;
            menuSprite = s;
        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _background, _numbers, _questionmark;
        private int _squareSize = 50;
        private int gridX = 200, gridY = 20;
        private int menuX = 0, menuY = 110;
        private Tile[,] tiles = new Tile[10, 10];
        private MenuItem[] menuItems = new MenuItem[8];
        private Rectangle currentNumberRec, menuRec, gridRec;
        private ButtonState lastMouseStateLeftClickPressed = ButtonState.Released;
        private int hoveredMenuItem = 10;
        private int currentNumber = 10;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;

            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            //fill grid with Tile items
            for (int i = 0; i < 10; i++){ 
                for(int j = 0; j < 10; j++){
                    tiles[i,j] = new Tile(new Rectangle(gridX + i * _squareSize, gridY + j * _squareSize, _squareSize, _squareSize));
                }
            }
            //fill menu items 0 - 3
            for (int i = 0;i < 4;i++){
                menuItems[i] = new MenuItem(
                        new Rectangle(menuX, menuY + i * _squareSize, _squareSize, _squareSize),
                        new Rectangle(i * 100 % 400, 0, 100, 100)
                    );
            }
            //fill menu items 4 - 7
            for (int i = 4; i < 8; i++)
            {
                menuItems[i] = new MenuItem(
                        new Rectangle(menuX, menuY + i * _squareSize, _squareSize, _squareSize),
                        new Rectangle(i * 100 % 400, 100, 100, 100)
                    );
            }
            //prepare interface areas
            currentNumberRec = new Rectangle(0, 0, 100, 100);
            menuRec = new Rectangle(menuX, menuY, _squareSize, _squareSize * 8);
            gridRec = new Rectangle(gridX, gridY, _squareSize * 10, _squareSize * 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _background = Content.Load<Texture2D>(@"background");
            _numbers = Content.Load<Texture2D>(@"numbers");
            _questionmark = Content.Load<Texture2D>(@"question");
        }

        protected override void Update(GameTime gameTime)
        {
            //press escape to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
                Exit();
            }
                
            MouseState mouse = Mouse.GetState();
            hoveredMenuItem = 10;

            //check if currently selected square was pressed, to reset selected menu item
            if ((mouse.LeftButton == ButtonState.Pressed) && currentNumberRec.Contains(mouse.X, mouse.Y)){
                currentNumber = 10;
            }
            //check if mouse id over menu
            else if (menuRec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = (mouse.Y - menuY) / _squareSize;
                this.Window.Title = hoveredMenuItem.ToString();
                if (mouse.LeftButton == ButtonState.Pressed){
                    currentNumber = (mouse.Y - menuY) / _squareSize;
                }
            }
            //check if grid was pressed
            else if (gridRec.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed){
                int x = (mouse.X - gridX) / _squareSize;
                int y = (mouse.Y - gridY) / _squareSize;
                if (tiles[x, y].number == currentNumber && lastMouseStateLeftClickPressed == ButtonState.Released){
                    tiles[x, y].rotation = (tiles[x, y].rotation + 1) % 4;
                }
                else{
                    tiles[x, y].number = currentNumber;
                }
                if (currentNumber == 10){
                    tiles[x,y].rotation = 0; 
                }
            }
            //save last LMB state
            lastMouseStateLeftClickPressed = mouse.LeftButton;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //display background
            _spriteBatch.Draw(
                _background,
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                Color.White
            );

            //display grid
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    if (tiles[i, j].number == 10){ 
                        _spriteBatch.Draw(_questionmark, tiles[i, j].rec, Color.White);
                    }
                    else{
                        _spriteBatch.Draw(
                            _numbers,
                            new Vector2(tiles[i,j].rec.X + _squareSize / 2, tiles[i,j].rec.Y + _squareSize / 2),
                            menuItems[tiles[i, j].number].menuSprite,
                            Color.White,
                            MathHelper.ToRadians(tiles[i,j].rotation * 90),
                            new Vector2(_squareSize, _squareSize),
                            new Vector2(0.5f, 0.5f),
                            SpriteEffects.None,
                            1
                        );
                    }
                }
            }
            //display current tile
            if (currentNumber == 10){
                _spriteBatch.Draw(_questionmark, currentNumberRec, Color.White);
            }
            else{
                _spriteBatch.Draw(_numbers, currentNumberRec, menuItems[currentNumber].menuSprite, Color.White);
            }
            //display menu
            for(int i = 0; i < 8; i++){ 
                _spriteBatch.Draw(_numbers, menuItems[i].menuRec, menuItems[i].menuSprite, Color.White);
            }
            //color hovered menu item
            for (int i = 0; i < 8; i++){ 
                if (hoveredMenuItem != 10){ 
                    _spriteBatch.Draw(_numbers, menuItems[hoveredMenuItem].menuRec, menuItems[hoveredMenuItem].menuSprite, Color.Black * 0.1f); 
                } 
            }

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
