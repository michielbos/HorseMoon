using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsefulShit
{
    public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
    {
        foreach (T item in list)
            action(item);
    }

    public static int Forward(this Transform t)
    {
        return t.eulerAngles.y == 180 ? -1 : 1;
    }

    public static void SetForward(this Transform t, float f)
    {
        if (f < 0)
            t.eulerAngles = new Vector3(0f, 180f, 0f);
        else if (f > 0)
            t.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    static Camera main;
    public static Camera Camera =>  main ? main : (main = Camera.main);

    public static bool OnCamera(this Transform transform, float padding)
    {
        return OnCamera(transform.position, padding);
    }

    public static bool OnCamera(Vector2 position, float padding)
    {
        var dy = position.y - Camera.transform.position.y;
        if(Mathf.Abs(dy) > Camera.orthographicSize + padding)
            return false;
        var dx = position.x - Camera.transform.position.x;
        if (Mathf.Abs(dx) > Camera.orthographicSize * Camera.aspect + padding)
            return false;
        return true;
    }
    
    public static Vector3Int ToVector3Int(this Vector2Int vector) => new Vector3Int(vector.x, vector.y, 0);
    
    public static Vector2Int ToVector2Int(this Vector3Int vector) => new Vector2Int(vector.x, vector.y);

    public static Vector2Int WorldToTile(this Vector3 worldPos) => ((Vector2) worldPos).WorldToTile();
    
    public static Vector2Int WorldToTile(this Vector2 worldPos) =>
        new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    
    public static Vector2 TileToWorld(this Vector2Int tilePos) => new Vector2(tilePos.x, tilePos.y);
}
