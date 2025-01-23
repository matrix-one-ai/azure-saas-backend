using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Icon.Matrix.Models;


namespace Icon.Matrix
{
    public interface IPlatformManager : IDomainService
    {
        Task<Guid> GetPlatformId(string name);
    }
    public class PlatformManager : IconServiceBase, IPlatformManager
    {
        private readonly IRepository<Platform, Guid> _platformRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PlatformManager(IRepository<Platform, Guid> platformRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _platformRepository = platformRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<Guid> GetPlatformId(string name)
        {
            var platform = await GetPlatform(name);
            return platform.Id;
        }

        private async Task<Platform> GetPlatform(string name)
        {

            var platform = await _platformRepository.FirstOrDefaultAsync(p => p.Name == name);

            if (platform == null)
            {
                platform = await CreatePlatform(name);
            }

            return platform;
        }

        private async Task<Platform> CreatePlatform(string name)
        {
            var platform = new Platform
            {
                Name = name
            };

            using (var uow = _unitOfWorkManager.Begin())
            {
                platform = await _platformRepository.InsertAsync(platform);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
            return platform;
        }

    }
}