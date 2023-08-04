using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.DtoMappings.Namene;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class NamenaManager : BaseManager<NamenaManager, Namena>, INamenaManager
    {
        public NamenaManager(ILogger<NamenaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<NamenaDto> GetMultiple()
        {
            return new ListResponse<NamenaDto>(Queryable().ToList().ToNamenaDtoList());
        }
    }
}
