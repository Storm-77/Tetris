using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tetris;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D frame;
    private Dictionary<string, Texture2D> tileCombinations;
    private RenderTarget2D gameCanvas;

    Vector2 position;

    private const float tileSize = (float)768 / (float)12;

    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;


    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        float factor = 1.8f;

        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = (int)(768f / factor);
        _graphics.PreferredBackBufferHeight = (int)(1408f / factor);
        _graphics.ApplyChanges();

        tileCombinations = new Dictionary<string, Texture2D>();

        position = Vector2.Zero;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        frame = Content.Load<Texture2D>("Board-Clear");

        // TODO: use this.Content to load your game content here

        tileCombinations["Z"] = Content.Load<Texture2D>("TILE_Z");
        tileCombinations["T"] = Content.Load<Texture2D>("TILE_T");
        tileCombinations["S"] = Content.Load<Texture2D>("TILE_S");
        tileCombinations["O"] = Content.Load<Texture2D>("TILE_O");
        tileCombinations["L"] = Content.Load<Texture2D>("TILE_L");
        tileCombinations["J"] = Content.Load<Texture2D>("TILE_J");
        tileCombinations["I"] = Content.Load<Texture2D>("TILE_I");

        gameCanvas = new RenderTarget2D(GraphicsDevice, frame.Width, frame.Height);
    }

    System.TimeSpan prev;
    int counter = 1;

    protected override void Update(GameTime gameTime)
    {

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        if ((gameTime.TotalGameTime - prev).Seconds >= 1)
        {

            counter++;
            position.Y += tileSize;

            prev = gameTime.TotalGameTime;
        }

        base.Update(gameTime);
    }
    //gamestate 12x20 2d array of chars representing the board
    protected override void Draw(GameTime gameTime)
    {

        // TODO: Add your drawing code here
        float scale = (float)GraphicsDevice.Viewport.Height / (float)gameCanvas.Height;

        GraphicsDevice.SetRenderTarget(gameCanvas);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(frame, new Vector2(0, 0), Color.White);

        _spriteBatch.Draw(tileCombinations["J"], position, Color.Red);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue); //? 

        _spriteBatch.Begin();

        _spriteBatch.Draw(gameCanvas, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
