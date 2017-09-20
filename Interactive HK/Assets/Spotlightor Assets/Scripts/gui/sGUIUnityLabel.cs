using UnityEngine;
using System.Collections;

/// <summary>
/// ʹ��������GUIText�ķ��������Unity GUI.Label����ô�����ԵĹ��ܣ���Ҫ��Ϊ����Label��Word Wrap�������ֲ�GUIText�Ĳ���
/// </summary>
[ExecuteInEditMode()]
public class sGUIUnityLabel : sGUIBase
{
    #region Fields
    public string text;
    #endregion

    #region Properties

    #endregion

    #region Functions
    protected override void DrawGUI(Rect drawRect, GUIStyle guiStyle)
    {
        if (guiStyle != null)
        {
            GUI.Label(drawRect, text, guiStyle);
        }
        else GUI.Label(drawRect, text);
    }
    #endregion
}
