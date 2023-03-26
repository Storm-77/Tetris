using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Grid
    {
        public uint CellSize { get; private set; }
        private bool[,] m_gridData; // holds indecies in m_floor list

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


            m_gridData = new bool[GameData.cellsX, GameData.cellsY];

            for (int i = 0; i < GameData.cellsX; i++)
            {
                for (int j = 0; j < GameData.cellsY; j++)
                {
                    m_gridData[i, j] = false;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, (int)CellSize, (int)CellSize);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    rect.X = (int)(i * CellSize);
                    rect.Y = (int)(j * CellSize);

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