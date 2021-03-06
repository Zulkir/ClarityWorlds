﻿using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiToResult
    {
        IDiBindResult If(Func<IDiContainer, bool> condition);
        void Otherwise();
    }

    public interface IDiToResult<in TAbstract>
    {
        IDiBindResult<TAbstract> If(Func<IDiContainer, bool> condition);
        void Otherwise();
    }
}