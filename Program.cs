using DotNetEnv;

// Load environment variables from .env file
Env.Load();

Cli cli = new(args);
await cli.SetupDay();
cli.Solve();