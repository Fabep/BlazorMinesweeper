namespace BlazorMinesweeper.Components.GameLogic;

public class GameTile
{
    public int Id { get; set; }
    public int Value { get; set; }
    public TileState Icon { get; set; } = TileState.empty;
    public bool Revealed
    {
        get => _revealed;
        set
        {
            _revealed = value;
            if (_revealed && Value == -1)
                Icon = TileState.mine;
            else if (_revealed)
                Icon = TileState.empty;
        }
    }
    private bool _revealed;
}
