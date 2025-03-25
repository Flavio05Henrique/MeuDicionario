using MeuDicionarioV2;

Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("https://localhost:7167");
        })
        .Build()
        .Run();
