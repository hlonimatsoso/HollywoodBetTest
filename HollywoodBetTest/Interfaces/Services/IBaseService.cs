using HollywoodBetTest.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Interfaces.Services
{
    public interface IBaseService<T> : IBaseRepository<T> where T : class
    {
    }
}
