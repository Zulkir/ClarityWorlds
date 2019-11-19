using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clarity.Common.Infra.DependencyInjection;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    [TestFixture]
    public class DiContainerTests
    {
        #region Types
        interface ITstDummy { }

        interface IAbstract { }
        interface IAbstract2 { }
        class Concrete : IAbstract, IAbstract2 { }

        class ConcreteTwoCtors : IAbstract
        {
            public ConcreteTwoCtors() {}
            public ConcreteTwoCtors(int x) {}
        }

        class ConcretePrivateCtor : IAbstract
        {
            private ConcretePrivateCtor() { }
        }

        interface IAbstractInjectableCtor 
        {
            IAbstract Empty { get; }
        }

        class ConcreteInjectableCtor : IAbstractInjectableCtor
        {
            public IAbstract Empty { get; private set; }

            public ConcreteInjectableCtor(IAbstract empty)
            {
                Empty = empty;
            }
        }
        
        interface IAbstractGeneric<T> {  }
        class ConcreteGeneric<T> : IAbstractGeneric<T> { }

        interface IAbstractGenericInjectableCtor<T>
        {
            IAbstractGeneric<T> Empty { get; }
        }

        class ConcreteInjectableCtor<T> : IAbstractGenericInjectableCtor<T>
        {
            public IAbstractGeneric<T> Empty { get; private set; }

            public ConcreteInjectableCtor(IAbstractGeneric<T> empty)
            {
                Empty = empty;
            }
        }
        #endregion

        private IDiContainer container;

        [SetUp]
        public void Setup()
        {
            container = new DiContainer();
        }
        
        [Test]
        public void GetSelf()
        {
            var containerResult = container.Get<IDiContainer>();
            Assert.That(containerResult, Is.SameAs(container));
        }

        [Test]
        public void BindObject()
        {
            var implementation = new Concrete();
            container.Bind<IAbstract>().To(implementation);
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.SameAs(implementation));
        }

        [Test]
        public void BindObjectIndirect()
        {
            var implementation = new Concrete();
            container.Bind(typeof(IAbstract)).To(implementation);
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.SameAs(implementation));
        }

        [Test]
        public void BindFactoryMethod()
        {
            container.Bind<IAbstract>().To(c => new Concrete());
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.TypeOf<Concrete>());
        }

        [Test]
        public void BindType()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.TypeOf<Concrete>());
        }

        [Test]
        public void BindConstructor()
        {
            var ctor = typeof(Concrete).GetConstructor(Type.EmptyTypes);
            container.Bind<IAbstract>().To(ctor);
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.TypeOf<Concrete>());
        }

        [Test]
        public void IndirectTypeCheckObject()
        {
            Assert.That(() => container.Bind(typeof(ITstDummy)).To(new object()), Throws.Exception);
        }

        [Test]
        public void IndirectTypeCheckFunc()
        {
            Assert.That(() => container.Bind(typeof(ITstDummy)).To(c => new Concrete()), Throws.Exception);
        }

        [Test]
        public void IndirectTypeCheckType()
        {
            Assert.That(() => container.Bind(typeof(ITstDummy)).To(typeof(Concrete)), Throws.Exception);
        }

        [Test]
        public void ManyConstructorsFail()
        {
            Assert.That(() => container.Bind<IAbstract>().To<ConcreteTwoCtors>(), Throws.Exception);
        }

        [Test]
        public void PrivateConstructorFail()
        {
            Assert.That(() => container.Bind<IAbstract>().To<ConcretePrivateCtor>(), Throws.Exception);
        }

        [Test]
        public void NoRecreation()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var result1 = container.Get<IAbstract>();
            var result2 = container.Get<IAbstract>();
            Assert.That(result1, Is.SameAs(result2));
        }

        [Test]
        public void ReuseSameConcreteType()
        {
            container.Bind<IAbstract>().To<Concrete>();
            container.Bind<IAbstract2>().To<Concrete>();
            var result1 = container.Get<IAbstract>();
            var result2 = container.Get<IAbstract2>();
            Assert.That(result1, Is.SameAs(result2));
        }

        [Test]
        public void ReuseSameConcreteTypeAfterConstructor()
        {
            var ctor = typeof(Concrete).GetConstructor(Type.EmptyTypes);
            container.Bind<IAbstract>().To(ctor);
            container.Bind<IAbstract2>().To(ctor);
            var result1 = container.Get<IAbstract>();
            var result2 = container.Get<IAbstract2>();
            Assert.That(result1, Is.SameAs(result2));
        }

        [Test]
        public void ReuseConcreteTypeWithoutOverriding()
        {
            var expected = new Concrete();
            container.Bind<Concrete>().To(expected);
            container.Bind<IAbstract>().To<Concrete>();
            var result = container.Get<IAbstract>();
            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void ReuseConcreteTypeImplicit()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var resultAbstract = container.Get<IAbstract>();
            var resultConcrete = container.Get<Concrete>();
            Assert.That(resultConcrete, Is.SameAs(resultAbstract));
        }

        [Test]
        public void CantBindAfterGet()
        {
            container.Bind<IAbstract>().To<Concrete>();
            container.Get<IAbstract>();
            Assert.That(() => container.Bind<IAbstract>(), Throws.Exception);
        }

        [Test]
        public void SimpleInjection()
        {
            container.Bind<IAbstract>().To<Concrete>();
            container.Bind<IAbstractInjectableCtor>().To<ConcreteInjectableCtor>();
            var result = container.Get<IAbstractInjectableCtor>();
            Assert.That(result, Is.TypeOf<ConcreteInjectableCtor>());
            Assert.That(result.Empty, Is.TypeOf<Concrete>());
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void If(bool condition)
        {
            var obj1 = new Concrete();
            var obj2 = new Concrete();

            container.Bind<IAbstract>()
                .To(obj1).If(c => condition)
                .To(obj2).Otherwise();

            var result = container.Get<IAbstract>();
            var expected = condition ? obj1 : obj2;
            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void DefaultOrder()
        {
            var stringBuilder = new StringBuilder();

            var objFalse = new Concrete();
            var objTrue = new Concrete();

            container.Bind<IAbstract>()
                .To(objFalse).If(c => { stringBuilder.Append("1"); return false; })
                .To(objFalse).If(c => { stringBuilder.Append("2"); return false; })
                .To(objTrue).If(c => { stringBuilder.Append("3"); return true; });

            container.Bind<IAbstract>()
                .To(objFalse).If(c => { stringBuilder.Append("4"); return false; })
                .To(objFalse).If(c => { stringBuilder.Append("5"); return false; });

            var result = container.Get<IAbstract>();
            Assert.That(result, Is.SameAs(objTrue));
            Assert.That(stringBuilder.ToString(), Is.EqualTo("45123"));
        }

        [Test]
        public void LastChoiceOrder()
        {
            var stringBuilder = new StringBuilder();

            var objFalse = new Concrete();
            var objTrue = new Concrete();

            container.Bind<IAbstract>()
                .To(objFalse).If(c => { stringBuilder.Append("1"); return false; })
                .To(objFalse).If(c => { stringBuilder.Append("2"); return false; });

            container.Bind<IAbstract>().AsLastChoice
                .To(objFalse).If(c => { stringBuilder.Append("3"); return false; })
                .To(objFalse).If(c => { stringBuilder.Append("4"); return false; })
                .To(objTrue).If(c => { stringBuilder.Append("5"); return true; });

            var result = container.Get<IAbstract>();
            Assert.That(result, Is.SameAs(objTrue));
            Assert.That(stringBuilder.ToString(), Is.EqualTo("12345"));
        }

        [Test]
        public void GetMulti()
        {
            var impl1 = new Concrete();
            var impl2 = new Concrete();
            container.BindMulti<IAbstract>().To(impl1);
            container.BindMulti<IAbstract>().To(impl2);
            var results = container.GetMulti<IAbstract>();
            Assert.That(results, Is.EquivalentTo(new[] { impl1, impl2 }));
        }

        [Test]
        public void FixRootBindingType()
        {
            container.Bind<IAbstract>().To(new Concrete());
            Assert.That(() => container.BindMulti<IAbstract>(), Throws.Exception);

            container.BindMulti<IAbstract2>().To(new Concrete());
            Assert.That(() => container.Bind<IAbstract2>(), Throws.Exception);
        }

        [Test]
        public void MultiListTypes()
        {
            container.BindMulti<IAbstract>().To(new Concrete());
            container.GetMulti<IAbstract>();
            container.GetMulti(typeof(IAbstract));
        }

        [Test]
        public void BindGeneric()
        {
            container.Bind(typeof(IAbstractGeneric<>)).To(typeof(ConcreteGeneric<>));
            var resultInt = container.Get<IAbstractGeneric<int>>();
            Assert.That(resultInt, Is.TypeOf<ConcreteGeneric<int>>());
            var resultString = container.Get<IAbstractGeneric<string>>();
            Assert.That(resultString, Is.TypeOf<ConcreteGeneric<string>>());
        }

        [Test]
        public void BindGenericConstructor()
        {
            var ctor = typeof(KeyValuePair<,>).GetConstructors().First(x => x.GetParameters().Length == 2);
            container.Bind(typeof(KeyValuePair<,>)).To(ctor);
            container.Bind<IAbstract>().To<Concrete>();
            var result = container.Get<KeyValuePair<IAbstract, IAbstract>>();
            Assert.That(result.Key, Is.TypeOf<Concrete>());
            Assert.That(result.Key, Is.SameAs(result.Value));
        }

        [Test]
        public void Func()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var func = container.Get<Func<IAbstract>>();
            var result1 = func();
            var result2 = func();
            Assert.That(result1, Is.TypeOf<Concrete>());
            Assert.That(result2, Is.TypeOf<Concrete>());
            Assert.That(result2, Is.Not.SameAs(result1));
        }

        [Test]
        public void Lazy()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var result = container.Get<Lazy<IAbstract>>();
            Assert.That(result.Value, Is.TypeOf<Concrete>());
        }

        [Test]
        public void LazySingleton()
        {
            container.Bind<IAbstract>().To<Concrete>();
            var result1 = container.Get<Lazy<IAbstract>>().Value;
            var result2 = container.Get<Lazy<IAbstract>>().Value;
            Assert.That(result2, Is.SameAs(result1));
        }

        [Test]
        public void ReadOnlyList()
        {
            var impl1 = new Concrete();
            var impl2 = new Concrete();
            container.BindMulti<IAbstract>().To(impl1);
            container.BindMulti<IAbstract>().To(impl2);
            var results = container.Get<IReadOnlyList<IAbstract>>();
            Assert.That(results, Is.EquivalentTo(new[] { impl1, impl2 }));
        }

        [Test]
        public void Instantiate()
        {
            container.Bind<IAbstract>().To<Concrete>();
            container.Bind<IAbstractInjectableCtor>().To<ConcreteInjectableCtor>();
            var result1 = container.Instantiate<IAbstractInjectableCtor>();
            Assert.That(result1, Is.TypeOf<ConcreteInjectableCtor>());
            var result2 = container.Instantiate<IAbstractInjectableCtor>();
            Assert.That(result2, Is.Not.SameAs(result1));
        }

        [Test]
        public void InstantiateConcrete()
        {
            container.Bind<IAbstract>().To<Concrete>();
            container.Bind<ConcreteInjectableCtor>().To<ConcreteInjectableCtor>();
            var result1 = container.Instantiate<ConcreteInjectableCtor>();
            var result2 = container.Instantiate<ConcreteInjectableCtor>();
            Assert.That(result2, Is.Not.SameAs(result1));
            Assert.That(result1.Empty, Is.SameAs(result2.Empty));
        }
    }
}