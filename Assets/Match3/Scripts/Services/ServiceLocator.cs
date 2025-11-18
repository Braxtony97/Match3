using System;
using System.Collections.Generic;

public class ServiceLocator
{
    public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

    private static ServiceLocator _instance;

    private readonly Dictionary<Type, IService> _services = new();

    public void Register<TService>(TService service) where TService : IService
    {
        Type type = typeof(TService);
        if (_services.ContainsKey(type))
            throw new Exception($"Service {type} already registered");

        _services[typeof(TService)] = service;
    }

    public TService Resolve<TService>() where TService : IService
    {
        if (_services.TryGetValue(typeof(TService), out var service))
            return (TService)service;

        throw new Exception($"Service of type {typeof(TService)} not found");
    }
}
