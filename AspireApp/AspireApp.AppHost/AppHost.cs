var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("mainpostgres")
    .WithPgAdmin();

var seq = builder.AddSeq("seq")
                 .ExcludeFromManifest()
                 .WithLifetime(ContainerLifetime.Persistent)
                 .WithEnvironment("ACCEPT_EULA", "Y");

var mainDB = postgres.AddDatabase("maindb");

builder.AddProject<Projects.OrangeFinance>("api-orange")
    .WithReplicas(3)
    .WithReference(mainDB)
    .WaitFor(mainDB)
    .WithReference(seq)
    .WaitFor(seq);

builder.Build().Run();
