using BlazorMinesweeper.Components.GameLogic;
using System.Security.Cryptography;


namespace RazorMinesweeper.Tests;


public class MinesweeperGameFixture
{
    public MinesweeperGameFixture()
    {
        Game = new();
        Game.GenerateGame();
    }
    public Game Game { get; private set; }
}

public class MinesweeperGameTests(MinesweeperGameFixture gameFixture) : IClassFixture<MinesweeperGameFixture>
{
    [Fact]
    public void PropertiesSetProperlyIfGameGeneratedWithNoParameters()
    {
        Assert.True(gameFixture.Game.Height == 16);
        Assert.True(gameFixture.Game.Width == 30);
        Assert.True(gameFixture.Game.GameTiles.Count == 16 * 30);
    }

    [Fact]
    public void RevealTileSetsTileToTrue()
    {
        var tileToReveal = RandomNumberGenerator.GetInt32(gameFixture.Game.Width * gameFixture.Game.Height);
        var revealedTiles = gameFixture.Game.RevealedTiles;
        gameFixture.Game.RevealTile(tileToReveal);
        Assert.True(gameFixture.Game.GameTiles[tileToReveal].Revealed);
        Assert.True(gameFixture.Game.RevealedTiles > revealedTiles);
    }

    [Fact]
    public void RevealingTileWithoutAdjacentMinesWillRevealAllOtherAdjacentTiles()
    {
        var tile = gameFixture.Game.GameTiles.First(x => x.Value.Value == 0);
        int tileId = tile.Key, width = gameFixture.Game.Width, height = gameFixture.Game.Height;

        int[] tilesToVerify =
            [
                tileId - width - 1, tileId - width, tileId - width + 1, 
                tileId - 1, tileId + 1,
                tileId + width - 1, tileId + width, tileId + width + 1,
            ];

        gameFixture.Game.RevealTile(tileId);

        Assert.True(gameFixture.Game.GameTiles[tileId].Revealed);

        foreach(int t in tilesToVerify)
        {
            var mod = t % width;
            var modOriginal = tileId % width;
            if (!
                (t < 0 ||
                t >= width * height ||
                mod > modOriginal + 1 ||
                mod < modOriginal - 1))
                Assert.True(gameFixture.Game.GameTiles[t].Revealed);
        }
    }
    [Fact]
    public void GameWonIfRevealedAllTilesThatAreNotMines()
    {
        foreach(var i in gameFixture.Game.GameTiles)
        {
            if (i.Value.Revealed || i.Value.Value == -1) continue;

            gameFixture.Game.RevealTile(i.Key);
        }

        Assert.True(gameFixture.Game.CheckIfWon());
    }

}
