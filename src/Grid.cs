using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Grid
    {
        public uint CellSize { get; private set; }
        private Color[,] m_gridData; // holds indecies in m_floor list

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
            CellSize = cellSize;

            LineColor = Color.Black;
            LineThickness = 2;

            TetrominoSpawnHeight = 2;
            TetrominoSpawnMargin = 2;


            m_gridData = new Color[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    m_gridData[i, j] = Color.Black;
                }
            }

        }

        public bool DetectCollision(Tetromino tetromino)
        {
            return tetromino.Collides(ref m_gridData);
        }

        public void LandTetromino(Tetromino tetromino)
        {
            tetromino.Land(ref m_gridData);
        }

        public void DrawLines(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, (int)CellSize, (int)CellSize);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    rect.X = (int)(i * CellSize);
                    rect.Y = (int)(j * CellSize);

                    if (m_gridData[i, j] != Color.Black)
                        spriteBatch.DrawRectangleFull(rect, m_gridData[i, j], LineColor, LineThickness);
                    else
                        spriteBatch.DrawRectangleOutline(rect, LineColor, LineThickness);
                }
            }

        }

        public Tetromino SpawnTetromino(TetrominoShape shape = TetrominoShape.None)
        {
            Tetromino newPiece = new Tetromino(this, shape);

            System.Random random = new System.Random();

            newPiece.Positon.X = random.Next((int)(Width - 2 * TetrominoSpawnMargin)) + TetrominoSpawnMargin;
            newPiece.Positon.Y = TetrominoSpawnHeight;

            return newPiece;
        }

    }
}