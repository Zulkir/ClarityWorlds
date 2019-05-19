using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Objects.WorldTree;
using JetBrains.Annotations;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IWorldTreeService
    {
        [CanBeNull]
        IWorld ParentWorld { get; set; }

        [NotNull]
        IWorld World { get; set; }

        ISceneNode MainRoot { get; }

        // todo: resolve ordering issues
        event Action<IAmEventMessage> UpdatedHigh;
        event Action<IAmEventMessage> UpdatedMed;

        ISceneNode GetById(int id);
        bool TryGetById(int id, out ISceneNode node);
    }
}