using _2C2P_test.BLL.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Interfaces
{
    public interface IService<TDto, TFilter> 
        where TFilter : IFilter
    {
        TDto Get(string id);
        IEnumerable<TDto> Get(TFilter filter);
        IEnumerable<TDto> Get(Func<TDto, bool> predicate);
        IEnumerable<TDto> GetAll();
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
