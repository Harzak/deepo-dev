﻿using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Client.App.Fragments.Factory
{
    public class FragmentFactory : IFragmentFactory
    {
        private readonly ILogger _logger;
        private readonly IHttpService _httpService;

        public FragmentFactory(ILogger logger, IHttpService httpService)
        {
            _logger = logger;
            _httpService = httpService;
        }

        public NewReleasesFragment CreateNewReleasesFragment()
            => new NewReleasesFragment(_httpService);

    }
}
