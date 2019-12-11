using MyAPI.Services.Entities;
using System;
using System.Collections.Generic;

namespace MyAPI.Services
{
    public interface IMyService
    {
        public bool IsSet();

        public IEnumerable<MyEntity> Get();
        public MyEntity Get(Guid id);
        public MyEntity Create(string name);
        public MyEntity Update(Guid id, string name);
        public void Delete(Guid id);
    }
}