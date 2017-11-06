using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
    public interface IContextProvider
    {
		 T GetItem<T>(String key) where T : class;
		 void SetItem<T>(String key, T value) where T : class;
    }
}
