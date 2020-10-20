using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Interfaces
{
    public interface IService<TDto>
    {
        TDto Get(string id);
        IEnumerable<TDto> Get(Func<TDto, bool> predicate);
        IEnumerable<TDto> GetAll();
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
