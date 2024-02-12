﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		private readonly DbSet<T> _dbSet;
		public Repository(ApplicationDbContext db)
		{
			_db = db;
			_dbSet = _db.Set<T>();
		}
		public void Add(T entity)
		{
			_db.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> filter, string? include = null)
		{
			IQueryable<T> query = _dbSet;

			query = query.Where(filter);

			if (!string.IsNullOrEmpty(include))
			{
				foreach (var property in include.Split(",", StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? include = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (!string.IsNullOrEmpty(include))
			{
				foreach (var property in include.Split(",", StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.ToList();
		}

		public void Delete(T entity)
		{
			_db.Remove(entity);
		}
	}
}
