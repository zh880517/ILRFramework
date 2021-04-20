using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomExportDefine
{
    public List<Type> valueTypeBinders;
    public List<Type> delegateTypes;

    public CustomExportDefine()
    {
//         VT<Vector3>();
//         VT<Vector2>();
//         VT<Quaternion>();

        DT<UnityEngine.Events.UnityAction<float>>();
        DT<UnityEngine.Events.UnityAction<Vector2>>();
    }

    private void VT<T>() where T : struct
    {
        if (valueTypeBinders == null)
        {
            valueTypeBinders = new List<Type>();
        }
        Type type = typeof(T);
        if (!valueTypeBinders.Contains(type))
        {
            valueTypeBinders.Add(type);
        }
    }

    private void DT<T>()
    {
        if (delegateTypes == null)
        {
            delegateTypes = new List<Type>();
        }
    }
}
