using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Clarity.Common.Infra.ActiveModel
{
    public static class AmEventMessageExtensions
    {
        public struct ObjResult<TObj> where TObj : class, IAmObject
        {
            private readonly IAmEventMessage message;
            private readonly TObj exactObj;

            public ObjResult(IAmEventMessage message, TObj exactObj = null)
            {
                this.message = message;
                this.exactObj = exactObj;
            }

            private bool ObjectIsSuitable(IAmObject obj) => exactObj != null ? exactObj == obj : obj is TObj;

            public bool Affected()
            {
                foreach (var binding in message.BindingStack)
                {
                    if (ObjectIsSuitable(binding.OwnerObject))
                        return true;
                }
                return false;
            }

            public bool Affected(string propertyName) => message.BindingStack.Any(x => x.OwnerObject is TObj && x.PropertyName == propertyName);
            public bool Affected<TVal>(Expression<Func<TObj, TVal>> propertyLambda) => Affected(GetPropNameFromLambda(propertyLambda));
            public bool Affected<TVal>(Expression<Func<TObj, IList<TVal>>> propertyLambda) => Affected(GetPropNameFromLambda(propertyLambda));

            public bool ValueChanged<TVal>(string propertyName, out IAmValueChangedEventMessage<TObj, TVal> cmessage)
            {
                cmessage = message as IAmValueChangedEventMessage<TObj, TVal>;
                return ObjectIsSuitable(message.Object) && cmessage?.Binding.PropertyName == propertyName;
            }

            public bool ValueChanged<TVal>(Expression<Func<TObj, TVal>> propertyLambda, out IAmValueChangedEventMessage<TObj, TVal> cmessage) =>
                ValueChanged(GetPropNameFromLambda(propertyLambda), out cmessage);

            public bool ItemAdded<TVal>(string propertyName, out IAmItemAddedEventMessage<TObj, TVal> cmessage)
            {
                cmessage = message as IAmItemAddedEventMessage<TObj, TVal>;
                return ObjectIsSuitable(message.Object) && cmessage?.Binding.PropertyName == propertyName;
            }

            public bool ItemAdded<TVal>(Expression<Func<TObj, IList<TVal>>> propertyLambda, out IAmItemAddedEventMessage<TObj, TVal> cmessage) =>
                ItemAdded(GetPropNameFromLambda(propertyLambda), out cmessage);

            public bool ItemRemoved<TVal>(string propertyName, out IAmItemRemovedEventMessage<TObj, TVal> cmessage)
            {
                cmessage = message as IAmItemRemovedEventMessage<TObj, TVal>;
                return ObjectIsSuitable(message.Object) && cmessage?.Binding.PropertyName == propertyName;
            }

            public bool ItemRemoved<TVal>(Expression<Func<TObj, IList<TVal>>> propertyLambda, out IAmItemRemovedEventMessage<TObj, TVal> cmessage) =>
                ItemRemoved(GetPropNameFromLambda(propertyLambda), out cmessage);

            public bool ItemAddedOrRemoved<TVal>(string propertyName, out IAmItemEventMessage<TObj, TVal> cmessage)
            {
                cmessage = message as IAmItemEventMessage<TObj, TVal>;
                return ObjectIsSuitable(message.Object) && cmessage?.Binding.PropertyName == propertyName;
            }

            public bool ItemAddedOrRemoved<TVal>(Expression<Func<TObj, IList<TVal>>> propertyLambda, out IAmItemEventMessage<TObj, TVal> cmessage) =>
                ItemAddedOrRemoved(GetPropNameFromLambda(propertyLambda), out cmessage);

            private static string GetPropNameFromLambda(LambdaExpression lambda) => ((MemberExpression)lambda.Body).Member.Name;
        }

        public static ObjResult<TObj> Obj<TObj>(this IAmEventMessage message, TObj obj = null)
            where TObj : class, IAmObject =>
                new ObjResult<TObj>(message, obj);

        public static bool ValueChanged<TObj, TVal>(this IAmEventMessage message, string propertyName)
            where TObj : IAmObject => 
                (message as AmValueChangedEventMessage<TObj, TVal>)?.Binding.PropertyName == propertyName;

        public static bool ItemAdded<TObj, TVal>(this IAmEventMessage message, string propertyName)
            where TObj : IAmObject =>
                (message as AmItemAddedEventMessage<TObj, TVal>)?.Binding.PropertyName == propertyName;

        public static bool ItemRemoved<TObj, TVal>(this IAmEventMessage message, string propertyName)
            where TObj : IAmObject =>
                (message as AmItemRemovedEventMessage<TObj, TVal>)?.Binding.PropertyName == propertyName;
    }
}