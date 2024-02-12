using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Interfaces
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll( Expression<Func<T, bool>>? filter = null, string? include = null);
		T Get(Expression<Func<T, bool>> filter,  string? include = null);
		void Add(T entity);
		void Delete(T entity);
	}
}
