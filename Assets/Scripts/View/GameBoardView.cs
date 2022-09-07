using Logic;
using UnityEngine;

public class GameBoardView
{
    private Transform _background;
    private Configuration _configuration;
    private GameBoard _gameBoard;
    private SpriteRenderer[,] _sprites;
    private float _cellSize;
    private int _gridHeight;

    public GameBoardView(Transform background, Configuration configuration, GameBoard gameBoard)
    {
        _background = background;
        _configuration = configuration;
        _gameBoard = gameBoard;
        _cellSize = _background.localScale.x / _configuration.Width;
        _gridHeight = (int)(_background.localScale.y / _cellSize);
        
        _sprites = new SpriteRenderer[_gridHeight, _configuration.Width];
        
        _background.gameObject.SetActive(true);
        FillGrid();
    }

    private void FillGrid()
    {
        float leftMinCornerX = _background.position.x -_background.localScale.x / 2;
        float leftMinCornerY = _background.position.x -_background.localScale.y / 2;

        float startPointX = leftMinCornerX + _cellSize / 2;
        float startPointY = leftMinCornerY + _cellSize / 2;
        Vector3 position = new Vector3(startPointX, startPointY);

        for (int i = 0; i < _sprites.GetLength(0); i++)
        {
            for (int j = 0; j < _sprites.GetLength(1); j++)
            {
                GameObject newCell = GameObject.Instantiate(_configuration.CellPrefab, position, Quaternion.identity);
                newCell.transform.localScale = new Vector3(_cellSize, _cellSize);
                newCell.transform.SetParent(_background.transform, true);
                _sprites[i, j] = newCell.GetComponent<SpriteRenderer>();
                SetColorToSprite(i, j);
                position.x += _cellSize;
            }

            position.x = startPointX;
            position.y += _cellSize;
        }
    }

    public void UpdateCellsView()
    {
        for (int i = 0; i < _sprites.GetLength(0); i++)
        {
            for (int j = 0; j < _sprites.GetLength(1); j++)
            {
                SetColorToSprite(i, j);
            }
        }
    }
    
    private void SetColorToSprite(int rowIndex, int columnIndex)
    {
        Color cellColor = _gameBoard[_sprites.GetLength(0) - 1 - rowIndex, columnIndex].Color;
        
        if (cellColor == Color.black)
            _sprites[rowIndex, columnIndex].color = _configuration.BackGroundColor;
        else
            _sprites[rowIndex, columnIndex].color = cellColor;
    }
}