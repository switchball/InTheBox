using UnityEngine;
using System.Collections;

public abstract class GenericInputReciever:EnhancedMonoBehaviour
{
    /// <summary>
    /// �ⲿ�����˷��÷�����Input������������в�ͬ��RecieverӦ��������Ӧ�ķ���
    /// </summary>
    /// <param name="x">ˮƽ����input</param>
    /// <param name="y">��ֱ����input</param>
    /// <param name="z">�����input</param>
    public abstract void OnDirectionInput(float x, float y, float z);

    /// <summary>
    /// �ⲿ�����˷������к���Input�����кϸ񷽷��в�ͬ��RecieverӦ�Բ�ͬ�����������Ӧ�ķ���
    /// </summary>
    /// <param name="index">��ţ�e.g. 0/1/2/3...</param>
    public abstract void OnIndexInput(uint index);
}