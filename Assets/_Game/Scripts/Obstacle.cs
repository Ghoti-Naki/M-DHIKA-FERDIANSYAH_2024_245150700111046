using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        Vector2Int pos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x), 
            Mathf.RoundToInt(transform.position.y)
        );

        GridManager.Instance.RegisterObject(this.gameObject, pos);
    }
}