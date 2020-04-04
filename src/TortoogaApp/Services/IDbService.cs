using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TortoogaApp.Data;
using TortoogaApp.Models;

namespace TortoogaApp.Services
{
    public interface IDbService
    {
        /// <summary>
        /// Retrieves all the entity
        /// </summary>
        /// <typeparam name="TModel">Model class that implements IEntity</typeparam>
        /// <param name="expression">where clause</param>
        /// <param name="navigationProperties">Include properties</param>
        /// <returns>TModel entities</returns>
        IEnumerable<TModel> Get<TModel>(Expression<Func<TModel, bool>> expression, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : class, IEntity;

        /// <summary>
        /// Retrieves single entity
        /// </summary>
        /// <typeparam name="TModel">Model class that implements IEntity</typeparam>
        /// <param name="expression">Where clause</param>
        /// <param name="navigationProperties">Include properties</param>
        /// <returns>The TModel entity or default value of TModel</returns>
        TModel GetSingle<TModel>(Func<TModel, bool> expression, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : class, IEntity;

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        int Add<TModel>(params TModel[] entities) where TModel : class, IEntity;

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        int Update<TModel>(params TModel[] entities) where TModel : class, IEntity;

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        int Remove<TModel>(params TModel[] entities) where TModel : class, IEntity;
    }

    public class EFDbService : IDbService
    {
        private ApplicationDbContext _context;

        public EFDbService(ApplicationDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves all the entity
        /// </summary>
        /// <typeparam name="TModel">Model class that implements IEntity</typeparam>
        /// <param name="expression">where clause</param>
        /// <param name="navigationProperties">Include properties</param>
        /// <returns>TModel entities</returns>
        public IEnumerable<TModel> Get
            <TModel>(Expression<Func<TModel, bool>> expression, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : class, IEntity
        {
            IQueryable<TModel> query = _context.Set<TModel>();

            //Eager loading of child property
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.AsNoTracking();
        }

        /// <summary>
        /// Retrives single entity
        /// </summary>
        /// <typeparam name="TModel">Model class that implements IEntity</typeparam>
        /// <param name="expression">Where clause</param>
        /// <param name="navigationProperties">Include properties</param>
        /// <returns>The TModel entity or default value of TModel</returns>
        public TModel GetSingle<TModel>(Func<TModel, bool> expression, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : class, IEntity
        {
            TModel item = null;

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), "expression must not be null");
            }

            IQueryable<TModel> query = _context.Set<TModel>();

            //Eager loading of child property
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            item = query.AsNoTracking().FirstOrDefault(expression);

            return item;
        }

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        public int Add<TModel>(params TModel[] entities) where TModel : class, IEntity
        {
            foreach (var item in entities)
            {
                _context.Entry(item).State = EntityState.Added;
            }

            return _context.SaveChanges();
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        public int Update<TModel>(params TModel[] entities) where TModel : class, IEntity
        {
            foreach (var item in entities)
            {
                _context.Entry(item).State = EntityState.Modified;
            }

            return _context.SaveChanges();
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <typeparam name="TModel">The Model class that implements IEntity</typeparam>
        /// <param name="entities">The entities</param>
        /// <returns>the updated count row in db</returns>
        public int Remove<TModel>(params TModel[] entities) where TModel : class, IEntity
        {
            foreach (var item in entities)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }

            return _context.SaveChanges();
        }
    }
}