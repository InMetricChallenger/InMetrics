﻿using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Services;

[ExcludeFromCodeCoverage]
public class ServiceProviderWrapper : ICustomServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
