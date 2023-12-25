﻿namespace CaseMngmt.Models.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {

        List<TEntity> GetAll();
        List<TEntity> Get(Func<TEntity, bool> where);
        void Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void Remove(IEnumerable<TEntity> entities);
        void SyncDisconnected(TEntity entity, bool forDeletion = false);
        void SyncDisconnected(IEnumerable<TEntity> entities, bool forDeletion = false);
    }
}
