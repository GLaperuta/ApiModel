# MyApi

Projeto de estudo focado em **arquitetura limpa, qualidade de código e CI/CD** utilizando tecnologias modernas do ecossistema .NET.

A aplicação foi desenvolvida com foco em:

- Clean Architecture
- DDD (Domain Driven Design)
- CQRS com MediatR
- Testes automatizados
- Code Quality com SonarQube
- CI/CD com Jenkins
- Containers com Docker
- Orquestração com Kubernetes

---

# Arquitetura do Projeto


ApiModel
│
├── Jenkinsfile
├── MyApi.slnx
│
├── src
│ ├── MyApi.Api
│ ├── MyApi.Application
│ ├── MyApi.Domain
│ └── MyApi.Infrastructure
│
├── tests
│ ├── MyApi.UnitTests
│ ├── MyApi.FunctionalTests
│ └── MyApi.E2ETests
│
├── docker
│ ├── api
│ │ └── Dockerfile
│ └── jenkins
│ └── Dockerfile
│
├── k8s
│ ├── deployment.yaml
│ └── service.yaml

---

# Tecnologias Utilizadas

- .NET 10
- ASP.NET Core
- MediatR
- xUnit
- Docker
- Jenkins
- SonarQube
- PostgreSQL
- Redis
- Kubernetes

---

# Rodando o Projeto

## 1️⃣ Restaurar dependências

dotnet restore MyApi.slnx

## 2️⃣ Compilar o projeto

dotnet build MyApi.slnx

## 3️⃣ Rodar os testes

dotnet test MyApi.slnx

Code Coverage

Para gerar cobertura de testes:

rodar no powershell
# 1. Início do Scanner 
# Apontamos para todos os arquivos .xml dentro da pasta central TestResults
dotnet-sonarscanner begin /k:"myapi" `
  /d:sonar.host.url="http://localhost:9000" `
  /d:sonar.login="seu_token" `
  /d:sonar.cs.opencover.reportsPaths="TestResults/*.xml" `
  /d:sonar.coverage.exclusions="**/Migrations/**,**/Program.cs"

# 2. Build da Solução
dotnet build MyApi.slnx

# 3. Execução dos Testes com saída centralizada
# Usamos caminhos relativos para subir dois níveis e chegar na pasta TestResults da raiz
dotnet test tests/MyApi.UnitTests/MyApi.UnitTests.csproj --no-build `
  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="../../TestResults/unit.xml"

dotnet test tests/MyApi.FunctionalTests/MyApi.FunctionalTests.csproj --no-build `
  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="../../TestResults/functional.xml"

dotnet test tests/MyApi.E2ETests/MyApi.E2ETests.csproj --no-build `
  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="../../TestResults/e2e.xml"

# 4. Finalização e Envio
dotnet-sonarscanner end /d:sonar.login="seu_token"


Rodando SonarQube
Subir o SonarQube com Docker
docker run -d \
-p 9000:9000 \
--name sonarqube \
sonarqube:lts-community

Acesse:

http://localhost:9000

Login padrão:

admin
admin

# Rodando Jenkins

Build da imagem customizada com .NET

docker build -t jenkins-dotnet -f docker/jenkins/Dockerfile .
Rodar Jenkins
docker run -d \
--name jenkins \
-p 8080:8080 \
-p 50000:50000 \
-v jenkins_home:/var/jenkins_home \
jenkins-dotnet

Acessar Jenkins:

http://localhost:8080


Pipeline CI/CD

O pipeline automatiza:

1️⃣ Restore do projeto
2️⃣ Build
3️⃣ Execução de testes
4️⃣ Análise de qualidade com SonarQube
5️⃣ Build da imagem Docker
6️⃣ Deploy no Kubernetes

Definido no arquivo:

Jenkinsfile

Build Docker da API
docker build -t myapi -f docker/api/Dockerfile .

Rodar container:

docker run -p 5000:8080 myapi
Deploy Kubernetes
kubectl apply -f k8s/
Qualidade de Código

O projeto utiliza SonarQube para monitoramento de:

Code Smells

Vulnerabilidades

Coverage

Duplicação de código

CI/CD

O pipeline CI/CD é executado automaticamente pelo Jenkins.

Etapas:

Restore
Build
Test
SonarQube
Docker Build
Deploy Kubernetes
Autor

Desenvolvido por Guilherme Laperuta