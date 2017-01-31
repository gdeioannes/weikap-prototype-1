using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils {

    public static void DestroyChildren(this Transform parent)
    {
        while (parent.childCount > 0)
        {
            var child = parent.GetChild(0);
            child.gameObject.SetActive(false);
            child.SetParent(null); // unparent object
            Object.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// <para>Tries to get a value from a generic dictionary ...</para>
    /// <para>It returns a default value (optionally specified by yourself) if key wasn't found</para>
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    /// <typeparam name="U">Value type</typeparam>
    /// <param name = "dict">base dictionary</param>
    /// <param name = "key">Key to look up</param>
    /// <param name = "defaultValue"> default value to be returned when given key is not found in dictionary </param>
    /// <returns>Dictionary value, if exists. Returns default value otherwise</returns>
    public static T TryGetValue<U, T>(this Dictionary<U, T> dict, U key, T defaultValue = default(T))
    {
        if (key == null || string.IsNullOrEmpty(key.ToString()))
        {
            return defaultValue;
        }

        T toReturn;

        if (dict.TryGetValue(key, out toReturn))
        {
            return toReturn;
        }

        return defaultValue;
    }

    public static Vector2 WorldToCanvasPosition(Vector3 position, Camera camera, RectTransform canvas)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }		
}
