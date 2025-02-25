---
sidebar_position: 800
title: 80 - AutoConstructor
description: Generating constructor for class with many properties
slug: /AutoConstructor
---
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';
import TOCInline from '@theme/TOCInline';

# AutoConstructor  by Kévin Gallienne


<TOCInline toc={toc}  />

## Nuget / site data
[![Nuget](https://img.shields.io/nuget/dt/AutoConstructor?label=AutoConstructor)](https://www.nuget.org/packages/AutoConstructor/)
[![GitHub last commit](https://img.shields.io/github/last-commit/k94ll13nn3/AutoConstructor?label=updated)](https://github.com/k94ll13nn3/AutoConstructor)
![GitHub Repo stars](https://img.shields.io/github/stars/k94ll13nn3/AutoConstructor?style=social)

## Details

### Info
:::info

Name: **AutoConstructor**

Source generator for automatic constructor generation.

Author: Kévin Gallienne

NuGet: 
*https://www.nuget.org/packages/AutoConstructor/*   


You can find more details at https://github.com/k94ll13nn3/AutoConstructor

Source : https://github.com/k94ll13nn3/AutoConstructor

:::

### Original Readme
:::note

# AutoConstructor

[![NuGet](https://img.shields.io/nuget/vpre/AutoConstructor?logo=nuget
)](https://www.nuget.org/packages/AutoConstructor/)
[![GitHub release](https://img.shields.io/github/release/k94ll13nn3/AutoConstructor.svg?logo=github)](https://github.com/k94ll13nn3/AutoConstructor/releases/latest)
[![GitHub license](https://img.shields.io/github/license/k94ll13nn3/AutoConstructor
)](https://raw.githubusercontent.com/k94ll13nn3/AutoConstructor/main/LICENSE)
![ci.yml](https://github.com/k94ll13nn3/AutoConstructor/workflows/.github/workflows/ci.yml/badge.svg)

C# source generator that generates a constructor from readonly fields/properties in a class.

## Installation

- Grab the latest package on [NuGet](https://www.nuget.org/packages/AutoConstructor/).

## Requirements

| Version | Visual Studio | .NET SDK |
|---------|---------------|----------|
| <=1.3.0 | 16.10+        | 5.0.300+ |
| >=2.0.0 | 17.0+         | 6.0.100+ |
| >=5.0.0 | 17.6+         | 7.0.302+ |

## Basic usage

The following code:

```csharp
[AutoConstructor]
public partial class MyClass
{
    private readonly MyDbContext _context;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IService _service;

    [AutoConstructorInject("options?.Value", "options", typeof(IOptions<ApplicationOptions>))]
    private readonly ApplicationOptions _options;
}
```

will generate:

```csharp
partial class MyClass
{
    public MyClass(
        MyApp.MyDbContext context,
        System.Net.Http.IHttpClientFactory clientFactory,
        MyApp.IService service,
        Microsoft.Extensions.Options.IOptions<MyApp.ApplicationOptions> options)
    {
        this._context = context;
        this._clientFactory = clientFactory;
        this._service = service;
        this._options = options?.Value;
    }
}
```

A sample containing more cases is available at the end of this README.

## How to use

For any class where the generator will be used:

- Mark the class as `partial`
- Use `AutoConstructorAttribute` on the class

By default, all `readonly` non-`static` fields without initialization will be used. They will be injected with the same name without any leading `_`.

Fields marked with `AutoConstructorIgnoreAttribute` will be ignored.

Use `AutoConstructorInjectAttribute` to customize the behavior, usually when the injected parameter and the fields
do not have the same type. It takes three optional parameters:

- `initializer`: a string that will be used to initialize the field (by example `myService.GetData()`), default to the `parameterName` if null or empty.
- `parameterName`: the name of the parameter to used in the constructor  (by example `myService`), default to the field name trimmed if null or empty.
- `injectedType`: the type of the parameter to used in the constructor  (by example `IMyService`), default to the field type if null.

If no parameters are provided, the behavior will be the same as without the attribute. Using the attribute on a field that would not be injected otherwise
won't make the field injectable.

When using `AutoConstructorInjectAttribute`, the parameter name can be shared across multiple fields,
and even use a parameter from another field not annotated with `AutoConstructorInjectAttribute`, but type must match.

### Constructor accessibility

Constructor accessibility can be changed using the optionnal parameter `accessibility` on `AutoConstructorAttribute` (like `[AutoConstructor("internal")]`).
The default is `public` and it can be set to one of the following values:
- `public`
- `private`
- `protected`
- `internal`
- `protected internal`
- `private protected`

### Initializer method

It is possible to add a method call at the end of the constructor. To do this, the attribute `AutoConstructorInitializer` can be added to
a parameterless method that returns void. This will generate a call to the method at the end.

```csharp
[AutoConstructor]
internal partial class Test
{
    private readonly int _t;

    [AutoConstructorInitializer]
    public void Initializer()
    {
    }
}
```

will generate

```csharp
public Test(int t)
{
    this._t = t;

    this.Initializer();
}
```

### Properties injection

Get-only properties (`public int Property { get; }`) are injected by the generator by default.
Non get-only properties (`public int Property { get; set;}`) are injected only if marked with (`[field: AutoConstructorInject]`) attributte.
The behavior of the injection can be modified using [auto-implemented property field-targeted attributes](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-7.3/auto-prop-field-attrs) on its backing field. The following code show an injected get-only property with a custom injecter:

```csharp
[field: AutoConstructorInject(initializer: "injected.ToString()", injectedType: typeof(int), parameterName: "injected")]
public int Property { get; }
```

⚠️ The compiler support for auto-implemented property field-targeted attributes is not perfect (as described in the link above), and Roslyn analyzers are not running on backings fields so some warnings may not be reported.

## Configuration

### Generating `ArgumentNullException`

By default, null checks with `ArgumentNullException` are not generated when needed.
To enable this behavior, set `AutoConstructor_DisableNullChecking` to `false` in the project file:

``` xml
<AutoConstructor_DisableNullChecking>false</AutoConstructor_DisableNullChecking>
```

### Generating XML documentation comment

By default, no XML documentation comment will be generated for the constructor.
To enable this behavior, set `AutoConstructor_GenerateConstructorDocumentation` to `true` in the project file:

``` xml
<AutoConstructor_GenerateConstructorDocumentation>true</AutoConstructor_GenerateConstructorDocumentation>
```

This will generate a default comment like this one, with each parameter reusing the corresponding field summary if available, and the parameter name otherwise:

``` c#
/// <summary>
/// Initializes a new instance of the Test class.
/// </summary>
/// <param name=""t1"">Some field.</param>
/// <param name=""t2"">t2</param>
```

By using the `AutoConstructor_ConstructorDocumentationComment` property, you can configure the comment message:

``` xml
<AutoConstructor_ConstructorDocumentationComment>Some comment for the {0} class.</AutoConstructor_ConstructorDocumentationComment>
```

This will generate the following code:

``` c#
/// <summary>
/// Some comment for the Test class.
/// </summary>
/// <param name=""t1"">Some field.</param>
/// <param name=""t2"">t2</param>
```

## Samples describing some cases

### Sample for fields

The following code

``` csharp
[AutoConstructor]
partial class Test
{
    private readonly string _name;

    // Won't be injected
    private readonly Uri _uri = new Uri("/non-modified", UriKind.Relative);

    // Won't be injected
    [AutoConstructorIgnore]
    private readonly DateTime _dateNotTaken;

    // Won't be injected because not readonly. Attribute would be taken into account if this were a property, not a field.
    [AutoConstructorInject]
    private int  _stuff;

    // Won't be injected
    private int? _toto;

    // Support for nullables
    private readonly DateTime? _date;

    // Support for generics
    private readonly List<DateTime> _items;

    // Inject with custom initializer
    [AutoConstructorInject("guid.ToString()", "guid", typeof(Guid))]
    private readonly string _guidString;

    // Use existing parameter defined with AutoConstructorInject
    [AutoConstructorInject("guid.ToString().Length", "guid", typeof(Guid))]
    private readonly int _guidLength;

    // Use existing parameter from a basic injection
    [AutoConstructorInject("name.ToUpper()", "name", typeof(string))]
    private readonly string _nameShared;
}
```

will generate

```csharp
public Test(string name, System.DateTime? date, System.Collections.Generic.List<System.DateTime> items, System.Guid guid)
{
    this._name = name ?? throw new System.ArgumentNullException(nameof(name));
    this._date = date ?? throw new System.ArgumentNullException(nameof(date));
    this._items = items ?? throw new System.ArgumentNullException(nameof(items));
    this._guidString = guid.ToString() ?? throw new System.ArgumentNullException(nameof(guid));
    this._guidLength = guid.ToString().Length;
    this._nameShared = name.ToUpper() ?? throw new System.ArgumentNullException(nameof(name));
}
```

### Sample for get-only properties

The following code

``` csharp
[AutoConstructor]
public partial class Test
{
    [field: AutoConstructorInject]
    public int Injected { get; }

    public int AlsoInjectedEvenWhenMissingAttribute { get; }

    /// <summary>
    /// Some property.
    /// </summary>
    [field: AutoConstructorInject]
    public int InjectedWithDocumentation { get; }

    [field: AutoConstructorInject]
    public int InjectedBecauseExplicitInjection { get; set; }

    [field: AutoConstructorInject]
    public static int NotInjectedBecauseStatic { get; }

    [field: AutoConstructorInject]
    public int NotInjectedBecauseInitialized { get; } = 2;

    [field: AutoConstructorIgnore]
    public int NotInjectedBecauseHasIgnoreAttribute { get; }

    [field: AutoConstructorInject(initializer: ""injected.ToString()"", injectedType: typeof(int), parameterName: ""injected"")]
    public string InjectedWithoutCreatingAParam { get; }
}
```

will generate

```csharp
 partial class Test
    {
        /// <summary>
        /// Initializes a new instance of the Test class.
        /// </summary>
        /// <param name=""injected"">injected</param>
        /// <param name=""injectedWithDocumentation"">Some property.</param>
        /// <param name=""injectedBecauseExplicitInjection"">injectedBecauseExplicitInjection</param>
        /// <param name=""alsoInjectedEvenWhenMissingAttribute"">alsoInjectedEvenWhenMissingAttribute</param>
        public Test(int injected, int injectedWithDocumentation, int injectedBecauseExplicitInjection, int alsoInjectedEvenWhenMissingAttribute)
        {
            this.Injected = injected;
            this.InjectedWithDocumentation = injectedWithDocumentation;
            this.InjectedBecauseExplicitInjection = injectedBecauseExplicitInjection;
            this.AlsoInjectedEvenWhenMissingAttribute = alsoInjectedEvenWhenMissingAttribute;
            this.InjectedWithoutCreatingAParam = injected.ToString() ?? throw new System.ArgumentNullException(nameof(injected));
        }
    }
```

## Diagnostics

### ACONS01

The `AutoConstructor` attribute is used on a class that is not partial.

### ACONS02

The `AutoConstructor` attribute is used on a class without fields to inject.

### ACONS03

The `AutoConstructorIgnore` attribute is used on a field that won't already be processed.

### ACONS04

The `AutoConstructorInject` attribute is used on a field that won't already be processed.

### ACONS05

The `AutoConstructorIgnore` or `AutoConstructorInject` are used on a class without the `AutoConstructor` attribute.

### ACONS06

A type specified in `AutoConstructorInject` attribute does not match the type of another parameter with the same name.

In the following sample, both fields will be injected with `guid` as parameter name, but one of type `string` and the other of type `Guid`,
preventing the generator from running.

``` csharp
public partial class Test
{
    [AutoConstructorInject("guid.ToString()", "guid", typeof(Guid))]
    private readonly string _guid2;
    private readonly string _guid;
}
```

### ACONS07

The accessibility defined in the `AutoConstructor` attribute is not an allowed value.

### ACONS08

`AutoConstructorInitializer` attribute used on multiple methods.

### ACONS09

`AutoConstructorInitializer` attribute used on a method not returning void.

### ACONS10

`AutoConstructorInitializer` attribute used on a method with parameters.


:::

### About
:::note

Generating constructor for class with many properties


:::

## How to use

### Example ( source csproj, source files )

<Tabs>

<TabItem value="csproj" label="CSharp Project">

This is the CSharp Project that references **AutoConstructor**
```xml showLineNumbers {16}
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

	<PropertyGroup>
		<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
		<CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
	</PropertyGroup>

	<ItemGroup>
	  <PackageReference Include="AutoConstructor" Version="4.1.1">
	    <PrivateAssets>all</PrivateAssets>
	    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
	  </PackageReference>
	</ItemGroup>
</Project>

```

</TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\Program.cs" label="Program.cs" >

  This is the use of **AutoConstructor** in *Program.cs*

```csharp showLineNumbers 
using QuickConstructorDemo;

var p = new Person("Andrei", "Ignat");

Console.WriteLine(p.FullName());
```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\Person.cs" label="Person.cs" >

  This is the use of **AutoConstructor** in *Person.cs*

```csharp showLineNumbers 
namespace QuickConstructorDemo;

[AutoConstructor]
internal partial class Person
{
    private readonly string FirstName;
    private readonly string? LastName;
    
    public string FullName() => $"{FirstName} {LastName}";
    
}

```
  </TabItem>

</Tabs>

### Generated Files

Those are taken from $(BaseIntermediateOutputPath)\GX

<Tabs>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\obj\GX\AutoConstructor.Generator\AutoConstructor.Generator.AutoConstructorGenerator\AutoConstructorAttribute.cs" label="AutoConstructorAttribute.cs" >


```csharp showLineNumbers 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the AutoConstructor source generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class AutoConstructorAttribute : System.Attribute
{
}

```

  </TabItem>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\obj\GX\AutoConstructor.Generator\AutoConstructor.Generator.AutoConstructorGenerator\AutoConstructorIgnoreAttribute.cs" label="AutoConstructorIgnoreAttribute.cs" >


```csharp showLineNumbers 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the AutoConstructor source generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class AutoConstructorIgnoreAttribute : System.Attribute
{
}

```

  </TabItem>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\obj\GX\AutoConstructor.Generator\AutoConstructor.Generator.AutoConstructorGenerator\AutoConstructorInjectAttribute.cs" label="AutoConstructorInjectAttribute.cs" >


```csharp showLineNumbers 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the AutoConstructor source generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class AutoConstructorInjectAttribute : System.Attribute
{
    public AutoConstructorInjectAttribute(string initializer = null, string parameterName = null, System.Type injectedType = null)
    {
        Initializer = initializer;
        ParameterName = parameterName;
        InjectedType = injectedType;
    }

    public string Initializer { get; }

    public string ParameterName { get; }

    public System.Type InjectedType { get; }
}

```

  </TabItem>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\AutoConstructor\src\ConstructorDemo\obj\GX\AutoConstructor.Generator\AutoConstructor.Generator.AutoConstructorGenerator\QuickConstructorDemo.Person.g.cs" label="QuickConstructorDemo.Person.g.cs" >


```csharp showLineNumbers 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the AutoConstructor source generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace QuickConstructorDemo
{
    partial class Person
    {
        public Person(string FirstName, string? LastName)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }
}

```

  </TabItem>


</Tabs>

## Usefull

### Download Example (.NET  C# )

:::tip

[Download Example project AutoConstructor ](/sources/AutoConstructor.zip)

:::


### Share AutoConstructor 

<ul>
  <li><a href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor&quote=AutoConstructor" title="Share on Facebook" target="_blank">Share on Facebook</a></li>
  <li><a href="https://twitter.com/intent/tweet?source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor&text=AutoConstructor:%20https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor" target="_blank" title="Tweet">Share in Twitter</a></li>
  <li><a href="http://www.reddit.com/submit?url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor&title=AutoConstructor" target="_blank" title="Submit to Reddit">Share on Reddit</a></li>
  <li><a href="http://www.linkedin.com/shareArticle?mini=true&url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor&title=AutoConstructor&summary=&source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FAutoConstructor" target="_blank" title="Share on LinkedIn">Share on Linkedin</a></li>
</ul>

https://ignatandrei.github.io/RSCG_Examples/v2/docs/AutoConstructor

### In the same category (Constructor) - 6 other generators


#### [AutoCtor](/docs/AutoCtor)


#### [AutoDeconstruct](/docs/AutoDeconstruct)


#### [ConstructorGenerator](/docs/ConstructorGenerator)


#### [PrimaryParameter](/docs/PrimaryParameter)


#### [QuickConstructor](/docs/QuickConstructor)


#### [sourcedepend](/docs/sourcedepend)

