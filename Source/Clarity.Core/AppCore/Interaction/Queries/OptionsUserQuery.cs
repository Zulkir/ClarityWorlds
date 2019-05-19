using System.Collections.Generic;

namespace Clarity.Core.AppCore.Interaction.Queries
{
    public class OptionsUserQuery : IUserQuery
    {
        private readonly IUserQueryService queryService;
        public IReadOnlyList<string> Options { get; }
        public int? Result { get; private set; }
        public bool IsComplete { get; private set; }

        public OptionsUserQuery(IUserQueryService queryService, IReadOnlyList<string> options)
        {
            this.queryService = queryService;
            Options = options;
        }

        public void Choose(int? option)
        {
            Result = option;
            IsComplete = true;
            queryService.OnQueryComplete(this);
        }
    }
}