using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GridComponent : DrawableGameComponent
    {
        private TetrominoController m_tetrominoController;
        private Grid m_grid;
        private TetrominoControllerFactory m_controllerFactory;
        private RenderTarget2D m_canvas;
        private SpriteBatch m_spriteBatch;
        private Texture2D m_bg;
        private SpriteFont m_spriteFont;

        public GridComponent(Game owner) : base(owner)
        {
            m_grid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
            m_controllerFactory = new TetrominoControllerFactory(ref m_grid);
        }

        protected override void LoadContent()
        {
        }

        public override void Initialize()
        {
            m_tetrominoController = m_controllerFactory.MakeController();

            Vector2 size;
            size.X = m_grid.Width * m_grid.CellSize;
            size.Y = m_grid.Height * m_grid.CellSize;

            m_canvas = new RenderTarget2D(GraphicsDevice, (int)size.X, (int)size.Y);
            m_canvas.Name = "GameGrid";

            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.SetRenderTargets(m_canvas);

            m_bg = Game.Content.Load<Texture2D>("background");
            m_spriteFont = Game.Content.Load<SpriteFont>("Text");
        }

        bool m_flagW = true;

        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyUp(Keys.W) && m_flagW)
            {
                m_flagW = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !m_flagW)
            {
                m_flagW = true;
                m_tetrominoController = m_controllerFactory.MakeController();
            }

            m_tetrominoController.Update(gameTime, m_grid);

            bool magicFlag = m_grid.DetectCollision(m_tetrominoController.Tetromino);
            if (m_tetrominoController.Tetromino.MaxYComponent() == m_grid.Height - 1 || magicFlag)
            {
                if (magicFlag)
                    m_tetrominoController.Tetromino.Positon.Y--;

                if (m_tetrominoController.Tetromino.Positon.Y <= m_grid.TetrominoSpawnHeight) //game over
                {
                    gameOver = true;
                    this.Enabled = false;
                    return;
                }
                m_grid.LandTetromino(m_tetrominoController.Tetromino);

                m_tetrominoController = m_controllerFactory.MakeController();
            }

            m_grid.TestFills();

        }
        bool gameOver = false;
        public override void Draw(GameTime _)
        {
            GraphicsDevice.SetRenderTarget(m_canvas);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_bg, new Rectangle(0, 0, m_canvas.Width, m_canvas.Height), Color.White);

            m_tetrominoController.Tetromino.Draw(m_spriteBatch);
            m_grid.DrawLines(m_spriteBatch); // grid lines drawn over tetrominos

            if (gameOver)
            {
                Vector2 scale = new Vector2(4f);
                m_spriteBatch.DrawString(m_spriteFont, "GAME OVER!", new Vector2(100, m_canvas.Height / 2 - 50), Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            m_spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void UnloadContent()
        {
        }

        public void Restart()
        {
            m_grid = new Grid(GameData.cellsX, GameData.cellsY, GameData.cellSize);
            m_tetrominoController = m_controllerFactory.MakeController();
            gameOver = false;
        }

    }
}