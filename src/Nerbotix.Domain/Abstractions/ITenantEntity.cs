﻿namespace Nerbotix.Domain.Abstractions;

public interface ITenantEntity<TKey> : IEntity<TKey>
{
    TKey TenantId { get; set; }
}