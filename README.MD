<img src="https://repository-images.githubusercontent.com/268701472/8bf84980-a6ce-11ea-83da-e2133c5a3a7a" alt=".NET DevPack" width="300px" />

What is the .NET DevPack.JwtExtensions?
=====================
.NET DevPack JwtExtensions was created to help you protect you API ASP.NET Core with JWT Bearer Token using a Custom JWKS Endpoint. Giving hability to leverage the security of your environment using Assymetric Keys. Which is by far a most recommended cryptography to digitally signin you JWT.

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/f1bd42eda59844ea95852606741147fa)](https://www.codacy.com/gh/NetDevPack/NetDevPack.Identity?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=NetDevPack/NetDevPack.JwtExtensions&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/e283g9ik4rk3ymsp?svg=true)](https://ci.appveyor.com/project/EduardoPires/netdevpack-identity)
![.NET Core](https://github.com/NetDevPack/NetDevPack.JwtExtensions/workflows/.NET%20Core/badge.svg)
[![License](http://img.shields.io/github/license/NetDevPack/NetDevPack.JwtExtensions.svg)](LICENSE)

## Give a Star! :star:
If you liked the project or if NetDevPack helped you, please give a star ;)

## Get Started

| Package |  Version | Popularity |
| ------- | ----- | ----- |
| `NetDevPack.JwtExtensions` | [![NuGet](https://img.shields.io/nuget/v/NetDevPack.JwtExtensions.svg)](https://nuget.org/packages/NetDevPack.JwtExtensions) | [![Nuget](https://img.shields.io/nuget/dt/NetDevPack.JwtExtensions.svg)](https://nuget.org/packages/NetDevPack.JwtExtensions) |


.NET DevPack.JwtExtensions can be installed in your ASP.NET Core application using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package NetDevPack.JwtExtensions
```

Then use Extension at configuration in `ConfigureServices` method of your `Startup.cs`:

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
   {
       options.SetJwksOptions(new JwkOptions("https://localhost:5001/jwks"));
   });
```

### Configuring JWT
If you want to generate JSON Web Tokens in your application you need to add the [NetDevPack.Jwk](https://github.com/NetDevPack/NetDevPack.Jwk).

>**Note:** NetDevPack.JwtExtensions is for those who already have an api who use `NetDevPack.Jwk`
>
>**Note:** The `NetDevPack.Jwk` is a set of components who will generate Keys using industry security best standards (NIST Rotating keys, RSA Key Length, ECDsa P-256). It's supports [Elliptic Curves](https://blog.cloudflare.com/a-relatively-easy-to-understand-primer-on-elliptic-curve-cryptography/) and RSA as well.

## Examples
Use the [sample application](https://github.com/NetDevPack/NetDevPack.Identity/tree/master/src/Samples/AspNetCore.Jwt.Sample) to understand how NetDevPack.Identity can be implemented and help you to decrease the complexity of your application and development time.

## Compatibility
The **NetDevPack.Identity** was developed to be implemented in **ASP.NET Core 3.1** `LTS` applications, in the next versions will be add the 2.1 `LTS` support.

## About
.NET DevPack.Identity was developed by [Eduardo Pires](http://eduardopires.net.br) under the [MIT license](LICENSE).