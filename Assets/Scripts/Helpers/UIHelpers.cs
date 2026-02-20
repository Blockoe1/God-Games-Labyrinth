/*****************************************************************************
// File Name : UIHelpers.cs
// Author : Brandon Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions that deal with the UI systems
// that have common utility across multiple projects.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIHelpers
{
    /// <summary>
    /// Checks if a specific position is over a UI element.
    /// </summary>
    /// <param name="pos">The position in screen space to check.</param>
    /// <returns>Whether the position is over a UI object.</returns>
    public static bool IsPositionOverUI(Vector2 pos)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.CompareTag("IgnorePointer"))
            {
                results.RemoveAt(i);
                i--;
            }
        }
        return results.Count > 0;
    }

    /// <summary>
    /// Formats a given number to display a number of 0s before and after the digits to give it an arcade feel.
    /// </summary>
    /// <param name="num">The number to be displayed.</param>
    /// <param name="digits">The number of digits that should be displayed.</param>
    /// <param name="postfixDigits">The number of additional 0s to add to the end of the digit.</param>
    /// <returns>The formatted digit as a string.</returns>
    public static string ArcadeFormat(int num, int digits, int postfixDigits = 0)
    {
        string scoreString = num.ToString();
        //Debug.Log(scoreString);
        for (int i = scoreString.Length; i < digits; i++)
        {
            scoreString = "0" + scoreString;
        }
        // Add Postfix digits.
        for (int i = 0; i < postfixDigits; i++)
        {
            scoreString = scoreString + "0";
        }
        //Debug.Log(scoreString);
        return scoreString;
    }
}
