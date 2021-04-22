using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections.Generic;

public unsafe class TSMatrix4x4Binder : ValueTypeBinder<TSMatrix4x4>
{
    public override unsafe void AssignFromStack(ref TSMatrix4x4 ins, StackObject* ptr, IList<object> mStack)
    {
        int idx = 1;
        var v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M11, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M12, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M13, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M14, v, mStack);

        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M21, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M22, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M23, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M24, v, mStack);

        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M31, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M32, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M33, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        AssignFromStack(ref ins.M34, v, mStack);
    }

    public override unsafe void CopyValueTypeToStack(ref TSMatrix4x4 ins, StackObject* ptr, IList<object> mStack)
    {
        int idx = 1;
        var v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M11, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M12, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M13, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M14, v, mStack);

        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M21, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M22, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M23, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M24, v, mStack);

        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M31, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M32, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M33, v, mStack);
        v = ILIntepreter.Minus(ptr, idx++);
        CopyValueTypeToStack(ref ins.M34, v, mStack);
    }
}
