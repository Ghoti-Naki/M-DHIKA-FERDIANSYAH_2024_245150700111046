using UnityEngine;
using System.Collections;

public class PushableBox : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Visual Feedback")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color onGoalColor = new Color(0.8f, 0.8f, 0.8f);

    private Vector2Int _gridPosition;
    private bool _isMoving;
    private SpriteRenderer _spriteRenderer;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
        transform.position = new Vector3(_gridPosition.x, _gridPosition.y, 0);

        GridManager.Instance.RegisterObject(this.gameObject, _gridPosition);

        Invoke(nameof(UpdateGoalVisuals), 0.1f);
    }
    private void UpdateGoalVisuals()
    {
        if (GridManager.Instance == null || _spriteRenderer == null) return;

        bool isOnGoal = GridManager.Instance.IsGoalAtPosition(_gridPosition);

        _spriteRenderer.color = isOnGoal ? onGoalColor : defaultColor;
    }

    public bool TryPush(Vector2Int direction)
    {
        if (_isMoving) return false;

        Vector2Int targetPos = _gridPosition + direction;

        if (GridManager.Instance.IsCellOccupied(targetPos))
        {
            return false;
        }

        StartCoroutine(MoveBox(targetPos));
        return true;
    }

    public void ForceMove(Vector2Int direction)
    {
        Vector2Int targetPos = _gridPosition + direction;
        StartCoroutine(MoveBox(targetPos));
    }

    public bool CanMoveTo(Vector2Int direction)
    {
        Vector2Int targetPos = _gridPosition + direction;
        return !GridManager.Instance.IsCellOccupied(targetPos);
    }
    private IEnumerator MoveBox(Vector2Int targetPos)
    {
        _isMoving = true;

        AudioManager.Instance.PlayPush();

        GridManager.Instance.MoveObjectPosition(_gridPosition, targetPos);
        _gridPosition = targetPos;

        Vector3 endPos = new Vector3(targetPos.x, targetPos.y, 0);

        while ((transform.position - endPos).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = endPos;

        UpdateGoalVisuals();
        GridManager.Instance.CheckWinCondition();

        _isMoving = false;
    }
}