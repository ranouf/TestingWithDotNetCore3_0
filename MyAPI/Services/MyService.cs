using MyAPI.EntityFramework;
using MyAPI.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAPI.Services
{
    public class MyService: IMyService
    {
        private readonly MyDbContext _dbContext;

        public MyService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MyEntity Create(string name)
        {
            var result = _dbContext.MyEntities.Add(new MyEntity() { Name = name });
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public void Delete(Guid id)
        {
            var myEntity = _dbContext.MyEntities.FirstOrDefault(e => e.Id == id);
            if (myEntity == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            _dbContext.MyEntities.Remove(myEntity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<MyEntity> Get()
        {
            return _dbContext.MyEntities.ToList();
        }

        public MyEntity Get(Guid id)
        {
            return _dbContext.MyEntities.FirstOrDefault(e => e.Id == id);
        }

        public bool IsSet()
        {
            return true;
        }

        public MyEntity Update(Guid id, string name)
        {
            var myEntity = _dbContext.MyEntities.FirstOrDefault(e => e.Id == id);
            if (myEntity == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            myEntity.Name = name;
            var result = _dbContext.MyEntities.Update(myEntity);
            _dbContext.SaveChanges();
            return result.Entity;
        }
    }
}
