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

    private LayerStack m_layerStack;
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

        m_layerStack = new LayerStack();

        m_layerStack.PushLayer(new GridLayer());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }


    protected override void Update(GameTime gameTime)
    {

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        m_layerStack.Update(gameTime);



        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float scale = (float)GraphicsDevice.Viewport.Height / (float)gameCanvas.Height;

        GraphicsDevice.SetRenderTarget(gameCanvas);
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _spriteBatch.Begin();

        m_layerStack.Draw(_spriteBatch); //?

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
