﻿using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Data.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseApp.Data.DataRepository.Users.Impl
{
    internal static class UserExtensions
    {
        public static IQueryable<User> IncludeRoles(this IQueryable<User> users)
        {
            return users.Include(x => x.UserRoles).ThenInclude(x => x.Role);
        }

        public static IQueryable<AccountProjection> SelectAccountProjection(this IQueryable<User> users)
        {
            return users.Select(m => new AccountProjection
            {
                Id = m.Id,
                Login = m.Login,
                Password = m.Password,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                Roles = m.UserRoles.Select(t => t.Role.Name)
            });
        }
    }

    public class UserRepository : RepositoryEntityDeletableBase<User>, IUserRepository
    {
        public IRoleRepository Roles => GetRepository<RoleRepository>();

        public UserRepository(DataContextProvider context)
            : base(context)
        {
        }

        public List<User> GetUsersForAdmin(string search, PagingSortingInfo pagingSorting)
        {
            var query = EntitySet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.FullName.Contains(search));
            }
            return query.IncludeRoles().PagingSorting(pagingSorting).ToList();
        }

        public List<User> GetUsersByFilter(string prefix, int count)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = null;
            return EntitySet
                .Where(m => (prefix == null || m.FirstName.StartsWith(prefix) || m.LastName.StartsWith(prefix)))
                .Take(count)
                .ToList();
        }

        public User GetWithRolesOrNull(int id)
        {
            return EntitySet.IncludeRoles()
                .FirstOrDefault(m => m.Id == id);
        }

        public User GetByEmailOrNull(string email, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Email == email);
        }

        public User GetByLoginOrNull(string login, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Login == login);
        }

        public AccountProjection GetAccountByLoginOrNull(string login)
        {
            return EntitySet.Where(m => m.Login == login)
                .SelectAccountProjection().FirstOrDefault();
        }

        public AccountProjection GetAccountByIdOrNull(int id)
        {
            return EntitySet.Where(m => m.Id == id)
                .SelectAccountProjection().FirstOrDefault();
        }

        private IQueryable<User> GetUserView(bool includeDeleted = false)
        {
            var q = EntitySet.AsQueryable();
            if (includeDeleted)
            {
                q = q.IgnoreQueryFilters();
            }
            return q;
        }

        public List<User> GetDeleted()
        {
            return EntitySet.IgnoreQueryFilters().Where(x => x.DeletedDate != null).ToList();
        }

        public override User CreateEmpty()
        {
            var item = base.CreateEmpty();
            item.CreatedDate = DateTime.Now;
            return item;
        }
    }
}
