# SmallTodoApi

A small ASP.NET Core 8 REST API with a GitHub Actions CI/CD pipeline.

## Local run

```bash
dotnet restore
dotnet run --project SmallTodoApi.csproj
```

API: `http://localhost:5000/api/todos`

## GitHub Actions CI/CD

Workflow:

`.github/workflows/ci-cd.yml`

### Pull requests to main

The CI pipeline performs:

1. Restore
2. Build
3. `dotnet format` verification
4. Vulnerable NuGet package check

**No unit tests or test cases are executed by this pipeline.**

### Pushes to main

The same CI checks run first. If they pass, the Docker job:

1. Logs into GitHub Container Registry (GHCR)
2. Builds the Docker image
3. Pushes it to GHCR
4. Creates a `latest` tag and a commit-SHA tag

No cloud deployment is configured yet.

## GitHub setup

1. Create a GitHub repository.
2. Push this project.
3. Ensure the repository uses the `main` branch.
4. GitHub Actions uses the built-in `GITHUB_TOKEN` for GHCR authentication.
5. After a successful `main` push, check the repository's **Packages** section for the Docker image.

## Pipeline

```text
Pull Request
    |
    v
Restore -> Build -> Format Check -> Vulnerability Check
                                      |
                                      v
                                 PR status

main push
    |
    v
CI checks
    |
    v
Docker Build
    |
    v
GitHub Container Registry
```

## Next steps

For a production-style pipeline, you can later add:

- CodeQL
- Dependabot
- Unit/integration tests
- Docker image vulnerability scanning
- Azure Container Registry
- Azure App Service / Container Apps / AKS deployment
- Staging and production environments
- Deployment approvals
- OpenID Connect (OIDC) from GitHub to Azure
