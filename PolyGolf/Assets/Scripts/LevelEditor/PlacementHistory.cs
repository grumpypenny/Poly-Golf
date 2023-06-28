using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHistory : MonoBehaviour
{

    private Stack<PlacementHistoryState> undoStack = new();
    private Stack<PlacementHistoryState> redoStack = new();


    public void UpdateHistory(PlacementHistoryState newState)
    {
        undoStack.Push(newState);

        // New state, so redo states must be cleared
        redoStack.Clear(); 
    }

    public PlacementHistoryState Undo()
    {
        if (undoStack.Count > 0)
        {
            PlacementHistoryState state = undoStack.Pop();
            redoStack.Push(state);
            return state;
        }
        return null;
    }
    
    public PlacementHistoryState Redo()
    {
        if (redoStack.Count > 0)
        {
            PlacementHistoryState state = redoStack.Pop();
            undoStack.Push(state);
            return state;
        }
        return null;
    }
}

public class PlacementHistoryState
{
    public PlacementType placementType { get; private set; }
    public AssetPlacementData placementData { get; private set; }

    public PlacementHistoryState(AssetPlacementData placementData, PlacementType placementType)
    {
        this.placementType = placementType;
        this.placementData = placementData;
    }
}

public enum PlacementType
{
    Placement = 1,
    Removal = 2
}
