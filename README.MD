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
If you want to generate JSON Web Tokens in your application you need to add the JWT configuration in `ConfigureServices` method of your `startup.cs`
```csharp
services.AddJwtConfiguration(Configuration, "AppSettings");
```

>**Note:** If you don't inform the configuration name the value adopted will be _AppJwtSettings_


Set your `appsettings.json` file with this values:

```json
"AppSettings": {
    "SecretKey": "MYSECRETSUPERSECRET",
    "Expiration": 2,
    "Issuer": "SampleApp",
    "Audience": "https://localhost"
}
``` 

|Key|Meaning|
|--|--|
|SecretKey  | Is your key to build JWT. This value need to be stored in a safe place in the production way |
|Expiration| Expiration time in hours  |
|Issuer| The name of the JWT issuer  |
|Audience| The domain that the JWT will be valid. Can be a string collection  |

### Generating JWT
You will need to set some dependencies in your Authentication Controller:

```csharp
private readonly SignInManager<IdentityUser> _signInManager;
private readonly UserManager<IdentityUser> _userManager;
private readonly AppJwtSettings _appJwtSettings;

public AuthController(SignInManager<IdentityUser> signInManager,
		      UserManager<IdentityUser> userManager,
		      IOptions<AppJwtSettings> appJwtSettings)
{
    _signInManager = signInManager;
    _userManager = userManager;
    _appJwtSettings = appJwtSettings.Value;
}
```

>**Note:** The _AppJwtSettings_ is our dependency and is configured internally during JWT setup (in `startup.cs` file). You just need to inject it in your controller.
>
>**Note:** The _SignInManager_ and _UserManager_ classes is native from Identity and provided in NetDevPack.Identity. You just need to inject it in your controller.

After user register or login process you can generate a JWT to respond the request. Use our implementation, you just need inform the user email and the dependencies injected in your controller:

```csharp
return new JwtBuilder()
	.WithUserManager(_userManager)
	.WithJwtSettings(_appJwtSettings)
	.WithEmail(email)
	.BuildToken();
```

>**Note:** This builder can return a single string with JWT or a complex object `UserResponse` if you want return more data than a single JWT string.

#### Adding Claims to your JWT
You can call more methods in `JwtBuilder` to provide more information about the user:

```csharp
return new JwtBuilder()
    .WithUserManager(_userManager)
    .WithJwtSettings(_appJwtSettings)
    .WithEmail(email)
    .WithJwtClaims()
    .WithUserClaims()
    .WithUserRoles()
    .BuildToken();
```

|Method|Meaning|
|--|--|
|WithJwtClaims()| Claims of JWT like `sub`, `jti`, `nbf` and others |
|WithUserClaims()| The user claims registered in `AspNetUserClaims` table|
|WithUserRoles()| The user roles (as claims) registered in `AspNetUserRoles` table  |
|BuildToken()| Build and return the JWT as single string  |

If you want return your complex object `UserResponse` you need to change the last method to:

```csharp
return new JwtBuilder()
    .WithUserManager(_userManager)
    .WithJwtSettings(_appJwtSettings)
    .WithEmail(email)
    .WithJwtClaims()
    .WithUserClaims()
    .WithUserRoles()
    .BuildUserResponse() as UserResponse;
```

>**Note:** The safe cast to `UserResponse` is needed because is a subtype of `UserResponse<TKey>`.

## Examples
Use the [sample application](https://github.com/NetDevPack/NetDevPack.Identity/tree/master/src/Samples/AspNetCore.Jwt.Sample) to understand how NetDevPack.Identity can be implemented and help you to decrease the complexity of your application and development time.

## Compatibility
The **NetDevPack.Identity** was developed to be implemented in **ASP.NET Core 3.1** `LTS` applications, in the next versions will be add the 2.1 `LTS` support.

## About
.NET DevPack.Identity was developed by [Eduardo Pires](http://eduardopires.net.br) under the [MIT license](LICENSE).