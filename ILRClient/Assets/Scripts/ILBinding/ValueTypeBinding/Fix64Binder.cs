using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections.Generic;

public unsafe class Fix64Binder : ValueTypeBinder<FP>
{
    public override unsafe void AssignFromStack(ref FP ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        ins._serializedValue = *(long*)&v->Value;
    }

    public override unsafe void CopyValueTypeToStack(ref FP ins, StackObject* ptr, IList<object> mStack)
    {
        var v = ILIntepreter.Minus(ptr, 1);
        *(long*)&v->Value = ins._serializedValue;
    }

    public static void ParseFP(out FP val, ILIntepreter intp, StackObject* ptr, IList<object> mStack)
    {
        var a = ILIntepreter.GetObjectAndResolveReference(ptr);
        if (a->ObjectType == ObjectTypes.ValueTypeObjectReference)
        {
            var src = *(StackObject**)&a->Value;
            val._serializedValue = *(long*)&ILIntepreter.Minus(src, 1)->Value;
            intp.FreeStackValueType(ptr);
        }
        else
        {
            val = (FP)StackObject.ToObject(a, intp.AppDomain, mStack);
            intp.Free(ptr);
        }
    }
}
