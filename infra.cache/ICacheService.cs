﻿using model.Entities;

namespace infra.cache
{
    public interface ICacheService<T, ID> where T : BaseEntity<ID>
    {
        Task<T> Save(T entity);
        Task<T> Get(ID key);
    }
}
