﻿namespace OrangeFinance.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task setCache<T>(string key, T value);
    Task<T?> getCache<T>(string key);

}
