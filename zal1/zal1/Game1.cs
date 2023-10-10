using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace zal1
{
    public class Tile 
    {
        public int number;
        public int rotation;
        public Rectangle rec;

        public Tile(Rectangle rec) 
        {
            this.rec = rec;
            this.number = 0;
            this.rotation = 0;
        }
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _background, _numbers, _questionmark;
        private int _squareSize = 50;
        private List<List<Tile>> tiles = new List<List<Tile>>();
        private int gridX = 200, gridY = 20;
        private int menuX = 0, menuY = 110;
        private int currentNumber = 0;
        private Rectangle currentNumberRec, menu1Rec, menu2Rec, menu3Rec, menu4Rec, menu5Rec, menu6Rec, menu7Rec, menu8Rec;
        private Rectangle sprite1, sprite2, sprite3, sprite4, sprite5, sprite6, sprite7, sprite8;
        private MouseState lastMouseState;
        private int hoveredMenuItem = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;

            _graphics.IsFullScreen = false;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            for (int i = 0; i < 10; i++) 
            { 
                List<Tile> listI = new List<Tile>();
                for(int j = 0; j < 10; j++)
                {
                    listI.Add(new Tile(new Rectangle(gridX + i * _squareSize, gridY + j * _squareSize, _squareSize, _squareSize)));
                }
                tiles.Add(listI);
            }

            currentNumberRec = new Rectangle(0, 0, 100, 100);
            menu1Rec = new Rectangle(menuX, menuY, _squareSize, _squareSize);
            menu2Rec = new Rectangle(menuX, menuY + _squareSize, _squareSize, _squareSize);
            menu3Rec = new Rectangle(menuX, menuY + 2 * _squareSize, _squareSize, _squareSize);
            menu4Rec = new Rectangle(menuX, menuY + 3 * _squareSize, _squareSize, _squareSize);
            menu5Rec = new Rectangle(menuX, menuY + 4 * _squareSize, _squareSize, _squareSize);
            menu6Rec = new Rectangle(menuX, menuY + 5 * _squareSize, _squareSize, _squareSize);
            menu7Rec = new Rectangle(menuX, menuY + 6 * _squareSize, _squareSize, _squareSize);
            menu8Rec = new Rectangle(menuX, menuY + 7 * _squareSize, _squareSize, _squareSize);

            sprite1 = new Rectangle(0, 0, 100, 100);
            sprite2 = new Rectangle(100, 0, 100, 100);
            sprite3 = new Rectangle(200, 0, 100, 100);
            sprite4 = new Rectangle(300, 0, 100, 100);
            sprite5 = new Rectangle(0, 100, 100, 100);
            sprite6 = new Rectangle(100, 100, 100, 100);
            sprite7 = new Rectangle(200, 100, 100, 100);
            sprite8 = new Rectangle(300, 100, 100, 100);

            lastMouseState = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _background = Content.Load<Texture2D>(@"background");
            _numbers = Content.Load<Texture2D>(@"numbers");
            _questionmark = Content.Load<Texture2D>(@"question");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState();

            if ((mouse.LeftButton == ButtonState.Pressed) && currentNumberRec.Contains(mouse.X, mouse.Y)) { currentNumber = 0; }

            hoveredMenuItem = 0;
            if (menu1Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 1;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 1;
            }
            if (menu2Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 2;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 2;
            }
            if (menu3Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 3;
            }
            if (menu4Rec.Contains(mouse.X, mouse.Y)) {
                hoveredMenuItem = 4;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 4;
            }
            if (menu5Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 5;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 5;
            }
            if (menu6Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 6;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 6;
            }
            if (menu7Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 7;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 7;
            }
            if (menu8Rec.Contains(mouse.X, mouse.Y)){
                hoveredMenuItem = 8;
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentNumber = 8;
            }


            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    if ((mouse.LeftButton == ButtonState.Pressed) && tiles[i][j].rec.Contains(mouse.X, mouse.Y) ){
                        if (tiles[i][j].number == currentNumber){
                            if (lastMouseState.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed){
                                tiles[i][j].rotation = (tiles[i][j].rotation + 1) % 4;
                            }
                        }
                        else{
                            tiles[i][j].number = currentNumber;
                        }
                    }
                    if (currentNumber == 0) { tiles[i][j].rotation = 0; }
                }
            }
            lastMouseState = mouse;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //display background
            _spriteBatch.Draw(_background,
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                Color.White);

            //display grid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (tiles[i][j].number)
                    {
                        default:
                            _spriteBatch.Draw(_questionmark, tiles[i][j].rec, Color.White);
                            break;
                        case 1:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize/ 2), sprite1, Color.White, MathHelper.ToRadians(tiles[i][j].rotation*90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f,0.5f), SpriteEffects.None, 1);
                            break;
                        case 2:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite2, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 3:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite3, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 4:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite4, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 5:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite5, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 6:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite6, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 7:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite7, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                        case 8:
                            _spriteBatch.Draw(_numbers, new Vector2(tiles[i][j].rec.X + _squareSize / 2, tiles[i][j].rec.Y + _squareSize / 2), sprite8, Color.White, MathHelper.ToRadians(tiles[i][j].rotation * 90), new Vector2(_squareSize, _squareSize), new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                            break;
                    }
                }
            }

            //display current tile
            switch (currentNumber)
            {
                default:
                    _spriteBatch.Draw(_questionmark, currentNumberRec, Color.White);
                    break;
                case 1:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite1, Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite2, Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite3, Color.White);
                    break;
                case 4:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite4, Color.White);
                    break;
                case 5:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite5, Color.White);
                    break;
                case 6:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite6, Color.White);
                    break;
                case 7:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite7, Color.White);
                    break;
                case 8:
                    _spriteBatch.Draw(_numbers, currentNumberRec, sprite8, Color.White);
                    break;
            }

            //display menu
            _spriteBatch.Draw(_numbers, menu1Rec, sprite1, Color.White);
            _spriteBatch.Draw(_numbers, menu2Rec, sprite2, Color.White);
            _spriteBatch.Draw(_numbers, menu3Rec, sprite3, Color.White);
            _spriteBatch.Draw(_numbers, menu4Rec, sprite4, Color.White);
            _spriteBatch.Draw(_numbers, menu5Rec, sprite5, Color.White);
            _spriteBatch.Draw(_numbers, menu6Rec, sprite6, Color.White);
            _spriteBatch.Draw(_numbers, menu7Rec, sprite7, Color.White);
            _spriteBatch.Draw(_numbers, menu8Rec, sprite8, Color.White);

            //color hovered menu item
            if (hoveredMenuItem == 1) { _spriteBatch.Draw(_numbers, menu1Rec, sprite1, Color.Black * 0.5f); }
            if (hoveredMenuItem == 2) { _spriteBatch.Draw(_numbers, menu2Rec, sprite2, Color.Black * 0.5f); } 
            if (hoveredMenuItem == 3) { _spriteBatch.Draw(_numbers, menu3Rec, sprite3, Color.Black * 0.5f); }
            if (hoveredMenuItem == 4) { _spriteBatch.Draw(_numbers, menu4Rec, sprite4, Color.Black * 0.5f); }
            if (hoveredMenuItem == 5) { _spriteBatch.Draw(_numbers, menu5Rec, sprite5, Color.Black * 0.5f); }
            if (hoveredMenuItem == 6) { _spriteBatch.Draw(_numbers, menu6Rec, sprite6, Color.Black * 0.5f); }
            if (hoveredMenuItem == 7) { _spriteBatch.Draw(_numbers, menu7Rec, sprite7, Color.Black * 0.5f); }
            if (hoveredMenuItem == 8) { _spriteBatch.Draw(_numbers, menu8Rec, sprite8, Color.Black * 0.5f); }

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}