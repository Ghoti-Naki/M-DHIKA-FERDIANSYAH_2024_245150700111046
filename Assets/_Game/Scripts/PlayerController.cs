using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    public Vector2Int GridPosition;
    private bool _isMoving;
    private Stack<ICommand> _commandHistory = new Stack<ICommand>();
    private void Start()
    {
        GridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
        transform.position = new Vector3(GridPosition.x, GridPosition.y, 0);

        GridManager.Instance.RegisterObject(this.gameObject, GridPosition);
    }

    private void Update()
    {
        if (_isMoving || GridManager.Instance.LevelFinished) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();
                ScoreManager.Instance.UndoMove();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        Vector2Int moveDir = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = Vector2Int.right;

        if (moveDir != Vector2Int.zero)
        {
            if (_animator != null)
            {
                _animator.SetFloat("InputX", moveDir.x);
                _animator.SetFloat("InputY", moveDir.y);
            }

            AttemptMove(moveDir);
        }
    }
    private void AttemptMove(Vector2Int direction)
    {
        Vector2Int targetPos = GridPosition + direction;
        GameObject objectAtTarget = GridManager.Instance.GetObjectAt(targetPos);
        PushableBox boxToPush = null;

        if (objectAtTarget != null)
        {
            PushableBox box = objectAtTarget.GetComponent<PushableBox>();
            if (box == null) return;
            if (!box.CanMoveTo(direction)) return;

            boxToPush = box;
        }

        ScoreManager.Instance.AddMove();

        ICommand moveCommand = new MoveCommand(this, direction, boxToPush);
        moveCommand.Execute();
        _commandHistory.Push(moveCommand);
    }
    public void MoveToPosition(Vector2Int targetPos)
    {
        StartCoroutine(MovePlayerVisual(targetPos));
    }

    private System.Collections.IEnumerator MovePlayerVisual(Vector2Int targetPos)
    {
        _isMoving = true;

        AudioManager.Instance.PlayMove();

        GridManager.Instance.MoveObjectPosition(GridPosition, targetPos);
        GridPosition = targetPos;

        Vector3 targetWorldPos = new Vector3(targetPos.x, targetPos.y, 0);

        while ((targetWorldPos - transform.position).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetWorldPos;

        _isMoving = false;
    }
}