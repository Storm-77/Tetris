﻿using Microsoft.Xna.Framework;
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

    Texture2D m_keyTex;
    IntPtr m_keyTexGpuId;

    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;
    }

    void OnResize(object sender, EventArgs args)
    {
        ImGui.GetIO().DisplaySize = new System.Numerics.Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = GameData.windowW;
        _graphics.PreferredBackBufferHeight = GameData.windowH;

        _graphics.ApplyChanges();

        m_GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

        Utility.Init(GraphicsDevice);
        UI.EmbraceTheDarkness();

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard; // Enable Keyboard Controls
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;     // Enable Docking

        var grid = new GridComponent(this);
        Components.Add(grid);

        base.Initialize();

        Texture2D can = Array.Find(GraphicsDevice.GetRenderTargets(), t => t.RenderTarget.Name == "GameGrid").RenderTarget as Texture2D;

        m_canvasSize = new(can.Width, can.Height);

        m_canvasGpuId = m_GuiRenderer.BindTexture(can);

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        m_keyTex = Content.Load<Texture2D>("Keys");
        m_keyTexGpuId = m_GuiRenderer.BindTexture(m_keyTex);

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
        UI.DrawDockSpace(this);
        bool isOpen = true;


        ImGui.Begin("canvas", ref isOpen, ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoMove);

        foreach (var item in Components)
        {
            if (item.GetType() == typeof(GridComponent))
            {
                var c = item as GridComponent;
                c.Enabled = ImGui.IsWindowFocused();
            }
        }

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

        OptionsWindow();

        // ImGui.ShowStyleEditor();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkRed);

        _spriteBatch.Begin();

        _spriteBatch.End();

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

    public void Restart()
    {
        foreach (var item in Components)
            if (item.GetType() == typeof(GridComponent))
                (item as GridComponent).Restart();
    }
    public void OptionsWindow()
    {
        bool open = true;
        ImGui.Begin("Options", ref open);

        if (UI.ButtonCenteredOnLine("Restart game"))
        {
            this.Restart();
        }

        Vector2 texSize = new(m_keyTex.Width, m_keyTex.Height);
        Vector2 area = ImGui.GetContentRegionAvail();
        float scaleY = area.Y / texSize.Y;
        float scaleX = area.X / texSize.X;
        float scale = Math.Min(scaleX, scaleY);
        Vector2 imageSize = texSize * scale;
        ImGui.SetCursorPos((ImGui.GetWindowSize() - imageSize).ToNumerics() * 0.5f);

        ImGui.Image(m_keyTexGpuId, imageSize.ToNumerics());


        ImGui.End();
    }

}
