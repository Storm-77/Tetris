using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGui;
using ImGuiNET;
using System;
using System.Collections.Generic;

namespace Tetris;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D gameCanvas;

    private LayerStack m_layerStack;

    public ImGuiRenderer GuiRenderer; //This is the ImGuiRenderer
    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = GameData.windowW;
        _graphics.PreferredBackBufferHeight = GameData.windowH;

        _graphics.ApplyChanges();

        GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();


        Utility.Init(GraphicsDevice);

        var gameLayer = new GridLayer();

        Vector2 gridSize = gameLayer.GetGridSizePx();

        gameCanvas = new RenderTarget2D(GraphicsDevice, (int)gridSize.X, (int)gridSize.Y);

        m_layerStack = new LayerStack();
        m_layerStack.PushLayer(gameLayer);

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


        GuiRenderer.BeginLayout(gameTime);

        //Insert Your ImGui code

        ImGui.Begin("mywindow");

        ImGui.Text("Hello tetris");

        ImGui.End();

        GuiRenderer.EndLayout();
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        Utility.Shutdown();

        base.OnExiting(sender, args);
    }
}
