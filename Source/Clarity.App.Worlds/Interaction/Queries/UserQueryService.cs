using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Clarity.App.Worlds.Coroutines;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.Interaction.Queries
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IEventRoutingService eventRoutingService;
        private readonly ICoroutineService coroutineService;
        private readonly List<IUserQuery> queries;

        public IReadOnlyList<IUserQuery> Queries => queries;

        public UserQueryService(IEventRoutingService eventRoutingService, ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            this.eventRoutingService = eventRoutingService;
            queries = new List<IUserQuery>();
        }

        public async Task<int?> QueryOptions(IReadOnlyList<string> options)
        {
            var query = new OptionsUserQuery(this, options);
            queries.Add(query);
            eventRoutingService.FireEvent<IUserQueryEvent>(new UserQueryEvent());
            await coroutineService.WaitCondition(() => query.IsComplete);
            return query.Result;
        }

        public void OnQueryComplete(IUserQuery query)
        {
            Debug.Assert(query.IsComplete, "query.IsComplete");
            queries.Remove(query);
            eventRoutingService.FireEvent<IUserQueryEvent>(new UserQueryEvent());
        }
    }
}