using CarListingApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICarRepository Cars { get; }
        IRepository<ProcessedEvent> ProcessedEvents { get; }
        Task<int> CompleteAsync();
    }
}
