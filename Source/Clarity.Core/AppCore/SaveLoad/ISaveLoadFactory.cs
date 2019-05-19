﻿using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ISaveLoadFactory
    {
        ITrwSerializationWriteContext MetaWriteContext(ITrwWriter writer);
        ITrwSerializationReadContext MetaReadContext(ITrwReader reader);

        ITrwSerializationWriteContext WorldWriteContext(ITrwWriter writer);
        ITrwSerializationReadContext WorldReadContext(ITrwReader reader, IReadOnlyDictionary<string, Type> typeAliases);

        ITrwSerializationWriteContext AliasesWriteContext(ITrwWriter writer);
        ITrwSerializationReadContext AliasesReadContext(ITrwReader reader);

        ITrwSerializationWriteContext AssetsInfoWriteContext(ITrwWriter writer);
        ITrwSerializationReadContext AssetsInfoReadContext(ITrwReader reader);
    }
}