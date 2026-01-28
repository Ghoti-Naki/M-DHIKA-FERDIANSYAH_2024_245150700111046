using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public bool LevelFinished { get; private set; }
    private Dictionary<Vector2Int, GameObject> _gridObjects = new Dictionary<Vector2Int, GameObject>();
    private HashSet<Vector2Int> _goalPositions = new HashSet<Vector2Int>();

    [Header("UI Reference")]
    [SerializeField] private GameObject winTextPanel;
    [SerializeField] private GameObject gameplayUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void RegisterObject(GameObject obj, Vector2Int position)
    {
        if (_gridObjects.ContainsKey(position))
        {
            Debug.LogWarning($"Cell {position} is already occupied by {_gridObjects[position].name}");
            return;
        }
        _gridObjects[position] = obj;
    }
    public void RegisterGoal(Vector2Int position)
    {
        if (!_goalPositions.Contains(position))
        {
            _goalPositions.Add(position);
        }
    }

    public void MoveObjectPosition(Vector2Int oldPos, Vector2Int newPos)
    {
        if (_gridObjects.ContainsKey(oldPos))
        {
            GameObject obj = _gridObjects[oldPos];
            _gridObjects.Remove(oldPos);
            _gridObjects[newPos] = obj;

            CheckWinCondition();
        }
    }
    public bool IsCellOccupied(Vector2Int position)
    {
        return _gridObjects.ContainsKey(position);
    }

    public GameObject GetObjectAt(Vector2Int position)
    {
        if (_gridObjects.TryGetValue(position, out GameObject obj))
        {
            return obj;
        }
        return null;
    }
    public bool IsGoalAtPosition(Vector2Int pos)
    {
        return _goalPositions.Contains(pos);
    }

    public void CheckWinCondition()
    {
        if (LevelFinished) return;

        if (_goalPositions.Count == 0) return;

        foreach (Vector2Int goalPos in _goalPositions)
        {
            GameObject objAtGoal = GetObjectAt(goalPos);

            if (objAtGoal == null || objAtGoal.GetComponent<PushableBox>() == null)
            {
                return;
            }
        }

        LevelFinished = true;

        if (winTextPanel != null)
        {
            winTextPanel.SetActive(true);
        }

        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        ScoreManager.Instance.CompleteLevel();
        AudioManager.Instance.PlayWin();
    }
}