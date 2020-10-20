using _2C2P_test.BLL.Interfaces;
using _2C2P_test.DAL.Repository;
using _2C2P_test.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Implementations
{
    public abstract class Service<TEntity, TDto> : IService<TDto>
    where TEntity : class
    where TDto : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        protected IRepository<TEntity> Repository;


        protected abstract TDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TDto dto);


        public abstract TDto Get(string id);
        public abstract IEnumerable<TDto> Get(Func<TDto, bool> predicate);
        public abstract IEnumerable<TDto> GetAll();
        public abstract void Add(TDto dto);
        public abstract void Remove(string id);
        public abstract void Update(TDto dto);
    }
}
