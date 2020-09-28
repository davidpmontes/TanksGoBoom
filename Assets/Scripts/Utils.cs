using UnityEngine;

public static class Utils
{
    public static GameObject FindChildByNameRecursively(Transform parentTransform, string name)
    {
        Transform[] children = parentTransform.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }
}
