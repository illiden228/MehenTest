using UnityEngine.Serialization;

[System.Serializable]
public class BoolArray
{
    [System.Serializable]
    public struct RowData
    {
        public bool[] Row;
    }

    public int Size = 4;
    public RowData[] Rows;

    public bool[,] Get()
    {
        bool[,] array = new bool[Size, Size];
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = Rows[i].Row[j];
            }
        }

        return array;
    }
}