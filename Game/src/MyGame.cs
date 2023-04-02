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
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;
    }

    void OnResize(object sender, EventArgs args)
    {
        ImGui.GetIO().DisplaySize = new System.Numerics.Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = GameData.windowW;
        _graphics.PreferredBackBufferHeight = GameData.windowH;

        _graphics.ApplyChanges();

        m_GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

        Utility.Init(GraphicsDevice);
        UiTheme.EmbraceTheDarkness();

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
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    private void DockSpace()
    {
        var io = ImGui.GetIO();
        bool p_open = true;
        ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None;

        // We are using the ImGuiWindowFlags.NoDocking flag to make the parent window not dockable into,
        // because it would be confusing to have two docking targets within each others.
        ImGuiWindowFlags window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

        ImGuiViewportPtr viewport = ImGui.GetMainViewport();

        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(viewport.WorkSize);
        ImGui.SetNextWindowViewport(viewport.ID);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);


        if ((int)(dockspace_flags & ImGuiDockNodeFlags.PassthruCentralNode) > 0)
            window_flags |= ImGuiWindowFlags.NoBackground;

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0.0f, 0.0f));
        ImGui.Begin("DockSpace", ref p_open, window_flags);
        ImGui.PopStyleVar();
        ImGui.PopStyleVar(2);


        if ((int)(io.ConfigFlags & ImGuiConfigFlags.DockingEnable) == 0)
        {
            int a = 0;// error docing disabled
        }

        uint dockspace_id = ImGui.GetID("MyDockSpace");

        ImGui.DockSpace(dockspace_id, new System.Numerics.Vector2(0.0f, 0.0f), dockspace_flags);

        if (ImGui.BeginMenuBar())
        {

            if (ImGui.BeginMenu("Options"))
            {
                ImGui.Separator();

                if (ImGui.MenuItem("ImGui: PassthruCentralNode", "", (dockspace_flags & ImGuiDockNodeFlags.PassthruCentralNode) != 0, true))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.PassthruCentralNode;
                }

                ImGui.Separator();

                if (ImGui.MenuItem("Close", null, false, true))
                {
                    this.Exit();
                }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }

        ImGui.End();
    }

    private void ImGuiDraw()
    {
        DockSpace();
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

        ImGui.Begin("Options");

        if (ImGui.Button("Restart game"))
        {
            //todo restart tetris
        }

        //todo add buttons for sound control
        //todo add keyboard keybind explanation
        //todo add bgcolor control

        ImGui.End();

        ImGui.ShowDemoWindow();
        ImGui.ShowFontSelector("mylabel");

        ImGui.ShowStyleSelector("selector laber");

        ImGui.ShowStyleEditor();


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
}
