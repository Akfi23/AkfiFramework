﻿namespace _Source.Code.Interfaces
{
    public interface ISaveService : IAKService
    {
        public bool Has(string key);
        T Load<T>(string key, T value);
        void Save<T>(string key, T value);
        void Remove(string key);
    }
}