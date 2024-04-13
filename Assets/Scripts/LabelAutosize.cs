/**
 * @author:  Müjdat KIRIKKAYA
 * @date: April 2024 
 */

using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class LabelAutosize : MonoBehaviour
{
    private const string _autosizeLabelName = "autosize-label";
    private const string _fixButtonName = "controllers__fix-button";
    private const string _sizeToggleName = "controllers__size-toggle";
    private const string _manualToggleName = "controllers__manual-toggle";

    private Label _label;
    private Button _fixButton;
    private TextField _textField;

    private Toggle _fixSizeToggle;
    private Toggle _manualToggle;

    private float _labelDefaultSize;


    void OnEnable()
    {
        UIDocument uIDocument = GetComponent<UIDocument>();
        VisualElement rootElement = uIDocument.rootVisualElement;
        _label = rootElement.Q<Label>(_autosizeLabelName);
        _label.RegisterCallbackOnce<GeometryChangedEvent>(OnLabelGeometryChangedEvent);
        _label.RegisterValueChangedCallback(OnLabelValueChanged);
        _textField = rootElement.Q<TextField>();
        _textField.RegisterValueChangedCallback(OnTextFieldVaueChanged);
        _fixButton = rootElement.Q<Button>(_fixButtonName);
        _fixButton.RegisterCallback<ClickEvent>(OnFixSizeClicked);
        _fixSizeToggle = rootElement.Q<Toggle>(_sizeToggleName);
        _fixSizeToggle.RegisterValueChangedCallback(OnFixeSizeToogleValueChanged);
        _manualToggle = rootElement.Q<Toggle>(_manualToggleName);
        _manualToggle.RegisterValueChangedCallback(OnManualToogleValueChanged);

        LoadToggleDatasAndSetRelatedElements();
    }

    private void OnDestroy()
    {
        if(_textField != null)
            _textField.UnregisterValueChangedCallback(OnTextFieldVaueChanged);
        if(_fixButton != null)
            _fixButton.UnregisterCallback<ClickEvent>(OnFixSizeClicked);
    }

    private void LoadToggleDatasAndSetRelatedElements()
    {
        _fixSizeToggle.SetValueWithoutNotify(PlayerPrefs.GetInt(_sizeToggleName, 1) == 1);
        _manualToggle.SetValueWithoutNotify(PlayerPrefs.GetInt(_manualToggleName, 0) == 1);
        _fixButton.SetEnabled(_manualToggle.value);
    }

    private void SetLabelFlexBasis()
    {
        _label.RegisterCallbackOnce<GeometryChangedEvent>(OnLabelGeometryChangedEvent2);
        _label.style.flexBasis = _fixSizeToggle.value ? _labelDefaultSize : StyleKeyword.None;
    }


    #region  Events

    private void OnLabelGeometryChangedEvent(GeometryChangedEvent evt)
    {
        _labelDefaultSize = _label.resolvedStyle.flexBasis.value;
        SetLabelFlexBasis();
    }
    private void OnLabelGeometryChangedEvent2(GeometryChangedEvent evt)
    {
        _label.FixLabelTextSize(false);
    }

    private void OnFixeSizeToogleValueChanged(ChangeEvent<bool> evt)
    {
        SetLabelFlexBasis();
        PlayerPrefs.SetInt(_sizeToggleName, _fixSizeToggle.value ? 1 : 0);
    }

    private void OnManualToogleValueChanged(ChangeEvent<bool> evt)
    {
        _fixButton.SetEnabled(_manualToggle.value);
        if(!_manualToggle.value)
            _label.FixLabelTextSize(false);
        PlayerPrefs.SetInt(_manualToggleName, _manualToggle.value ? 1 : 0);
    }

    private void OnTextFieldVaueChanged(ChangeEvent<string> evt)
    {
        _label.text = evt.newValue;
    }

    private void OnLabelValueChanged(ChangeEvent<string> evt)
    {
        if(!_manualToggle.value)
            _label.FixLabelTextSize(true, evt.newValue.Length > evt.previousValue.Length);
    }
    private void OnFixSizeClicked(ClickEvent evt)
    {
        _label.FixLabelTextSize(false);
    }

    #endregion
}
