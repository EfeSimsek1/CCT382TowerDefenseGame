using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void OnMouseHoverObj() { }
    public virtual void OnMouseDownObj() { }
    public virtual void OnMouseUpObj() { }
    public virtual void OnMouseEnterObj() { }
    public virtual void OnMouseExitObj() { }
}
