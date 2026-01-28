using UnityEngine;
public interface ICommand
{
    void Execute();
    void Undo();
}

public class MoveCommand : ICommand
{
    private PlayerController _player;
    private Vector2Int _direction;
    private PushableBox _pushedBox;

    public MoveCommand(PlayerController player, Vector2Int direction, PushableBox pushedBox = null)
    {
        _player = player;
        _direction = direction;
        _pushedBox = pushedBox;
    }
    public void Execute()
    {
        if (_pushedBox != null)
        {
            _pushedBox.ForceMove(_direction);
        }

        _player.MoveToPosition(_player.GridPosition + _direction);
    }

    public void Undo()
    {
        _player.MoveToPosition(_player.GridPosition - _direction);

        if (_pushedBox != null)
        {
            _pushedBox.ForceMove(-_direction);
        }
    }
}