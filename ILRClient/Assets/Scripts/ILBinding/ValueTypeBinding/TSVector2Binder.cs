using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections.Generic;

public unsafe class TSVector2Binder : ValueTypeBinder<TSVector2>
{

    public override unsafe void AssignFromStack(ref TSVector2 ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        AssignFromStack(ref ins.x, v, mStack);
        v = ILIntepreter.Minus(ptr, 2);
        AssignFromStack(ref ins.y, v, mStack);

    }

    public override unsafe void CopyValueTypeToStack(ref TSVector2 ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        CopyValueTypeToStack(ref ins.x, v, mStack);
        v = ILIntepreter.Minus(ptr, 2);
        CopyValueTypeToStack(ref ins.y, v, mStack);
    }

}
