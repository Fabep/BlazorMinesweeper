using BlazorMinesweeper.Components.GameLogic;
using Microsoft.AspNetCore.Components;

namespace BlazorMinesweeper.Components.Components;

public partial class Tile : ComponentBase
{
    [Parameter]
    public EventCallback<int> OnTileLeftClick { get; set; }
    [Parameter]
    public EventCallback<TileState> OnTileRightClick { get; set; }

    [Parameter]
    public GameTile GameTile { get; set; } = null!;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string TileSize { get; set; } = null!;

    string PlaySquareCss = "tile-hidden";

    string Icon = "";

    protected override void OnParametersSet()
    {
        Icon = GameTile.Icon switch
        {
            TileState.flag => "icon-flag",
            TileState.question => "icon-question",
            TileState.mine => "icon-bomb",
            _ => ""
        };
        PlaySquareCss = GameTile.Revealed ? "tile-revealed" : "tile-hidden";

        base.OnParametersSet();
    }

    private async Task HandleLeftClick()
    {
        if (GameTile.Revealed || GameTile.Icon == TileState.flag || Disabled) return;
        PlaySquareCss = "revealed";
        await OnTileLeftClick.InvokeAsync(GameTile.Id).ConfigureAwait(true);
    }

    private async Task HandleRightClick()
    {
        if (GameTile.Revealed || Disabled || GameTile.Icon == TileState.mine) return;

        switch (GameTile.Icon)
        {
            case TileState.empty:
                GameTile.Icon = TileState.flag;
                Icon = "icon-flag";
                break;
            case TileState.flag:
                GameTile.Icon = TileState.question;
                Icon = "icon-question";
                break;
            default:
                GameTile.Icon = TileState.empty;
                Icon = "";
                break;
        }
        await OnTileRightClick.InvokeAsync(GameTile.Icon).ConfigureAwait(true);
    }
}
