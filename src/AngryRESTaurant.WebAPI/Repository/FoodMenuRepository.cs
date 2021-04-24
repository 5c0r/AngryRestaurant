using AngryRESTaurant.WebAPI.Model;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AngryRESTaurant.WebAPI.Repository
{
    // A simple Synchronous Food menu repository , no async for now , 
    public sealed class FoodMenuRepository
    {
        private readonly IDocumentStore _store;

        public FoodMenuRepository(IDocumentStore store)
        {
            this._store = store;
        }

        public IEnumerable<FoodMenu> GetAllFoodMenu()
        {
            using var querySession = _store.QuerySession();
            return querySession.Query<FoodMenu>().ToList();

        }

        public IEnumerable<FoodMenu> SearchFoodMenuByName(string name)
        {
            using var querySession = _store.QuerySession();
            return querySession.Query<FoodMenu>()
                .Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IEnumerable<FoodMenu> SearchFoodMenu(Func<FoodMenu,bool> searchQuery)
        {
            using var querySession = _store.QuerySession();
            return querySession.Query<FoodMenu>().Where(searchQuery).ToList();
        }

        public FoodMenu CreateFoodMenu(IFoodMenuPayload payload)
        {
            using var openSession = _store.OpenSession();

            var newFoodMenu = new FoodMenu
            {
                Id = Guid.NewGuid(),
                CookingTime = payload.CookingTime, 
                IsDifficult = payload.IsDifficult, 
                Name = payload.Name
            };

            openSession.Store(newFoodMenu);
            openSession.SaveChanges();

            return newFoodMenu;
        }

        public IEnumerable<FoodMenu> CreateALotOfFoodMenu(IEnumerable<IFoodMenuPayload> payload)
        {
            using var openSession = _store.OpenSession();

            var newFoodMenus = payload.Select(x => new FoodMenu
            {
                Id = Guid.NewGuid(),
                CookingTime = x.CookingTime,
                IsDifficult = x.IsDifficult,
                Name = x.Name
            });

            openSession.Store<FoodMenu>(newFoodMenus);
            openSession.SaveChanges();

            return newFoodMenus;
        }

        public FoodMenu UpsertFoodMenu(Guid id, IFoodMenuPayload payload)
        {
            using var openSession = _store.OpenSession();

            var toBeUpsertedFoodMenu = new FoodMenu
            {
                Id = id,
                CookingTime = payload.CookingTime,
                IsDifficult = payload.IsDifficult,
                Name = payload.Name
            };

            openSession.Store(toBeUpsertedFoodMenu);
            openSession.SaveChanges();

            return toBeUpsertedFoodMenu;
        }

        public void DeleteFoodMenu(Guid id)
        {
            using var openSession = _store.OpenSession();
            openSession.DeleteWhere<FoodMenu>(x => x.Id == id);

            openSession.SaveChanges();
        }
    }


}
