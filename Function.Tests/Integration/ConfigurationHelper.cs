﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function.Tests.Integration
{
    public static class ConfigurationHelper
    {
        private static Settings _settings;

        public static Settings Settings
        {
            get
            {
                if (_settings != null)
                {
                    return _settings;
                }

                var configurationRoot = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
                _settings = new Settings();
                configurationRoot.Bind(_settings);

                return _settings;
            }
        }
    }
}
