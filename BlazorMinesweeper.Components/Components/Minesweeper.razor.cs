using BlazorMinesweeper.Components.GameLogic;
using Microsoft.AspNetCore.Components;

namespace BlazorMinesweeper.Components.Components;

public partial class Minesweeper : ComponentBase
{
    /// <summary>
    /// Event to trigger when the game starts.
    /// </summary>
    [Parameter]
    public EventCallback OnStart { get; set; }

    /// <summary>
    /// Event that triggers after a game has ended.
    /// Sends back a bool, true if the game was won and false if lost.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnGameEnd { get; set; }

    /// <summary>
    /// Event that triggers after the user left clicks on a tile.
    /// </summary>
    [Parameter]
    public EventCallback OnTileLeftClick { get; set; }

    /// <summary>
    /// Event that triggers after the user right clicks on a tile.
    /// Sends back the current state of the Tile.
    /// </summary>
    [Parameter]
    public EventCallback<TileState> OnTileRightClick { get; set; }

    /// <summary>
    /// Width of the grid.
    /// The component will throw an exception if the width * height is less than 50.
    /// The component will throw an exception if the width * height / 2 is less than the specified amount of mines.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 30;

    /// <summary>
    /// Height of the grid.
    /// The component will throw an exception if the width * height is less than 50.
    /// The component will throw an exception if the width * height / 2 is less than the specified amount of mines.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 16;

    /// <summary>
    /// Amount of mines on the grid.
    /// The component will throw an exception if the amount if mines are less than or equal to 0.
    /// The component will throw an exception if the width * height / 2 is less than the specified amount of mines.
    /// </summary>
    [Parameter]
    public int MineCount { get; set; } = 99;

    [Parameter]
    public bool IgnoreDefaultGameOver { get; set; }

    [Parameter]
    public int TileSize { get; set; } = 20;

    private string TileSizeString => $"{TileSize}px";


    private bool DisplayGame;

    private bool GameOver;

    private string GameOverMessage = "";

    private readonly Game Game = new();

    protected override void OnParametersSet()
    {
        if (Width * Height / 2 <= MineCount)
            throw new ArgumentException("Mine count can't exceed half of the amount of tiles!");
        if (Width * Height < 50)
            throw new ArgumentException("The amount of tiles you have specified is to small. Consider increasing the width and/or height of the component.");
        if (MineCount <= 0)
            throw new ArgumentException("The amount of mines must be larger than 0!");
        base.OnParametersSet();
    }

    /// <summary>
    /// Method for starting / generating a new game.
    /// </summary>
    /// <returns></returns>
    public async Task StartGame()
    {
        await OnStart.InvokeAsync().ConfigureAwait(true);
        Game.GenerateGame(Height, Width, MineCount);
        DisplayGame = true;
        GameOver = false;
    }

    private async Task SquareLeftClicked(int tileId)
    {
        await OnTileLeftClick.InvokeAsync().ConfigureAwait(true);
        Game.RevealTile(tileId);

        int tileVal = Game.GameTiles[tileId].Value;
        if (tileVal == -1)
            await HandleGameOver().ConfigureAwait(true);
        else if (Game.CheckIfWon())
            await HandleWin().ConfigureAwait(true);
    }

    private async Task SquareRightClicked(TileState tileIcon)
    {
        await OnTileRightClick.InvokeAsync(tileIcon).ConfigureAwait(true);
    }

    private async Task HandleWin()
    {
        GameOverMessage = "You win!";
        GameOver = true;
        await OnGameEnd.InvokeAsync(true).ConfigureAwait(true);
    }

    private async Task HandleGameOver()
    {
        GameOverMessage = "You lose!";
        GameOver = true;
        Game.RevealMines();
        await OnGameEnd.InvokeAsync(false).ConfigureAwait(true);
    }
}
