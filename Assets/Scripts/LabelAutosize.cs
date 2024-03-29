using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class LabelAutosize : MonoBehaviour
{
    private static  Vector2 _vector2Zero = new (0f, 0f);

    private Label _label;

    void OnEnable()
    {
        UIDocument uIDocument = GetComponent<UIDocument>();
        VisualElement rootElement = uIDocument.rootVisualElement;
        _label = rootElement.Q<Label>();
        TextField _textField = rootElement.Q<TextField>();
        Button FixSizeManualy = rootElement.Q<Button>("FixSizeManualy");
        FixSizeManualy.RegisterCallback<ClickEvent>(OnClickedFixSize);
        _textField.RegisterValueChangedCallback(OnVaueChangedTextField);
    }

    private void OnVaueChangedTextField(ChangeEvent<string> evt)
    {
        _label.text = evt.newValue;
    }

    private void OnClickedFixSize(ClickEvent evt)
    {
        FixLabelTextSize(_label);
    }

    private void FixLabelTextSize(Label label)
    {
        if(label.contentRect.size == _vector2Zero)
        {
            Debug.Log($"ContentRect size is zero({label.contentRect.size})!!!");
            return;    
        }

        if(label.resolvedStyle.fontSize <= 1)
        {
            Debug.Log($"Font size({label.resolvedStyle.fontSize}) is less than one!!!");
            return;    
        }

        Vector2 measureTextSize = label.MeasureTextSize(label.text, label.contentRect.size.x, VisualElement.MeasureMode.Undefined, label.contentRect.size.y, VisualElement.MeasureMode.Undefined);
        while(measureTextSize.x > label.contentRect.size.x)
        {
            measureTextSize = label.MeasureTextSize(label.text, label.contentRect.size.x, VisualElement.MeasureMode.Undefined, label.contentRect.size.y, VisualElement.MeasureMode.Undefined);
            label.style.fontSize = label.resolvedStyle.fontSize - 1;
            Debug.Log($"FixLabelTextSize-measureTextSize:{measureTextSize}, fontSize:{label.style.fontSize.value.value}/{label.resolvedStyle.fontSize}");
        }
    }
}
