using UnityEngine;

public interface IPlacementState
{
    void EndState();
    void OnAction(Vector3Int gridPosition, int rotation, int height);
    void OnRemove(Vector3Int gridPosition, int height);
    void Undo();
    void Redo();

    void UpdateState(Vector3Int gridPosition, int rotation, int height);
}