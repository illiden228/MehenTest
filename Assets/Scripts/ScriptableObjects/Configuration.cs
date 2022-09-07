using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Configuration")]
public class Configuration : ScriptableObject
{
    [SerializeField] private int _width;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Color _backGroundColor;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private List<BoolArray> _shapes;

    public int Width => _width;

    public float Speed => _speed;

    public GameObject CellPrefab => _cellPrefab;

    public Color BackGroundColor => _backGroundColor;

    public List<Color> Colors => _colors;

    public List<BoolArray> Shapes => _shapes;
}