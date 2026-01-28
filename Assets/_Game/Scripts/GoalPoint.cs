using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private void Start()
    {
        Vector2Int pos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x), 
            Mathf.RoundToInt(transform.position.y)
        );

        GridManager.Instance.RegisterGoal(pos);
    }
}