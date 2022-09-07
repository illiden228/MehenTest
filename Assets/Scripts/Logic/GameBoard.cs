using System;
using System.Threading;
using UnityEngine;

namespace Logic
{
    public class GameBoard
    {
        private Cell[,] _map;
        private Figure _currentFigure;
        private FigureFabric _figureFabric;
        private int _figureRowIndex;
        private int _bufferSize;
        private int _downFigureIndex = Int32.MinValue;
        private int _leftFigureIndex = Int32.MaxValue;
        private int _rightFigureIndex = Int32.MinValue;

        public event Action GameOver;

        public GameBoard(int height, int width, FigureFabric figureFabric)
        {
            _figureFabric = figureFabric;
            _bufferSize = _figureFabric.GetMaxHeight();
            _map = new Cell[height + _bufferSize, width];
            FillMap();
            CreateFigure(_figureFabric.GetRandomFigure());
        }

        public Cell this[int indexRow, int indexColumn] => _map[indexRow + _bufferSize, indexColumn];

        public void Update()
        {
            if (TryMoveDownFigure() == false)
            {
                FixateAll();
                CheckAndClearFillRows();
                if (CheckEnd())
                    GameOver?.Invoke();
                else
                    CreateFigure(_figureFabric.GetRandomFigure());
            }
        }

        public void CreateFigure(Figure figure)
        {
            _currentFigure = figure;

            _downFigureIndex = Int32.MinValue;
            _leftFigureIndex = Int32.MaxValue;
            _rightFigureIndex = Int32.MinValue;

            int gridCenter = _map.GetLength(1) / 2;
            int figureCenter = _currentFigure.Map.GetLength(1) / 2;
            int startIndexFromCenter = gridCenter - figureCenter;
            int addingIndex = _currentFigure.Map.GetLength(1) % 2 == 0 ? 0 : 1;

            for (int i = 0; i < _bufferSize; i++)
            {
                if (i >= _currentFigure.Map.GetLength(0))
                    break;

                for (int j = startIndexFromCenter, k = 0; j < gridCenter + figureCenter + addingIndex; j++, k++)
                {
                    if (_currentFigure.Map[i, k])
                    {
                        _map[i, j].FillFigure(figure.Color);
                    }
                }
            }

            for (int i = _bufferSize; i >= 0; i--)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (_map[i, j].IsFigure)
                    {
                        if (i > _downFigureIndex)
                            _downFigureIndex = i;

                        if (j < _leftFigureIndex)
                            _leftFigureIndex = j;

                        if (j > _rightFigureIndex)
                            _rightFigureIndex = j;
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _map[i, j].Clear();
                }
            }
        }

        public bool TryMoveDownFigure()
        {
            bool canMove = CheckCanDownMove();
            if (canMove)
            {
                ShiftDownToRow(_downFigureIndex, cell => cell.IsFigure);

                _downFigureIndex++;
            }

            return canMove;
        }

        public bool TryMoveHorizontalFigure(int direction)
        {
            bool canMove = CheckCanHorizontalMove(direction);
            int startColumn = direction < 0 ? _leftFigureIndex : _rightFigureIndex;
            int endColumn = direction < 0 ? _rightFigureIndex : _leftFigureIndex;

            if (!canMove)
                return canMove;

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = startColumn; j != endColumn - direction; j -= direction)
                {
                    if (_map[i, j].IsFigure)
                    {
                        _map[i, j].Clear();
                        _map[i, j + direction].FillFigure(_currentFigure.Color);
                    }
                }
            }

            _leftFigureIndex += direction;
            _rightFigureIndex += direction;

            return canMove;
        }

        private void FillMap()
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _map[i, j] = new Cell();
                }
            }
        }

        private bool CheckEnd()
        {
            bool result = false;
            for (int i = 0; i < _map.GetLength(1); i++)
            {
                if (_map[_bufferSize, i].IsEmpty == false)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool CheckCanDownMove()
        {
            bool canMove = _currentFigure != null &&
                           _downFigureIndex < _map.GetLength(0) - 1;
            if (canMove)
            {
                for (int i = 0; i < _map.GetLength(1); i++)
                {
                    if (_map[_downFigureIndex, i].IsFigure)
                    {
                        if (_map[_downFigureIndex + 1, i].IsFigure == false &&
                            _map[_downFigureIndex + 1, i].IsEmpty == false)
                        {
                            canMove = false;
                        }
                    }
                }
            }

            return canMove;
        }

        private bool CheckCanHorizontalMove(int direction)
        {
            bool canMove = _currentFigure != null &&
                           (direction < 0 ? _leftFigureIndex > 0 : _rightFigureIndex < _map.GetLength(1) - 1);
            int checkColumn = direction < 0 ? _leftFigureIndex : _rightFigureIndex;
            if (canMove)
            {
                for (int i = 0; i < _map.GetLength(0); i++)
                {
                    if (_map[i, checkColumn].IsFigure)
                    {
                        if (_map[i, checkColumn + direction].IsFigure == false &&
                            _map[i, checkColumn + direction].IsEmpty == false)
                        {
                            canMove = false;
                        }
                    }
                }
            }

            return canMove;
        }
        
        private void ShiftDownToRow(int rowIndex, Func<Cell, bool> shiftCondition)
        {
            for (int i = rowIndex; i >= 0; i--)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (shiftCondition(_map[i, j]))
                    {
                        Color color = _map[i, j].Color;
                        bool isFigure = _map[i, j].IsFigure;
                        _map[i, j].Clear();
                        _map[i + 1, j].FillFigure(color);
                        if(!isFigure)
                            _map[i + 1, j].FixateFigure();
                    }
                }
            }
        }

        private void FixateAll()
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _map[i, j].FixateFigure();
                }
            }
        }

        private void CheckAndClearFillRows()
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                bool isFillRow = true;
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (_map[i, j].IsEmpty)
                    {
                        isFillRow = false;
                        break;
                    }
                }

                if (isFillRow)
                    ClearRow(i);
            }
        }

        private void ClearRow(int rowIndex)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                _map[rowIndex, j].Clear();
            }

            ShiftDownToRow(rowIndex - 1, cell => !cell.IsEmpty && !cell.IsFigure);
        }
    }
}