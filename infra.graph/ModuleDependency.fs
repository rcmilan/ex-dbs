namespace infra.graph

    open System
    open Microsoft.Extensions.DependencyInjection
    open Neo4jClient
    
    [<System.Runtime.CompilerServices.Extension>]
    module ModuleDependency =

        type Connection = { ConnectionString : string; User : string; Password : string }

        [<System.Runtime.CompilerServices.Extension>]
        let AddGraphModule (services : IServiceCollection) (conn : Connection) =

            let client = new GraphClient(new Uri(conn.ConnectionString), conn.User, conn.Password);

            client.ConnectAsync().Wait()

            services.AddSingleton<ICypherGraphClient>(client)