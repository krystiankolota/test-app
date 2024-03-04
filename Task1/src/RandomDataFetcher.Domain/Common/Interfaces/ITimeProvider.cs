﻿namespace RandomDataFetcher.Domain.Common.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
