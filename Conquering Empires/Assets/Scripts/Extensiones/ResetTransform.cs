using UnityEngine;

public static class ResetTransform
{
    public static void Reset(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }
}