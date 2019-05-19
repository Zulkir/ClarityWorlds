using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Engine.Utilities
{
    public static class AmFactory
    {
        private static IAmDiBasedObjectFactory factory;

        public static IAmDiBasedObjectFactory Factory => factory;

        public static void Initialize(IAmDiBasedObjectFactory actualFactory)
        {
            factory = actualFactory;
        }

        public static T Create<T>()
            where T : IAmObject
        {
            return factory.Create<T>();
        }
    }
}