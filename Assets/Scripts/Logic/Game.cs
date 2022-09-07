using System.Collections.Generic;
using Logic;
using UnityEngine;

public class Game
{
    private Configuration _configuration;
    private StartMenu _startMenu;
    private GameOverMenu _gameOverMenu;
    private Background _boardBackground;
    private FigureFabric _figureFabric;
    private GameBoard _gameBoard;
    private GameBoardView _gameBoardView;
    private float _timer = 0;
    private float _tickTime = 1f;
    private bool _isGameOver = true;

    public Game()
    {
        _configuration = Resources.Load<Configuration>(Constants.ConfigurationPath);
        _startMenu = GameObject.FindObjectOfType<StartMenu>();
        _gameOverMenu = GameObject.FindObjectOfType<GameOverMenu>();
        _boardBackground = GameObject.FindObjectOfType<Background>();

        _startMenu?.Init(Start);
        _gameOverMenu?.Init(Restart);
        
        _gameOverMenu?.gameObject.SetActive(false);
        _boardBackground.gameObject.SetActive(false);
    }

    private void Start()
    {
        List<bool[,]> shapes = new List<bool[,]>();
        foreach (var shape in _configuration.Shapes)
        {
            shapes.Add(shape.Get());
        }

        int height = (int)(_boardBackground.transform.localScale.y / (_boardBackground.transform.localScale.x /
                           _configuration.Width));
        _figureFabric = new FigureFabric(shapes, _configuration.Colors);
        _gameBoard = new GameBoard(height, _configuration.Width, _figureFabric);
        _gameBoardView = new GameBoardView(_boardBackground.transform, _configuration, _gameBoard);

        _gameBoard.GameOver += OnGameOver;
        
        _startMenu?.gameObject.SetActive(false);

        _isGameOver = false;
    }

    public void Update()
    {
        if (_isGameOver)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _tickTime / _configuration.Speed)
        {
            _gameBoard.Update();
            _gameBoardView.UpdateCellsView();
            _timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _gameBoard.TryMoveHorizontalFigure(-1);
            _gameBoardView.UpdateCellsView();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameBoard.TryMoveHorizontalFigure(1);
            _gameBoardView.UpdateCellsView();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _gameBoard.TryMoveDownFigure();
            _gameBoardView.UpdateCellsView();
        }
    }

    private void OnGameOver()
    {
        _isGameOver = true;
        _gameOverMenu?.gameObject.SetActive(true);
    }

    private void Restart()
    {
        _gameBoard.Clear();
        _gameBoard.CreateFigure(_figureFabric.GetRandomFigure());
        _gameBoardView.UpdateCellsView();
        _isGameOver = false;
        _gameOverMenu?.gameObject.SetActive(false);
    }
}