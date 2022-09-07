using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Logic
{
    public class FigureFabric
    {
        private List<bool[,]> _figureShapes;
        private List<Color> _colors;
        private Random _random;
        
        public FigureFabric(List<bool[,]> figureShapes, List<Color> colors)
        {
            _figureShapes = figureShapes;
            _colors = colors;
            _random = new Random();
        }

        public int GetMaxHeight()
        {
            int maxHeight = 0;
            foreach (var shape in _figureShapes)
                if (shape.GetLength(0) > maxHeight)
                    maxHeight = shape.GetLength(0);

            return maxHeight;
        }

        public Figure GetRandomFigure()
        {
            bool[,] shape = _figureShapes[_random.Next(0, _figureShapes.Count)];
            Color color = _colors[_random.Next(0, _colors.Count)];
            Figure figure = new Figure(shape, color);
            return figure;
        }
    }
}