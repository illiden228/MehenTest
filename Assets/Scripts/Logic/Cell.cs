using UnityEngine;

namespace Logic
{
    public class Cell
    {
        private bool _isEmpty = true;
        private bool _isFigure = false;
        private Color _color = Color.black;

        public bool IsEmpty => _isEmpty;
        public bool IsFigure => _isFigure;
        public Color Color => _color;

        public void FillFigure(Color color)
        {
            _color = color;
            _isEmpty = false;
            _isFigure = true;
        }

        public void FixateFigure()
        {
            _isFigure = false;
        }

        public void Clear()
        {
            _color = Color.black;
            _isEmpty = true;
            _isFigure = false;
        }
    }
}