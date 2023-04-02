using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGui;
using ImGuiNET;
using System;

namespace Tetris;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; //? unused
    private ImGuiRenderer m_GuiRenderer;
    private IntPtr m_canvasGpuId;
    Vector2 m_canvasSize;
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

        m_GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

        Utility.Init(GraphicsDevice);

        var layer = new GridComponent(this);
        Components.Add(layer);

        base.Initialize();

        Texture2D can = Array.Find(GraphicsDevice.GetRenderTargets(), t => t.RenderTarget.Name == "GameGrid").RenderTarget as Texture2D;

        m_canvasSize = new(can.Width, can.Height);

        m_canvasGpuId = m_GuiRenderer.BindTexture(can);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        base.Update(gameTime);
    }

    private void ImGuiDraw()
    {
        bool isOpen = true;
        ImGui.Begin("canvas", ref isOpen, ImGuiWindowFlags.NoTitleBar);

        Vector2 area = ImGui.GetContentRegionAvail();
        float scaleY = area.Y / m_canvasSize.Y;
        float scaleX = area.X / m_canvasSize.X;
        float scale = Math.Min(scaleX, scaleY);
        Vector2 imageSize = m_canvasSize * scale;

        ImGui.SetCursorPos((ImGui.GetWindowSize() - imageSize).ToNumerics() * 0.5f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, 0f);
        ImGui.Image(m_canvasGpuId, imageSize.ToNumerics());
        ImGui.PopStyleVar();

        ImGui.End();

    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.DarkRed);

        base.Draw(gameTime);

        m_GuiRenderer.BeginLayout(gameTime);
        ImGuiDraw();
        m_GuiRenderer.EndLayout();
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        Utility.Shutdown();
        base.OnExiting(sender, args);
    }
}
