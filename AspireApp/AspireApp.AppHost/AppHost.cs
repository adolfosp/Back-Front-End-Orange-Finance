var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("mainpostgres")
    .WithPgAdmin();

var seq = builder.AddSeq("seq")
                 .ExcludeFromManifest()
                 .WithLifetime(ContainerLifetime.Persistent)
                 .WithEnvironment("ACCEPT_EULA", "Y");

var mainDB = postgres.AddDatabase("maindb");

var cache = builder.AddRedis("cache");

var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();


builder.AddProject<Projects.OrangeFinance>("api-orange")
    .WithReplicas(1)
    .WithReference(mainDB)
    .WaitFor(mainDB)
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);


builder.Build().Run();
