/**
 * @author:  Müjdat KIRIKKAYA
 * @date: April 2024 
 */

using UnityEngine;
using UnityEngine.UIElements;

public static class UIExtensions
{
    private static  Vector2 _vector2Zero = new (0f, 0f);

     /// <summary>
     /// This method adjust the letter size according to the text area.
     /// </summary>
     /// <param name="isAuto">If using manual control this must set "false".</param>
     /// <param name="isTextLengthIncrease">Need to this value for auto control. We must to know letter is added or deleted?</param>
    public static void FixLabelTextSize(this Label label, bool isAuto, bool isTextLengthIncrease = true)
    {
        if(label.contentRect.size == _vector2Zero)
        {
            Debug.Log($"FixLabelTextSize-ContentRect size is zero({label.contentRect.size})!!!");
            return;    
        }

        if(label.resolvedStyle.fontSize <= 1)
        {
            Debug.Log($"FixLabelTextSize-Font size({label.resolvedStyle.fontSize}) is less than one!!!");
            return;    
        }

        string afterTrim = label.text.Trim();
        if(string.IsNullOrEmpty(afterTrim)) 
        {
            Debug.Log("FixLabelTextSize-Font is empty!");
            return;    
        }

        float labelContentSizeX = label.contentRect.size.x;
        float labelContentSizeY = label.contentRect.size.y;

        Vector2 measureTextSize = CalculateMeasureTextSize(label, labelContentSizeX, labelContentSizeY);
        
        //Decrease the font size
        while((!isAuto || isTextLengthIncrease) && measureTextSize.x > labelContentSizeX)
        {
            label.style.fontSize = label.resolvedStyle.fontSize - 1;
            measureTextSize = CalculateMeasureTextSize(label, labelContentSizeX, labelContentSizeY);
        }

        //Increase the font size
        while((!isAuto || !isTextLengthIncrease) && measureTextSize.x < labelContentSizeX && measureTextSize.y <labelContentSizeY)
        {
            label.style.fontSize = label.resolvedStyle.fontSize + 1;
            measureTextSize = CalculateMeasureTextSize(label, labelContentSizeX, labelContentSizeY);

            // We need this control because last visual of text can be oversize of the text area.
            if(measureTextSize.x > labelContentSizeX)
            {
                label.style.fontSize = label.resolvedStyle.fontSize - 1;
                Debug.Log("FixLabelTextSize-measureTextSize-increase-last check!");
            }
        }
    }

    private static Vector2 CalculateMeasureTextSize(Label label, float labelContentSizeX, float labelContentSizeY)
    {
        return label.MeasureTextSize(label.text, labelContentSizeX, VisualElement.MeasureMode.Undefined, labelContentSizeY, VisualElement.MeasureMode.Undefined);
    }
}
