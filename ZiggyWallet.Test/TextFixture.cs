    using ZiggyZiggyWallet.Models;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    namespace ZiggyWallet.Test
    {
        public class TestFixture<TStartup> : IDisposable where TStartup : class
        {
            public readonly TestServer Server;
            private readonly HttpClient _client;
            public TestFixture()
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings.json");
                var builder = new WebHostBuilder()
                    .UseEnvironment("Development")
                    .UseStartup<TStartup>()
                    .ConfigureAppConfiguration((context, conf) =>
                    {
                        conf.AddJsonFile(configPath);
                    });
                Server = new TestServer(builder);
                _client = new HttpClient();
            }
            public void Dispose()
            {
                _client.Dispose();
                Server.Dispose();
            }
        }
    }
