using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Clarity.Core.AppCore.Coroutines;

namespace Clarity.Core.AppCore.Interaction.Queries
{
    public class UserQueryService : IUserQueryService
    {
        private readonly ICoroutineService coroutineService;
        private readonly List<IUserQuery> queries;
        public event Action Updated;

        public IReadOnlyList<IUserQuery> Queries => queries;

        public UserQueryService(ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            queries = new List<IUserQuery>();
        }

        public async Task<int?> QueryOptions(IReadOnlyList<string> options)
        {
            var query = new OptionsUserQuery(this, options);
            queries.Add(query);
            Updated?.Invoke();
            await coroutineService.WaitCondition(() => query.IsComplete);
            return query.Result;
        }

        public void OnQueryComplete(IUserQuery query)
        {
            Debug.Assert(query.IsComplete, "query.IsComplete");
            queries.Remove(query);
            Updated?.Invoke();
        }
    }
}