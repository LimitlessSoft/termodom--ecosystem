using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UslovFormiranjaWebCeneRepository(OfficeDbContext dbContext)
    : LSCoreRepositoryBase<UslovFormiranjaWebCeneEntity>(dbContext), IUslovFormiranjaWebCeneRepository;
