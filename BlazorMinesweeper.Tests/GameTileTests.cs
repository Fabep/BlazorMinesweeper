using BlazorMinesweeper.Components.GameLogic;

namespace RazorMinesweeper.Tests
{
    public class GameTileTests
    {
        [Fact]
        public void TileStateChangesToMineIfRevealedAndIsMine()
        {
            // Arrange
            GameTile tile = new()
            {
                Value = -1
            };
            TileState expected = TileState.mine;

            // Act
            tile.Revealed = true;

            // Assert
            Assert.IsType<GameTile>(tile);
            Assert.Equal(tile.Icon, expected);
        }
    }
}