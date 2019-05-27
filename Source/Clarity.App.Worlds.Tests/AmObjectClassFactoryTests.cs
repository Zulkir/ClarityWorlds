using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.ActiveModel.ClassEmitting;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    [TestFixture]
    public class AmObjectClassFactoryTests
    {
        #region Types
        #region ModelRoot and SimpleObj
        // todo: remove
        public abstract class ModelRoot : AmObjectBase<ModelRoot>
        {
            public abstract IAmObject Child { get; set; }

            public event Action<IAmEventMessage> Updated;

            public override void AmOnChildEvent(IAmEventMessage message)
            {
                Updated?.Invoke(message);
            }
        }

        public abstract class SimpleObj : AmObjectBase<SimpleObj>
        {
            public abstract int A { get; set; }
        }

        public abstract class ObjWithTwoBindingTypes : AmObjectBase<ObjWithTwoBindingTypes>
        {
            public abstract string A { get; set; }
            public abstract IList<double> B { get; }
        }
        #endregion

        #region ModelRootWithDeclaredParent
        public abstract class ModelRootWithParent : AmObjectBase<ModelRootWithParent, SimpleObj>
        {
        }
        #endregion

        #region With Ctor
        public abstract class ObjWithCtor : AmObjectBase<ObjWithCtor>
        {
            public bool A { get; }
            public string B { get; }
            public int C { get; }
            public Type D { get; }

            protected ObjWithCtor(bool a, string b, int c, Type d)
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }
        }
        
        #endregion
        
        #region Non-propagating
        public abstract class NonPropagatingObject : AmObjectBase<NonPropagatingObject>
        {
            public abstract int A { get; set; }
            
            public override void AmOnChildEvent(IAmEventMessage message)
            {
                message.StopPropagation = true;
            }
        }
        #endregion

        #region WithFlags
        public abstract class ObjectWithFlags : AmObjectBase<ObjectWithFlags>
        {
            [AmReference]
            public abstract int A { get; set; }

            [AmDerived]
            public abstract int B { get; set; }

            [AmReference, AmDerived]
            public abstract int C { get; set; }
        }
        #endregion

        #region WatcherObject
        public abstract class WatcherObject : AmObjectBase<WatcherObject>
        {
            [AmReference]
            public abstract SimpleObj Reference { get; set; }

            public int EventCount { get; private set; }

            public override void AmOnChildEvent(IAmEventMessage message)
            {
                if (message.Obj(Reference).Affected())
                    EventCount++;
            }
        }

        public abstract class WatcherParent : AmObjectBase<WatcherParent>
        {
            public abstract SimpleObj Simple { get; set; }
            public abstract WatcherObject Watcher { get; set; }

            public int EventCount { get; private set; }

            public override void AmOnChildEvent(IAmEventMessage message)
            {
                EventCount++;
            }
        }
        #endregion

        #region ListOfObjects
        public abstract class ObjectWithListOfObjects : AmObjectBase<ObjectWithListOfObjects>
        {
            public abstract IList<SimpleObj> Objects { get; }
        }
        #endregion

        #region Equatable
        public abstract class CollectionOfEquatables : AmObjectBase<CollectionOfEquatables>
        {
            public abstract IList<EquatableObj> Objects { get; }
        }

        public abstract class EquatableObj : AmObjectBase<EquatableObj>, IEquatable<EquatableObj>
        {
            public abstract int A { get; set; }

            public bool Equals(EquatableObj other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return A == other.A;
            }

            public override bool Equals(object obj) => Equals(obj as EquatableObj);
            public override int GetHashCode() => 0;
        }
        #endregion
        #endregion

        private IAmObjectClassFactory factory;
        // todo: remove
        private ModelRoot model;
        private List<IAmEventMessage> eventsFired;

        private T Create<T>() => (T)factory.CreateObjectClass(typeof(T), null).Instantiate();

        [SetUp]
        public void Setup()
        {
            var bindingTypeDescriptors = new IAmBindingTypeDescriptor[]
            {
                new AmSingularBindingTypeDescriptor(),
                new AmListBindingTypeDescriptor()
            };
            factory = new AmObjectClassFactory(bindingTypeDescriptors);
            model = null;
            eventsFired = null;
        }

        [Test]
        public void ModelRootBasics()
        {
            var modelRootInstantiator = factory.CreateObjectClass(typeof(ModelRoot), null);
            var instance = modelRootInstantiator.Instantiate();
            Assert.That(instance, Is.AssignableTo<ModelRoot>());
            model = (ModelRoot)instance;
            eventsFired = new List<IAmEventMessage>();
            model.Updated += m => { eventsFired.Add(m); };
        }

        [Test]
        public void AmInterface()
        {
            var simpleInstantiator = factory.CreateObjectClass(typeof(SimpleObj), null);
            var instance = simpleInstantiator.Instantiate();
            Assert.That(instance.AmInterface, Is.EqualTo(typeof(SimpleObj)));
        }
        
        [Test]
        public void ModelRootWithParentBasics()
        {
            var modelRootInstantiator = factory.CreateObjectClass(typeof(ModelRootWithParent), null);
            var instance = modelRootInstantiator.Instantiate();
            Assert.That(instance, Is.AssignableTo<ModelRootWithParent>());
            var modelRoot = (ModelRootWithParent)instance;
            Assert.DoesNotThrow(() => { var p = modelRoot.AmParent; });
        }

        [Test]
        public void AmParent()
        {
            var parent = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var obj = (SimpleObj)factory.CreateObjectClass(typeof(SimpleObj), null).Instantiate();
            parent.Child = obj;
            Assert.That(obj.AmParent, Is.EqualTo(parent));
        }

        [Test]
        public void SimpleBindings()
        {
            ModelRootBasics();
            var simpleInstantiator = factory.CreateObjectClass(typeof(SimpleObj), null);
            var instance = simpleInstantiator.Instantiate();
            Assert.That(instance, Is.AssignableTo<SimpleObj>());
            var obj = (SimpleObj)instance;
            model.Child = obj;
            Assert.That(eventsFired.Count, Is.EqualTo(1));
            obj.A = 123;
            Assert.That(eventsFired.Last().Obj<SimpleObj>().ValueChanged(x => x.A, out _));
        }
        
        [Test]
        public void DifferentBindingTypes()
        {
            ModelRootBasics();
            var simpleInstantiator = factory.CreateObjectClass(typeof(ObjWithTwoBindingTypes), null);
            var instance = simpleInstantiator.Instantiate();
            Assert.That(instance, Is.AssignableTo<ObjWithTwoBindingTypes>());
            var obj = (ObjWithTwoBindingTypes)instance;
            model.Child = obj;
            obj.A = "asd";
            Assert.That(eventsFired.Last().Obj<ObjWithTwoBindingTypes>().ValueChanged(x => x.A, out _));
            obj.B.Add(123.0);
            Assert.That(eventsFired.Last().Obj<ObjWithTwoBindingTypes>().ItemAdded(x => x.B, out _));
        }

        [Test]
        public void ObjWithConstructor()
        {
            ModelRootBasics();
            Func<Type, object> getDependency = x =>
            {
                if (x == typeof(bool))
                    return true;
                if (x == typeof(string))
                    return "ASD";
                if (x == typeof(int))
                    return 123;
                if (x == typeof(Type))
                    return typeof(DateTime);
                throw new ArgumentOutOfRangeException();
            };
            var instantiator = factory.CreateObjectClass(typeof(ObjWithCtor), getDependency);
            var instance = instantiator.Instantiate();
            Assert.That(instance, Is.AssignableTo<ObjWithCtor>());
            var obj = (ObjWithCtor)instance;
            Assert.That(obj.A, Is.True);
            Assert.That(obj.B, Is.EqualTo("ASD"));
            Assert.That(obj.C, Is.EqualTo(123));
            Assert.That(obj.D, Is.EqualTo(typeof(DateTime)));
            model.Child = obj;
        }

        [Test]
        public void Clone()
        {
            var modelRootInstantiator = factory.CreateObjectClass(typeof(ModelRoot), null);
            var modelRoot = (ModelRoot)modelRootInstantiator.Instantiate();
            var simpleObjInstantiator = factory.CreateObjectClass(typeof(SimpleObj), null);
            var simpleObj = (SimpleObj)simpleObjInstantiator.Instantiate();
            simpleObj.A = 123;
            modelRoot.Child = simpleObj;
            var clone = modelRoot.CloneTyped();
            Assert.That(clone, Is.Not.Null);
            Assert.That(clone, Is.Not.SameAs(modelRoot));
            //Assert.That(clone.AmCore, Is.Not.SameAs(modelRoot.AmCore));
            Assert.That(clone.Child, Is.AssignableTo<SimpleObj>());
            var simpleClone = (SimpleObj)clone.Child;
            Assert.That(simpleClone.A, Is.EqualTo(123));
            Assert.That(simpleClone, Is.Not.SameAs(simpleObj));
        }

        [Test]
        public void CloneDi()
        {
            ModelRootBasics();
            ObjWithConstructor();
            var obj = (ObjWithCtor)model.Child.Clone();
            Assert.That(obj.A, Is.True);
            Assert.That(obj.B, Is.EqualTo("ASD"));
            Assert.That(obj.C, Is.EqualTo(123));
            Assert.That(obj.D, Is.EqualTo(typeof(DateTime)));
        }

        [Test]
        public void StopPropagation()
        {
            ModelRootBasics();
            var instantiator = factory.CreateObjectClass(typeof(NonPropagatingObject), null);
            var obj = (NonPropagatingObject)instantiator.Instantiate();
            model.Child = obj;
            var numEvents = eventsFired.Count;
            obj.A = 123;
            Assert.That(eventsFired.Count, Is.EqualTo(numEvents));
        }

        [Test]
        public void ThrowOnImplicitReparenting()
        {
            var parent1 = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var parent2 = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var obj = (SimpleObj)factory.CreateObjectClass(typeof(SimpleObj), null).Instantiate();
            parent1.Child = obj;
            Assert.That(() => { parent2.Child = obj; }, Throws.Exception);
        }

        [Test]
        public void FalseReparenting()
        {
            var parent1 = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var obj = (SimpleObj)factory.CreateObjectClass(typeof(SimpleObj), null).Instantiate();
            parent1.Child = obj;
            Assert.That(() => { parent1.Child = obj; }, Throws.Nothing);
        }

        [Test]
        public void ExplicitReparenting()
        {
            var parent1 = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var parent2 = (ModelRoot)factory.CreateObjectClass(typeof(ModelRoot), null).Instantiate();
            var obj = (SimpleObj)factory.CreateObjectClass(typeof(SimpleObj), null).Instantiate();
            parent1.Child = obj;
            Assert.That(obj.AmParent, Is.EqualTo(parent1));
            parent1.Child = null;
            Assert.That(obj.AmParent, Is.Null);
            parent2.Child = obj;
            Assert.That(obj.AmParent, Is.EqualTo(parent2));
            obj.Deparent();
            Assert.That(obj.AmParent, Is.Null);
            parent1.Child = obj;
            Assert.That(obj.AmParent, Is.EqualTo(parent1));
        }

        [Test]
        public void Flags()
        {
            var obj = (ObjectWithFlags)factory.CreateObjectClass(typeof(ObjectWithFlags), null).Instantiate();
            var bindings = obj.Bindings.ToDictionary(x => x.PropertyName);
            Assert.That(bindings["A"].Flags, Is.EqualTo(AmBindingFlags.Reference));
            Assert.That(bindings["B"].Flags, Is.EqualTo(AmBindingFlags.Derived));
            Assert.That(bindings["C"].Flags, Is.EqualTo(AmBindingFlags.Reference | AmBindingFlags.Derived));
        }

        [Test]
        public void DontCloneDerived()
        {
            var obj = Create<ObjectWithFlags>();
            obj.A = 123;
            obj.B = 234;
            obj.C = 345;
            var clone = obj.CloneTyped();
            Assert.That(clone.A, Is.EqualTo(123));
            Assert.That(clone.B, Is.EqualTo(0));
            Assert.That(clone.C, Is.EqualTo(0));
        }

        [Test]
        public void ReferencingOthersChild()
        {
            var parent = Create<ModelRoot>();
            var watcher = Create<WatcherObject>();
            var child = Create<SimpleObj>();
            parent.Child = child;
            Assert.That(() => watcher.Reference = child, Throws.Nothing);
        }

        [Test]
        public void ReferenceEvents()
        {
            var watcher = Create<WatcherObject>();
            var obj = Create<SimpleObj>();
            obj.A = 123;
            watcher.Reference = obj;
            obj.A = 234;
            obj.A = 345;
            Assert.That(watcher.EventCount, Is.EqualTo(2));
        }

        [Test, Ignore]
        public void CloneReferencesSimple()
        {
            var watcher = Create<WatcherObject>();
            var obj = Create<SimpleObj>();
            watcher.Reference = obj;
            var watcherClone = watcher.CloneTyped();
            Assert.That(watcherClone.Reference, Is.EqualTo(watcher.Reference));
        }

        [Test]
        public void CloneReferencesEntangled()
        {
            var watcherParent = Create<WatcherParent>();
            watcherParent.Simple = Create<SimpleObj>();
            watcherParent.Simple.A = 123;
            watcherParent.Watcher = Create<WatcherObject>();
            watcherParent.Watcher.Reference = watcherParent.Simple;
            var clone = watcherParent.CloneTyped();
            Assert.That(clone.Watcher.Reference, Is.EqualTo(clone.Simple));
            Assert.That(clone.Watcher.Reference, Is.Not.EqualTo(watcherParent.Watcher.Reference));
            Assert.That(clone.Watcher.Reference.A, Is.EqualTo(123));
        }

        [Test]
        public void MultiReferencing()
        {
            var obj = Create<SimpleObj>();
            var watcher1 = Create<WatcherObject>();
            var watcher2 = Create<WatcherObject>();
            watcher1.Reference = obj;
            watcher2.Reference = obj;
            obj.A = 123;
            Assert.That(watcher1.EventCount, Is.EqualTo(1));
            Assert.That(watcher2.EventCount, Is.EqualTo(1));
        }

        [Test]
        public void DontPropagateReferenceEvents()
        {
            var obj = Create<SimpleObj>();
            var watcher = Create<WatcherObject>();
            var watcherParent = Create<WatcherParent>();
            watcher.Reference = obj;
            watcherParent.Watcher = watcher;
            obj.A = 123;
            Assert.That(watcherParent.EventCount, Is.EqualTo(1));
        }

        [Test]
        public void CloneListOfAmObjects()
        {
            var parent = Create<ObjectWithListOfObjects>();
            parent.Objects.Add(Create<SimpleObj>());
            parent.Objects.Add(Create<SimpleObj>());
            parent.Objects[0].A = 123;
            parent.Objects[1].A = 234;
            var clone = parent.CloneTyped();
            Assert.That(clone.Objects.Count, Is.EqualTo(parent.Objects.Count));
            Assert.That(clone.Objects[0], Is.Not.EqualTo(parent.Objects[0]));
            Assert.That(clone.Objects[1], Is.Not.EqualTo(parent.Objects[1]));
            Assert.That(clone.Objects[0].A, Is.EqualTo(parent.Objects[0].A));
            Assert.That(clone.Objects[1].A, Is.EqualTo(parent.Objects[1].A));
        }

        [Test]
        public void CloneEquatables()
        {
            var parent = Create<CollectionOfEquatables>();
            var child1 = Create<EquatableObj>();
            var child2 = Create<EquatableObj>();
            child1.A = child2.A = 123;
            parent.Objects.Add(child1);
            parent.Objects.Add(child2);

            var parentClone = parent.CloneTyped();
            Assert.That(parentClone.Objects[0], Is.Not.SameAs(parentClone.Objects[1]));
        }
    }
}