using UnityEngine;

namespace Logic
{
    public class Figure
    {
        private bool[,] _map;
        private Color _color;
        
        public bool[,] Map => _map;
        public Color Color => _color;
        
        public Figure(bool[,] map, Color color)
        {
            _map = map;
            _color = color;
        }
    }
}