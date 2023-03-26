using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tetris;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D gameCanvas;

    private Grid m_grid;

    private Tetromino m_testTetromino;

    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        float factor = 1.8f;

        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = (int)(GameData.bufferW / factor);
        _graphics.PreferredBackBufferHeight = (int)(GameData.bufferH / factor);
        _graphics.ApplyChanges();


        Utility.Init(GraphicsDevice);

        gameCanvas = new RenderTarget2D(GraphicsDevice, GameData.bufferW, GameData.bufferH);

        m_grid = new Grid(12, 25, 50);

        m_testTetromino = new Tetromino(50);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    bool flag = true;

    protected override void Update(GameTime gameTime)
    {

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyUp(Keys.W) && flag)
            flag = false;

        if (Keyboard.GetState().IsKeyDown(Keys.W) && !flag)
        {
            flag = true;
            m_testTetromino = m_grid.SpawnTetromino();
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float scale = (float)GraphicsDevice.Viewport.Height / (float)gameCanvas.Height;

        GraphicsDevice.SetRenderTarget(gameCanvas);
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _spriteBatch.Begin();


        m_testTetromino.Draw(_spriteBatch, 7, 5);

        m_grid.Draw(_spriteBatch); //? grid lines

        _spriteBatch.End();


        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue); //? 

        _spriteBatch.Begin();
        _spriteBatch.Draw(gameCanvas, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f); // output from game layer
        _spriteBatch.End();


        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        Utility.Shutdown();

        base.OnExiting(sender, args);
    }
}
