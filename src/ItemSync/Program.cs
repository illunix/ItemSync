using ItemSync.Infrastrtucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WDBXLib.Definitions.WotLK;
using WDBXLib.Reader;
using WDBXLib.Storage;

namespace ItemSync
{
    class Program
    {
        private static IConfigurationRoot _configuration;

        static async Task Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetService<ApplicationDbContext>();

            var itemTemplate = await context.ItemTemplate
                .Select(q => new Item { 
                    ID = q.Entry, 
                    ClassID = q.Class,
                    SubclassID = q.SubClass, 
                    Sound_override_subclassid = q.SoundOverrideSubClass, 
                    Material = q.Material ,
                    DisplayInfoID = q.DisplayId,
                    InventoryType = q.InventoryType,
                    SheatheType = q.Sheath
                })
                .ToListAsync();

            var itemDbcFilePath = Path.Combine(
                _configuration["DBCFilesPath"], 
                "Item.dbc"
            );

            if (!File.Exists(itemDbcFilePath))
            {
                return;
            }

            var itemDbc = DBReader.Read<Item>(itemDbcFilePath);

            itemDbc.Rows.Clear();
            
            foreach (var item in itemTemplate)
            {
                itemDbc.Rows
                    .Add(item);
            }

            DBReader.Write(
                itemDbc, 
                itemDbcFilePath
            );
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddSingleton(_configuration);

            services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(_configuration["ConnectionString"]));
        }
    }
}
