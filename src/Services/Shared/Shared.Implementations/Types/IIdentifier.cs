﻿using MongoDB.Bson;

namespace Shared.Implementations.Types;

public interface IIdentifier
{
    Guid Id { get; }
}