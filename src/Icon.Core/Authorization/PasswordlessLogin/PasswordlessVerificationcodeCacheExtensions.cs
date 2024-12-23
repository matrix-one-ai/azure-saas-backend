using Abp.Runtime.Caching;

namespace Icon.Authorization.PasswordlessLogin
{
    public static class PasswordlessVerificationcodeCacheExtensions
    {
        public static ITypedCache<string, PasswordlessLoginCodeCacheItem> GetPasswordlessVerificationCodeCache(
            this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, PasswordlessLoginCodeCacheItem>(PasswordlessLoginCodeCacheItem
                .CacheName);
        }
    }
}