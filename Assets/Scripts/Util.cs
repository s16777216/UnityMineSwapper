using UnityEngine;

public class Util
{
    public static void TryGetChildComponentByName<T>(MonoBehaviour target, string name, out T component)
    {
        var childTransform = target.transform.Find(name);
        if (!childTransform) component = default;
        else
        {
            childTransform.TryGetComponent<T>(out component);
        }
    }

    public static T TryGetComponentOnPointer<T>()
    {
        T component = default;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider == null) return component;
        hit.collider.TryGetComponent<T>(out component);
        return component;
    }

    public static bool TryGetComponentOnPointer<T>(out T component)
    {
        component = TryGetComponentOnPointer<T>();
        return component != null;
    }

    public static bool IsValid(object obj)
    {
        if (obj == null) return false;
        if (obj is MonoBehaviour mono && (mono == null || mono.gameObject == null)) 
            return false;
        return true;
    }
}