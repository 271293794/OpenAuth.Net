// ***********************************************************************
// Assembly         : Helper
// Author           : yubaolee
// Created          : 12-16-2016
//
// Last Modified By : yubaolee
// Last Modified On : 12-21-2016
// 使用微软默认带超时的Cache
// File: CacheContext.cs
// ***********************************************************************

using System;
using System.Web;

namespace Infrastructure.Cache
{
    /// <summary>
    /// cache是服务端的全局缓存，它是和具体用户无关，只和应用程序相关。
    /// session是和用户相关，当用户关闭浏览器，当前会话session就会清除
    /// </summary>
    public class CacheContext : ICacheContext
    {
        private readonly System.Web.Caching.Cache _objCache = HttpRuntime.Cache;
        public override T Get<T>(string key)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return (T) objCache[key];
        }

        public override bool Set<T>(string key, T t, DateTime expire)
        {
            var obj = Get<T>(key);
            if (obj != null)
            {
                Remove(key);
            }
            // NoAbsoluteExpiration 与NoSlidingExpiration分别为绝对过期和相对过期 

            _objCache.Insert(key, t, null, expire, System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }

        public override bool Remove(string key)
        {
            _objCache.Remove(key);
            return true;
        }
    }
}
