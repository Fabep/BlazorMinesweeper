using System.Security.Cryptography;

namespace BlazorMinesweeper.Components.GameLogic;

public class Game
{
    private int _height;

    private int _width;

    private int _mineCount;

    private int _revealedTiles;
    public int RevealedTiles => _revealedTiles;
    public int Height => _height;
    public int Width => _width;

    public Dictionary<int, GameTile> GameTiles { get; private set; }

    private HashSet<int> mines;

    private bool _firstClick;

    public Game(int height = 16, int width = 30, int mineCount = 99)
    {
        _height = height;
        _width = width;
        _mineCount = mineCount;
        GameTiles = new(1);
        mines = new(1);
    }

    public void GenerateGame(int height = 16, int width = 30, int mineCount = 99)
    {
        _height = height;
        _width = width;
        _mineCount = mineCount;
        _firstClick = true;
        GameTiles = ConstructDictionary();
        mines = new HashSet<int>(mineCount);
        _revealedTiles = 0;
        SetupBoard();
        CalculateTiles();
    }

    public void RevealTile(int tileId)
    {
        if (_firstClick && GameTiles[tileId].Value == -1)
        {
            int swapId = tileId;
            while (GameTiles[swapId].Value == -1)
            {
                swapId = RandomNumberGenerator.GetInt32(0, _width * _height);
            }
            (GameTiles[swapId], GameTiles[tileId]) = (GameTiles[tileId], GameTiles[swapId]);
            GameTiles[tileId].Value = 0;
            CalculateTile(tileId);
            CalculateAroundTile(swapId);
            _firstClick = false;
        }
        else if (_firstClick)
            _firstClick = false;

        GameTiles[tileId].Revealed = true;
        _revealedTiles++;


        if (GameTiles[tileId].Value == 0)
            RevealAdjacentTiles(tileId);
    }

    private void RevealAdjacentTiles(int tileId)
    {
        int[] adjacentTiles =
        [
            tileId - _width - 1, tileId - _width, tileId - _width + 1,
            tileId - 1, tileId + 1,
            tileId + _width - 1, tileId + _width, tileId + _width + 1
        ];

        foreach (int tile in adjacentTiles)
        {
            if (CheckIfValidTile(tile, tileId) &&
                !GameTiles[tile].Revealed &&
                GameTiles[tile].Value != -1)
            {
                GameTiles[tile].Revealed = true;
                _revealedTiles++;
                if (GameTiles[tile].Value == 0)
                    RevealAdjacentTiles(tile);
            }
        }
    }

    public void RevealMines()
    {
        foreach (int i in mines)
            GameTiles[i].Revealed = true;
    }
    private void CalculateTiles()
    {
        for (int tileId = 0; tileId < GameTiles.Count; tileId++)
        {
            if (GameTiles[tileId].Value == -1) continue;

            CalculateTile(tileId);
        }
    }

    private void CalculateTile(int tileId)
    {
        int[] adjacentTiles =
        [
            tileId - _width - 1, tileId - _width, tileId - _width + 1,
            tileId - 1, tileId + 1,
            tileId + _width - 1, tileId + _width, tileId + _width + 1
        ];

        GameTiles[tileId].Value += adjacentTiles
            .Count(adjacentTile => CheckIfValidTile(adjacentTile, tileId, TileIsMine));
    }

    private void CalculateAroundTile(int tileId)
    {
        int[] adjacentTiles =
        [
            tileId - _width - 1, tileId - _width, tileId - _width + 1,
            tileId - 1, tileId + 1,
            tileId + _width - 1, tileId + _width, tileId + _width + 1
        ];

        foreach (int tile in adjacentTiles)
        {
            if (GameTiles[tile].Value == -1) continue;
            GameTiles[tile].Value = 0;

            CalculateTile(tile);
        }
    }

    private bool TileIsMine(int tileToCheck)
    {
        if (GameTiles[tileToCheck].Value != -1)
            return false;

        return true;
    }

    private bool CheckIfValidTile(int tileToCheck, int original, Predicate<int>? predicate = null)
    {
        var mod = tileToCheck % _width;
        var modOriginal = original % _width;
        if (
            tileToCheck < 0 ||
            tileToCheck >= _width * _height ||
            mod > modOriginal + 1 ||
            mod < modOriginal - 1)
            return false;

        if (predicate != null)
            return predicate(tileToCheck);

        return true;
    }

    private void SetupBoard()
    {
        int i = 0;
        while (i < _mineCount)
        {
            bool hasPut = false;
            while (!hasPut)
            {

                var minePlacement = RandomNumberGenerator.GetInt32(0, _width * _height);
                if (GameTiles[minePlacement].Value == -1) continue;
                mines.Add(minePlacement);
                GameTiles[minePlacement].Value = -1;
                hasPut = true;
            }
            i++;
        }
    }

    private Dictionary<int, GameTile> ConstructDictionary()
    {
        Dictionary<int, GameTile> tiles = new(_height * _width);
        for (int i = 0; i < _height * _width; i++)
        {
            GameTile gt = new()
            {
                Id = i
            };
            tiles.Add(i, gt);
        }
        return tiles;
    }

    public bool CheckIfWon()
    {
        return _width * _height - _mineCount <= RevealedTiles;
    }
}
