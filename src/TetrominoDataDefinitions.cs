using System;
using System.Collections.Generic;

namespace Tetris
{
    public enum TetrominoShape
    {
        None = 0,
        I, O, N, Z, J, L, T
    }

    public partial class Tetromino
    {
        static private int[,,] s_figureData =
        {
               {{-1,  0}, {-2,  0}, { 0, 0}, { 1,  0} }, // I
               {{ 0, -1}, {-1, -1}, {-1, 0}, { 0,  0} }, // O
               {{-1,  0}, {-1,  1}, { 0, 0}, { 0, -1} }, // N
               {{ 0,  0}, {-1,  0}, { 0, 1}, {-1, -1} }, // Z
               {{ 0,  0}, { 0, -1}, { 0, 1}, {-1, -1} }, // J
               {{ 0,  0}, { 0, -1}, { 0, 1}, { 1, -1} }, // L (upside down)
               {{ 0,  0}, { 0, -1}, { 0, 1}, {-1,  0} }  // T
        };

        static private Dictionary<TetrominoShape, int> s_figureDataIndexMap = new()
        {
            { TetrominoShape.I, 0 },
            { TetrominoShape.O, 1 },
            { TetrominoShape.N, 2 },
            { TetrominoShape.Z, 3 },
            { TetrominoShape.J, 4 },
            { TetrominoShape.L, 5 },
            { TetrominoShape.T, 6 },
        };

        static private TetrominoShape PickRandomShape()
        {
            TetrominoShape[] values = (TetrominoShape[])Enum.GetValues(typeof(TetrominoShape));

            Random random = new Random();

            TetrominoShape shape = TetrominoShape.None;
            while (shape == TetrominoShape.None) // None value must not be picked
            {
                shape = values[random.Next(values.Length)];
            }

            return shape;
        }

    }
}