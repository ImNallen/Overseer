IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres =
    builder.AddPostgres("postgres")
    .WithPgAdmin();

IResourceBuilder<PostgresDatabaseResource> postgresdb = postgres.AddDatabase("overseer");

IResourceBuilder<SeqResource> seq = builder.AddSeq("seq");

builder.AddProject<Projects.Overseer_Api>("api")
    .WithReference(postgresdb)
    .WithReference(seq)
    .WaitFor(postgresdb)
    .WaitFor(seq);

await builder.Build().RunAsync();
