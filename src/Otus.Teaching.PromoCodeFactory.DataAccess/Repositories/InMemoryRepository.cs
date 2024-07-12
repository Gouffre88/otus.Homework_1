using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> CreateAsync(T item)
        {
            Data = Data.Append(item);
            return Task.FromResult(item);
        }

        public Task<T> UpdateAsync(T model)
        {
            var emp = Data.FirstOrDefault(x=>x.Id == model.Id);
            if (emp!=null)
            {
                emp = model;
            }
            return Task.FromResult(emp);
        }

        public Task DeleteAsync(T item)
        {
            Data = Data.Where(x => x.Id != item.Id);
            return Task.CompletedTask;
        } 

    }

}