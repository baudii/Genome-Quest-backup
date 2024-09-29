using System;
using UnityEngine.Events;

[Serializable]
public class ContextAction
{
    public string[] contextKeywords;
    public UnityEvent action;
}
