namespace Nerbotix.Infrastructure.Algorithms.MathBased;

public class HungarianAlgorithm
{
    private readonly double[,] _costMatrix;
    private readonly int _size;
    private readonly int[] _labelsRow, _labelsCol, _matchRow, _matchCol;
    private readonly bool[] _visitedRow, _visitedCol;
    private readonly double[] _minSlack;
    private readonly int[] _slackRow;
    private readonly int[] _parentRow;

    public HungarianAlgorithm(double[,] costMatrix)
    {
        _costMatrix = costMatrix;
        _size = costMatrix.GetLength(0);
        _labelsRow = new int[_size];
        _labelsCol = new int[_size];
        _matchRow = new int[_size];
        _matchCol = new int[_size];
        _visitedRow = new bool[_size];
        _visitedCol = new bool[_size];
        _minSlack = new double[_size];
        _slackRow = new int[_size];
        _parentRow = new int[_size];

        for (int i = 0; i < _size; i++)
        {
            _matchRow[i] = -1;
            _matchCol[i] = -1;
        }
    }

    public int[] Run()
    {
        for (int row = 0; row < _size; row++)
        {
            _labelsRow[row] = (int)_costMatrix.Cast<double>().Skip(row * _size).Take(_size).Max();
        }

        for (int col = 0; col < _size; col++)
        {
            _labelsCol[col] = 0;
        }

        for (int row = 0; row < _size; row++)
        {
            bool found = false;
            while (!found)
            {
                Array.Fill(_visitedRow, false);
                Array.Fill(_visitedCol, false);
                Array.Fill(_parentRow, -1);

                if (DFS(row))
                {
                    found = true;
                }
                else
                {
                    UpdateLabels();
                }
            }
        }

        int[] result = new int[_size];
        for (int i = 0; i < _size; i++)
        {
            result[i] = _matchRow[i];
        }

        return result;
    }

    private bool DFS(int row)
    {
        _visitedRow[row] = true;

        for (int col = 0; col < _size; col++)
        {
            if (_visitedCol[col])
                continue;

            double slack = _labelsRow[row] + _labelsCol[col] - _costMatrix[row, col];

            if (slack == 0)
            {
                _visitedCol[col] = true;

                if (_matchCol[col] == -1 || DFS(_matchCol[col]))
                {
                    _matchRow[row] = col;
                    _matchCol[col] = row;
                    return true;
                }
            }
            else if (slack < _minSlack[col])
            {
                _minSlack[col] = slack;
                _slackRow[col] = row;
            }
        }

        return false;
    }

    private void UpdateLabels()
    {
        double delta = double.MaxValue;

        for (int col = 0; col < _size; col++)
        {
            if (!_visitedCol[col])
            {
                delta = Math.Min(delta, _minSlack[col]);
            }
        }

        for (int row = 0; row < _size; row++)
        {
            if (_visitedRow[row])
            {
                _labelsRow[row] -= (int)delta;
            }
        }

        for (int col = 0; col < _size; col++)
        {
            if (_visitedCol[col])
            {
                _labelsCol[col] += (int)delta;
            }
            else
            {
                _minSlack[col] -= delta;
            }
        }
    }
}