using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// source of code https://www.reddit.com/r/Unity3D/comments/dfnfzv/how_to_create_a_custom_input_validator_for_text/
/// by u/Kirbyderby
/// https://www.reddit.com/user/Kirbyderby/
/// <summary>
/// Create your validator class and inherit TMPro.TMP_InputValidator 
/// Note that this is a ScriptableObject, so you'll have to create an instance of it via the Assets -> Create -> Input Field Validator 
/// </summary>
[CreateAssetMenu(fileName = "Input Field Validator", menuName = "Input Field Validator")]
public class CustomValidator : TMPro.TMP_InputValidator
{
    public static char ch1 = '.';
    /// <summary>
    /// Override Validate method to implement your own validation
    /// </summary>
    /// <param name="text">This is a reference pointer to the actual text in the input field; changes made to this text argument will also result in changes made to text shown in the input field</param>
    /// <param name="pos">This is a reference pointer to the input field's text insertion index position (your blinking caret cursor); changing this value will also change the index of the input field's insertion position</param>
    /// <param name="ch">This is the character being typed into the input field</param>
    /// <returns>Return the character you'd allow into </returns>
    public override char Validate(ref string text, ref int pos, char ch)
    {
        //Debug.Log($"Text = {text}; pos = {pos}; chr = {ch}");
        // If the typed character is a number, insert it into the text argument at the text insertion position (pos argument)
        if (char.IsNumber(ch))
        {
            // Insert the character at the given position if we're working in the Unity Editor
#if UNITY_EDITOR
            text = text.Insert(pos, ch.ToString());
#endif
            // Increment the insertion point by 1
            pos++;
            return ch;
            
        }
        else if(text.EndsWith(ch1))
        {
            return '\0';
        }
        else
        if(ch.CompareTo(ch1) == 0){
        // Insert the character at the given position if we're working in the Unity Editor
#if UNITY_EDITOR
            text = text.Insert(pos, ch.ToString());
#endif
            // Increment the insertion point by 1
            pos++;
            return ch;
        }
        // If the character is not a number, return null
        else
        {
            return '\0';
        }
    }
}
