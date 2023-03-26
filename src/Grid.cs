using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Grid
    {
        private uint m_cellSize;

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public Color LineColor { get; set; }
        public int LineThickness { get; set; }

        public uint TetrominoSpawnMargin { get; set; }
        public uint TetrominoSpawnHeight { get; set; }
        
        public Grid(uint X, uint Y, uint cellSize)
        {
            Width = X;
            Height = Y;
            m_cellSize = cellSize;

            LineColor = Color.Black;
            LineThickness = 2;

            TetrominoSpawnHeight = 2;
            TetrominoSpawnMargin = 2;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, (int)m_cellSize, (int)m_cellSize);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    rect.X = (int)(i * m_cellSize);
                    rect.Y = (int)(j * m_cellSize);

                    spriteBatch.DrawRectangleOutline(rect, LineColor, LineThickness);
                }
            }

        }

        public Tetromino SpawnTetromino(TetrominoShape shape = TetrominoShape.None)
        {

            Tetromino newPiece = new Tetromino(m_cellSize, shape);

            System.Random random = new System.Random();

            newPiece.Positon.X = random.Next((int)(Width - 2 * TetrominoSpawnMargin)) + TetrominoSpawnMargin;
            newPiece.Positon.Y = TetrominoSpawnHeight;

            return newPiece;
        }

    }
}