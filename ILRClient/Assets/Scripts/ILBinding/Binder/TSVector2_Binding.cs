using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class TSVector2_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::TSVector2);
            args = new Type[]{typeof(global::FP), typeof(global::FP)};
            method = type.GetMethod("Set", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Set_0);
            args = new Type[]{};
            method = type.GetMethod("get_one", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_one_1);
            args = new Type[]{typeof(global::TSVector2), typeof(global::TSVector2)};
            method = type.GetMethod("op_Addition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Addition_2);

            app.RegisterCLRCreateDefaultInstance(type, () => new global::TSVector2());

            args = new Type[]{typeof(global::FP), typeof(global::FP)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }

        static void WriteBackInstance(ILRuntime.Runtime.Enviorment.AppDomain __domain, StackObject* ptr_of_this_method, IList<object> __mStack, ref global::TSVector2 instance_of_this_method)
        {
            ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.Object:
                    {
                        __mStack[ptr_of_this_method->Value] = instance_of_this_method;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            var t = __domain.GetType(___obj.GetType()) as CLRType;
                            t.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, instance_of_this_method);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var t = __domain.GetType(ptr_of_this_method->Value);
                        if(t is ILType)
                        {
                            ((ILType)t).StaticInstance[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            ((CLRType)t).SetStaticFieldValue(ptr_of_this_method->ValueLow, instance_of_this_method);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as global::TSVector2[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = instance_of_this_method;
                    }
                    break;
            }
        }

        static StackObject* Set_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::FP @y = new global::FP();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder.ParseValue(ref @y, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @y = (global::FP)typeof(global::FP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::FP @x = new global::FP();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder.ParseValue(ref @x, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @x = (global::FP)typeof(global::FP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::TSVector2 instance_of_this_method = new global::TSVector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.ParseValue(ref instance_of_this_method, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                instance_of_this_method = (global::TSVector2)typeof(global::TSVector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            instance_of_this_method.Set(@x, @y);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            } else {
                WriteBackInstance(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            }

            __intp.Free(ptr_of_this_method);
            return __ret;
        }

        static StackObject* get_one_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::TSVector2.one;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* op_Addition_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::TSVector2 @value2 = new global::TSVector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.ParseValue(ref @value2, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @value2 = (global::TSVector2)typeof(global::TSVector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::TSVector2 @value1 = new global::TSVector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.ParseValue(ref @value1, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @value1 = (global::TSVector2)typeof(global::TSVector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = value1 + value2;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::FP @y = new global::FP();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder.ParseValue(ref @y, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @y = (global::FP)typeof(global::FP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::FP @x = new global::FP();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_FP_Binding_Binder.ParseValue(ref @x, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @x = (global::FP)typeof(global::FP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = new global::TSVector2(@x, @y);

            if(!isNewObj)
            {
                __ret--;
                if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                    ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.WriteBackValue(__domain, __ret, __mStack, ref result_of_this_method);
                } else {
                    WriteBackInstance(__domain, __ret, __mStack, ref result_of_this_method);
                }
                return __ret;
            }

            if (ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_TSVector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
                return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


    }
}
