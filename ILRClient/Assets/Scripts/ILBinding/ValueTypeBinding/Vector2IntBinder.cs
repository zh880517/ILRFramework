using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections.Generic;
using UnityEngine;

public class Vector2IntBinder : ValueTypeBinder<Vector2Int>
{
    public override unsafe void AssignFromStack(ref Vector2Int ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        ins.x = *(int*)&v->Value;
        v = ILIntepreter.Minus(ptr, 2);
        ins.y = *(int*)&v->Value;
    }

    public override unsafe void CopyValueTypeToStack(ref Vector2Int ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        *(int*)&v->Value = ins.x;
        v = ILIntepreter.Minus(ptr, 2);
        *(int*)&v->Value = ins.y;
    }
}
