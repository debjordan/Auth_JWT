# APIs com Autenticação JWT em C#

Este projeto implementa duas APIs REST em C# usando ASP.NET Core (.NET 8) para aprendizado sobre autenticação JWT e comunicação entre serviços. A **API 1** é um serviço protegido que exige um token JWT válido, enquanto a **API 2** consome a API 1, gerando um token JWT e retornando os dados protegidos.

## Objetivo
- Demonstrar como configurar autenticação JWT em uma API.
- Mostrar como uma API pode consumir outra usando tokens JWT.
- Ensinar conceitos básicos de comunicação entre APIs com C#.

## Estrutura do Projeto

### API 1 (Serviço Protegido)
- **Endpoint**: `GET /protected`
- **Função**: Retorna uma mensagem JSON (`{"Message": "Acesso concedido! Dados protegidos aqui."}`) se um token JWT válido for fornecido no header `Authorization: Bearer <token>`.
- **Tecnologias**:
  - ASP.NET Core com `Microsoft.AspNetCore.Authentication.JwtBearer` para validação de tokens.
  - `System.IdentityModel.Tokens.Jwt` para manipulação de JWT.

### API 2 (Consumidor)
- **Endpoint**: `GET /consume`
- **Função**: Gera um token JWT, faz uma requisição à API 1 e retorna os dados recebidos ou um erro.
- **Tecnologias**:
  - `System.IdentityModel.Tokens.Jwt` para geração de tokens.
  - `HttpClient` para comunicação com a API 1.

## Pré-requisitos

- **.NET SDK** (versão 8 ou superior).
- Ferramentas para testar APIs (ex.: `curl`, Postman, ou navegador).
- Pacotes NuGet:
  - API 1: `Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`
  - API 2: `System.IdentityModel.Tokens.Jwt`

## Configuração

1. **Crie os projetos**:
   ```bash
   dotnet new webapi -n Api1
   cd Api1
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   dotnet add package System.IdentityModel.Tokens.Jwt
   cd ..
   dotnet new webapi -n Api2
   cd Api2
   dotnet add package System.IdentityModel.Tokens.Jwt
   ```

2. **Configure a API 1**:
   - Substitua `Api1/Program.cs` pelo código fornecido (contém configuração de autenticação JWT).
   - Use a chave secreta: `minha_chave_secreta_super_segura_1234567890` (mínimo 32 caracteres para HS256).

3. **Configure a API 2**:
   - Substitua `Api2/Program.cs` pelo código fornecido (contém geração de JWT e chamada à API 1).
   - Use a mesma chave secreta da API 1.

## Como Executar

1. **Inicie a API 1**:
   ```bash
   cd Api1
   dotnet run --urls=http://localhost:5000
   ```

2. **Inicie a API 2**:
   ```bash
   cd Api2
   dotnet run --urls=http://localhost:5001
   ```

3. **Teste a API 2**:
   ```bash
   curl http://localhost:5001/consume
   ```
   - **Saída esperada**:
     ```json
     {"DataFromApi1":{"Message":"Acesso concedido! Dados protegidos aqui."}}
     ```

4. **Teste a API 1 diretamente** (opcional):
   - `curl http://localhost:5000/protected` (sem token) → Erro 401 Unauthorized.
   - Use Postman com um token gerado pela API 2 para testar.

## Código dos Projetos

### API 1 (Program.cs)
- Configura autenticação JWT com validação de chave simétrica (HS256).
- Protege o endpoint `/protected` com `[Authorize]`.
- Usa chave secreta de 256 bits ou mais.

### API 2 (Program.cs)
- Gera um JWT com claim `user_id` e expiração de 15 minutos.
- Usa `HttpClient` para chamar a API 1 com o token no header `Authorization`.
- Retorna os dados da API 1 ou erro.

## Conceitos Aprendidos

- **JWT (JSON Web Token)**:
  - Geração de tokens com claims e expiração.
  - Validação de tokens com chave simétrica.
  - Uso do header `Authorization: Bearer <token>`.

- **ASP.NET Core**:
  - Configuração de autenticação com `JwtBearer`.
  - Criação de APIs REST com minimal APIs.
  - Injeção de dependências (`IHttpClientFactory`).

- **Comunicação entre APIs**:
  - Envio de requisições HTTP com `HttpClient`.
  - Tratamento de respostas (sucesso e erro).

## Solução de Problemas

- **Erro "key size must be greater than 256 bits"**:
  - Use uma chave secreta com pelo menos 32 caracteres (ex.: `minha_chave_secreta_super_segura_1234567890`).
  - Confirme que ambas as APIs usam a mesma chave.

- **Erro 401 em /protected**:
  - Verifique se o token está no formato correto (`Bearer <token>`).
  - Teste com Postman para depurar.

- **API não responde**:
  - Confirme que as portas 5000 (API 1) e 5001 (API 2) estão livres.
  - Verifique se os projetos estão rodando corretamente (`dotnet run`).
