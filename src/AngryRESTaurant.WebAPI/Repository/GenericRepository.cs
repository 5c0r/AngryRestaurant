using AngryRESTaurant.WebAPI.Model;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AngryRESTaurant.WebAPI.Repository
{
    public interface IRepository<T> where T: IHaveGuid
    {
        IEnumerable<T> QueryAll();
        Task<IEnumerable<T>> QueryAllAsync();
        IEnumerable<T> Search(Expression<Func<T, bool>> searchQuery);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> searchQuery);

        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);


        IEnumerable<T> GetByIds(IEnumerable<Guid> ids);
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids);

        void Upsert(T upsertObject);
        Task UpsertAsync(T upsertObject);

        void UpsertMultiple(IEnumerable<T> upsertObjects);
        Task UpsertMultipleAsync(IEnumerable<T> upsertObjects);
        void Delete(Guid id);
        Task DeleteAsync(Guid id);

    }
    // TODO: Learn again about lightweight, open and query session @Marten
    public sealed class GenericRepository<T> : IRepository<T> where T : IHaveGuid
    {
        private readonly IDocumentStore _store;

        public GenericRepository(IDocumentStore store)
        {
            _store = store;
        }

        public void Delete(Guid id)
        {
            DoSomethingWithStore(session =>
            {
                session.Delete(id);
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            await DoSomethingWithStoreAsync(session =>
            {
                session.Delete(id);
            });
        }

        public T GetById(Guid id)
        {
            return DoQuery(x => x.Id == id).Single();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return (await DoQueryAsync(x => x.Id == id)).Single();
        }

        public IEnumerable<T> GetByIds(IEnumerable<Guid> ids)
        {
            using var querySession = _store.QuerySession();
            return querySession.LoadMany<T>(ids);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            using var querySession = _store.QuerySession();
            return await querySession.LoadManyAsync<T>(ids);
        }

        public IEnumerable<T> QueryAll()
        {
            return DoQuery();
        }

        public async Task<IEnumerable<T>> QueryAllAsync()
        {
            return DoQuery();
        }

        public IEnumerable<T> Search(Expression<Func<T, bool>> searchQuery)
        {
            return DoQuery(searchQuery);
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> searchQuery)
        {
            return await DoQueryAsync(searchQuery);
        }

        public void Upsert(T upsertObject)
        {
            DoSomethingWithStore(session =>
            {
                session.Store(upsertObject);
            });
        }

        public async Task UpsertAsync(T upsertObject)
        {
            await DoSomethingWithStoreAsync(session =>
            {
                session.Store(upsertObject);
            });
        }

        public void UpsertMultiple(IEnumerable<T> upsertObjects)
        {
            DoSomethingWithStore(session =>
            {
                session.Store<T>(upsertObjects);
            });
        }

        public async Task UpsertMultipleAsync(IEnumerable<T> upsertObjects)
        {
            await DoSomethingWithStoreAsync(session =>
            {
                session.Store<T>(upsertObjects);
            });
        }

        private void DoSomethingWithStore(Action<IDocumentSession> documentSessionAction)
        {
            using var documentSession = _store.OpenSession();
            documentSessionAction.Invoke(documentSession);

            documentSession.SaveChanges();
        }

        private async Task DoSomethingWithStoreAsync(Action<IDocumentSession> documentSessionAction)
        {
            using var documentSession = _store.OpenSession();
            documentSessionAction.Invoke(documentSession);

            await documentSession.SaveChangesAsync();
        }

        private IEnumerable<T> DoQuery(Expression<Func<T,bool>> searchQuery = null)
        {
            using var querySession = _store.QuerySession();
            var queryResult = querySession.Query<T>();

            if (searchQuery == null)
                return queryResult.ToList();

            return queryResult.Where(searchQuery).ToList();
        }

        private async Task<IReadOnlyList<T>> DoQueryAsync(Expression<Func<T,bool>> searchQuery = null)
        {
            using var querySession = _store.OpenSession();
            var queryResult = querySession.Query<T>();

            if (searchQuery == null)
                return await queryResult.ToListAsync();

            return await queryResult.Where(searchQuery).ToListAsync().ConfigureAwait(false);
        }
    }
}
