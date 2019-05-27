using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clarity.App.Worlds.Interaction.Queries
{
    public interface IUserQueryService
    {
        IReadOnlyList<IUserQuery> Queries { get; }

        Task<int?> QueryOptions(IReadOnlyList<string> options);

        void OnQueryComplete(IUserQuery query);
    }
}