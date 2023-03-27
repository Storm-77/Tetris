using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Tetris
{

    public partial class Tetromino
    {
        private int m_shapeDataIndex;
        private Color m_color;
        public Vector2 Positon;
        private Grid m_myGrid;

        bool m_rotated = false;

        public Tetromino(Grid belongsTo, TetrominoShape shape = TetrominoShape.None)
        {
            m_myGrid = belongsTo;

            if (shape == TetrominoShape.None)
                shape = PickRandomShape();

            s_figureDataIndexMap.TryGetValue(shape, out m_shapeDataIndex);

            // pick random color from pool
            Color[] colors = { Color.DeepPink, Color.Orange, Color.PaleGreen, Color.MediumBlue, Color.MediumVioletRed };

            System.Random random = new System.Random();

            m_color = colors[random.Next(colors.Length)];

            //? maybe add outline color variable

            Positon = new(5f, 2f);
        }

        public void Rotate()
        {
            m_rotated = !m_rotated;
        }

        private (int x, int y) GetPieceXY(int i)
        {
            int X = s_figureData[m_shapeDataIndex, i, 0];
            int Y = s_figureData[m_shapeDataIndex, i, 1];

            if (m_rotated)
            {
                int temp = X;
                X = Y;
                Y = -temp;
            }

            return (X, Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cell = new Rectangle(0, 0, (int)m_myGrid.CellSize, (int)m_myGrid.CellSize);

            for (int i = 0; i < 4; i++)
            {
                (int X, int Y) = GetPieceXY(i);

                cell.X = (int)(Positon.X * m_myGrid.CellSize + m_myGrid.CellSize * X);
                cell.Y = (int)(Positon.Y * m_myGrid.CellSize + m_myGrid.CellSize * Y);

                spriteBatch.DrawRectangleColor(cell, m_color);

            }
        }

        public bool Collides(ref bool[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                (int A, int B) = GetPieceXY(i);

                int X = (int)(Positon.X + A);
                int Y = (int)(Positon.Y + B);

                if (X < 0 || Y < 0 || grid[X, Y])
                {
                    Positon.Y--;
                    return true;
                }
            }

            return false;
        }

        public void Land(ref bool[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                (int A, int B) = GetPieceXY(i);
                int X = (int)(Positon.X + A);
                int Y = (int)(Positon.Y + B);

                grid[X, Y] = true;

            }
        }

        public void CheckOuterBorders()
        {
            int max = int.MinValue;
            int min = int.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                (int A, int B) = GetPieceXY(i);
                max = (int)Math.Max(A + Positon.X, max);
                min = (int)Math.Min(A + Positon.X, min);
            }

            if (max > m_myGrid.Width - 1)
                Positon.X--;

            if (min < 0)
                Positon.X++;
        }

        public int MaxYComponent() // lowest piece of the tetromino
        {
            int max = int.MinValue;

            for (int i = 0; i < 4; i++)
            {
                (int A, int B) = GetPieceXY(i);
                max = (int)Math.Max(B + Positon.Y, max);
            }

            return max;
        }

    }
}