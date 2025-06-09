using DataContracts;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Utility;
using System.Linq;

namespace Localization
{
    public class LocalizationManager
    {
        private Localizations _DcLoc = new Localizations();

        private LookupCache _lookupCache = new LookupCache();
        private MemoryCache _cache = MemoryCache.Default;

        public List<Localizations> GetLocalization(DatabaseKey dbKey, String ResourceSet, String LocaleId)
        {
            _DcLoc.LocaleId = LocaleId;
            _DcLoc.ResourceSet = ResourceSet;
            var localizations = _lookupCache.GetOrAddCache<Localizations>(LookupCacheKeys.LOCALIZATION, () => _DcLoc.RetrieveByResourceSet(dbKey), _cache);

            return localizations.ToList();
        }
    }
}
