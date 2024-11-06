using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandQueue : MonoBehaviour
{
    private readonly Queue<IUICommand> queue = new();
    
    public bool TryEnqueueCommand(IUICommand Command)
    {
        queue.Enqueue(Command);
        return true;
    }
    public bool TryDequeueCommand(out IUICommand Command)
    {
        if (queue.Count > 0)
        {
            Command = queue.Dequeue();
            return true;
        }
        Command = default;
        return false;
    }
}
