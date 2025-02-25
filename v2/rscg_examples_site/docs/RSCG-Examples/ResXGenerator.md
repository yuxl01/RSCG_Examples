---
sidebar_position: 640
title: 64 - ResXGenerator
description: Resources as string
slug: /ResXGenerator
---
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';
import TOCInline from '@theme/TOCInline';

# ResXGenerator  by Aigamo


<TOCInline toc={toc}  />

## Nuget / site data
[![Nuget](https://img.shields.io/nuget/dt/Aigamo.ResXGenerator?label=Aigamo.ResXGenerator)](https://www.nuget.org/packages/Aigamo.ResXGenerator/)
[![GitHub last commit](https://img.shields.io/github/last-commit/ycanardeau/ResXGenerator?label=updated)](https://github.com/ycanardeau/ResXGenerator)
![GitHub Repo stars](https://img.shields.io/github/stars/ycanardeau/ResXGenerator?style=social)

## Details

### Info
:::info

Name: **ResXGenerator**

ResX Designer Source Generator.

Author: Aigamo

NuGet: 
*https://www.nuget.org/packages/Aigamo.ResXGenerator/*   


You can find more details at https://github.com/ycanardeau/ResXGenerator

Source : https://github.com/ycanardeau/ResXGenerator

:::

### Original Readme
:::note

# ResXGenerator

ResXGenerator is a C# source generator to generate strongly-typed resource classes for looking up localized strings.

NOTE: This is an independent fork of VocaDb/ResXFileCodeGenerator.

## Usage

Install the `Aigamo.ResXGenerator` package:

```psl
dotnet add package Aigamo.ResXGenerator
```

Generated source from [ActivityEntrySortRuleNames.resx](https://github.com/VocaDB/vocadb/blob/6ac18dd2981f82100c8c99566537e4916920219e/VocaDbWeb.Resources/App_GlobalResources/ActivityEntrySortRuleNames.resx):

```cs
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Resources
{
    using System.Globalization;
    using System.Resources;

    public static class ActivityEntrySortRuleNames
    {
        private static ResourceManager? s_resourceManager;
        public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager("VocaDb.Web.App_GlobalResources.ActivityEntrySortRuleNames", typeof(ActivityEntrySortRuleNames).Assembly);
        public static CultureInfo? CultureInfo { get; set; }

        /// <summary>
        /// Looks up a localized string similar to Oldest.
        /// </summary>
        public static string? CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo);

        /// <summary>
        /// Looks up a localized string similar to Newest.
        /// </summary>
        public static string? CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo);
    }
}
```

## New in version 3

-   The generator now utilizes the IIncrementalGenerator API to instantly update the generated code, thus giving you instant intellisense.

-   Added error handling for multiple members of same name, and members that have same name as class. These are clickable in visual studio to lead you to the source of the error, unlike before where they resulted in broken builds and you had to figure out why.

-   Namespace naming fixed for resx files in the top level folder.

-   Resx files can now be named with multiple extensions, e.g. myresources.cshtml.resx and will result in class being called myresources.

-   Added the ability to generate inner classes, partial outer classes and non-static members. Very useful if you want to ensure that only a particular class can use those resources instead of being spread around the codebase.

-   Use same 'Link' setting as msbuild uses to determine embedded file name.

-   Can set a class postfix name

## New in version 3.1

-   The generator can now generate code to lookup translations instead of using the 20 year old System.Resources.ResourceManager

## Options

### PublicClass (per file or globally)

Use cases: https://github.com/VocaDB/ResXFileCodeGenerator/issues/2.

Since version 2.0.0, ResXGenerator generates internal classes by default. You can change this behavior by setting `PublicClass` to `true`.

```xml
<ItemGroup>
  <EmbeddedResource Update="Resources\ArtistCategoriesNames.resx">
    <PublicClass>true</PublicClass>
  </EmbeddedResource>
</ItemGroup>
```

or

```xml
<ItemGroup>
  <EmbeddedResource Update="Resources\ArtistCategoriesNames.resx" PublicClass="true" />
</ItemGroup>
```

If you want to apply this globally, use

```xml
<PropertyGroup>
  <ResXGenerator_PublicClass>true</ResXGenerator_PublicClass>
</PropertyGroup>
```

### NullForgivingOperators (globally)

Use cases: https://github.com/VocaDB/ResXFileCodeGenerator/issues/1.

```xml
<PropertyGroup>
  <ResXGenerator_NullForgivingOperators>true</ResXGenerator_NullForgivingOperators>
</PropertyGroup>
```

By setting `ResXGenerator_NullForgivingOperators` to `true`, ResXGenerator generates

```cs
public static string CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo)!;
```

instead of

```cs
public static string? CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo);
```

### Non-static classes (per file or globally)

To use generated resources with [Microsoft.Extensions.Localization](https://docs.microsoft.com/en-us/dotnet/core/extensions/localization) `IStringLocalizer<T>` and resource manager, the resolved type cannot be a static class. You can disable default behavior per file by setting the value to `false`.

```xml
<ItemGroup>
  <EmbeddedResource Update="Resources\ArtistCategoriesNames.resx">
    <StaticClass>false</StaticClass>
  </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_StaticClass>false</ResXGenerator_StaticClass>
</PropertyGroup>
```

With global non-static class you can also reset `StaticClass` per file by setting the value to anything but `false`.

### Partial classes (per file or globally)

To extend an existing class, you can make your classes partial.

```xml
<ItemGroup>
  <EmbeddedResource Update="Resources\ArtistCategoriesNames.resx">
    <PartialClass>true</PartialClass>
  </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_PartialClass>true</ResXGenerator_PartialClass>
</PropertyGroup>
```

### Static Members (per file or globally)

In some rare cases it might be useful for the members to be non-static.

```xml
<ItemGroup>
  <EmbeddedResource Update="Resources\ArtistCategoriesNames.resx">
    <StaticMembers>false</StaticMembers>
  </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_StaticMembers>false</ResXGenerator_StaticMembers>
</PropertyGroup>
```

### Postfix class name (per file or globally)

In some cases the it is useful if the name of the generated class doesn't follow the filename.

A clear example is Razor pages that always generates a class for the code-behind named "-Model".
This example configuration allows you to use Resources.MyResource in your model, or @Model.Resources.MyResource in your cshtml file.

```xml
<ItemGroup>
  <EmbeddedResource Update="**/Pages/*.resx">
    <ClassNamePostfix>Model</ClassNamePostfix>
    <StaticMembers>false</StaticMembers>
    <StaticClass>false</StaticClass>
    <PartialClass>true</PartialClass>
    <PublicClass>true</PublicClass>
    <InnerClassVisibility>public</InnerClassVisibility>
    <PartialClass>false</PartialClass>
    <InnerClassInstanceName>Resources</InnerClassInstanceName>
    <InnerClassName>_Resources</InnerClassName>
  </EmbeddedResource>
</ItemGroup>
```

or just the postfix globally

```xml
<PropertyGroup>
  <ResXGenerator_ClassNamePostfix>Model</ResXGenerator_ClassNamePostfix>
</PropertyGroup>
```

## Inner classes (per file or globally)

If your resx files are organized along with code files, it can be quite useful to ensure that the resources are not accessible outside the specific class the resx file belong to.

```xml
<ItemGroup>
    <EmbeddedResource Update="**/*.resx">
        <DependentUpon>$([System.String]::Copy('%(FileName).cs'))</DependentUpon>
        <InnerClassName>MyResources</InnerClassName>
        <InnerClassVisibility>private</InnerClassVisibility>
        <InnerClassInstanceName>EveryoneLikeMyNaming</InnerClassInstanceName>
        <StaticMembers>false</StaticMembers>
        <StaticClass>false</StaticClass>
        <PartialClass>true</PartialClass>
    </EmbeddedResource>
    <EmbeddedResource Update="**/*.??.resx;**/*.??-??.resx">
        <DependentUpon>$([System.IO.Path]::GetFileNameWithoutExtension('%(FileName)')).resx</DependentUpon>
    </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_InnerClassName>MyResources</ResXGenerator_InnerClassName>
  <ResXGenerator_InnerClassVisibility>private</ResXGenerator_InnerClassVisibility>
  <ResXGenerator_InnerClassInstanceName>EveryoneLikeMyNaming</InnerClassInstanceName>
  <ResXGenerator_StaticMembers>false</ResXGenerator_StaticMembers>
  <ResXGenerator_StaticClass>false</ResXGenerator_StaticClass>
  <ResXGenerator_PartialClass>true</ResXGenerator_PartialClass>
</PropertyGroup>
```

This example would generate files like this:

```cs
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Resources
{
    using System.Globalization;
    using System.Resources;

    public partial class ActivityEntryModel
    {
        public MyResources EveryoneLikeMyNaming { get; } = new();

        private class MyResources
        {
            private static ResourceManager? s_resourceManager;
            public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager("VocaDb.Web.App_GlobalResources.ActivityEntryModel", typeof(ActivityEntryModel).Assembly);
            public CultureInfo? CultureInfo { get; set; }

            /// <summary>
            /// Looks up a localized string similar to Oldest.
            /// </summary>
            public string? CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo);

            /// <summary>
            /// Looks up a localized string similar to Newest.
            /// </summary>
            public string? CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo);
        }
    }
}
```

### Inner Class Visibility (per file or globally)

By default inner classes are not generated, unless this setting is one of the following:

-   Public
-   Internal
-   Private
-   Protected
-   SameAsOuter

Case is ignored, so you could use "private".

It is also possible to use "NotGenerated" to override on a file if the global setting is to generate inner classes.

```xml
<ItemGroup>
    <EmbeddedResource Update="**/*.resx">
        <InnerClassVisibility>private</InnerClassVisibility>
    </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_InnerClassVisibility>private</ResXGenerator_InnerClassVisibility>
</PropertyGroup>
```

### Inner Class name (per file or globally)

By default the inner class is named "Resources", which can be overridden with this setting:

```xml
<ItemGroup>
    <EmbeddedResource Update="**/*.resx">
        <InnerClassName>MyResources</InnerClassName>
    </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_InnerClassName>MyResources</ResXGenerator_InnerClassName>
</PropertyGroup>
```

### Inner Class instance name (per file or globally)

By default no instance is available of the class, but that can be made available if this setting is given.

```xml
<ItemGroup>
    <EmbeddedResource Update="**/*.resx">
        <InnerClassInstanceName>EveryoneLikeMyNaming</InnerClassInstanceName>
    </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_InnerClassInstanceName>EveryoneLikeMyNaming</ResXGenerator_InnerClassInstanceName>
</PropertyGroup>
```

For brevity, settings to make everything non-static is omitted.

### Generate Code (per file or globally)

By default the ancient `System.Resources.ResourceManager` is used.

Benefits of using `System.Resources.ResourceManager`:

-   Supports custom `CultureInfo`
-   Languages are only loaded the first time a language is referenced
-   Only use memory for the languages used
-   Can ship satellite dlls separately

Disadvantages of using `System.Resources.ResourceManager`

-   The satellite dlls are always lazy loaded, so cold start penalty is high
-   Satellite dlls requires that you can scan the dir for which files are available, which can cause issues in some project types
-   Loading a satellite dll takes way more memory than just loading the respective strings
-   Build time for .resources -> satellite dll can be quite slow (~150msec per file)
-   Linker optimization doesn't work, since it cannot know which resources are referenced

Benefits of using `GenerateCode` code generation:

-   All languages are placed in the main dll, no more satellite dlls
-   Lookup speed is ~600% faster (5ns vs 33ns)
-   Zero allocations
-   Very small code footprint (about 10 bytes per language, instead of including the entire `System.Resources`)
-   Very fast build time
-   Because all code is referencing the strings directly, the linker can see which strings are actually used and which are not.
-   No cold start penalty
-   Smaller combined size of dll (up to 50%, since it doesn't need to store the keys for every single language)

Disadvantages of using `GenerateCode` code generation

-   Since `CultureInfo` are pre-computed, custom `CultureInfo` are not supported (or rather, they always return the default language)
-   Cannot lookup "all" keys (unless using reflection)
-   Main dll size increased since it contains all language strings (sometimes, the compiler can pack code strings much better than resource strings and it doesn't need to store the keys)

Notice, it is required to set `GenerateResource` to false for all resx files to prevent the built-in resgen.exe from running.

```xml
<ItemGroup>
    <EmbeddedResource Update="**/*.resx">
        <GenerateCode>true</GenerateCode>
        <GenerateResource>false</GenerateResource>
    </EmbeddedResource>
</ItemGroup>
```

or globally

```xml
<PropertyGroup>
  <ResXGenerator_GenerateCode>true</ResXGenerator_GenerateCode>
</PropertyGroup>
<ItemGroup>
    <EmbeddedResource Update="@(EmbeddedResource)">
        <GenerateResource>false</GenerateResource>
    </EmbeddedResource>
</ItemGroup>
```

If you get build error MSB3030, add this to your csproj to prevent it from trying to copy satellite dlls that no longer exists

```xml
<Target Name="PreventMSB3030" DependsOnTargets="ComputeIntermediateSatelliteAssemblies" BeforeTargets="GenerateSatelliteAssemblies" >
  <ItemGroup>
    <IntermediateSatelliteAssembliesWithTargetPath Remove="@(IntermediateSatelliteAssembliesWithTargetPath)"></IntermediateSatelliteAssembliesWithTargetPath>
  </ItemGroup>
</Target>
```

## Resource file namespaces

Linked resources namespace follow `Link` if it is set. The `Link` setting is also used by msbuild built-in 'resgen.exe' to determine the embedded filename.

Use-case: Linking `.resx` files from outside source (e.g. generated in a localization sub-module by translators) and expose them as "Resources" namespace.

```xml
<ItemGroup>
  <EmbeddedResource Include="..\..\Another.Project\Translations\*.resx">
    <Link>Resources\%(FileName)%(Extension)</Link>
    <PublicClass>true</PublicClass>
    <StaticClass>false</StaticClass>
  </EmbeddedResource>
  <EmbeddedResource Update="..\..\Another.Project\Translations\*.*.resx">
    <DependentUpon>$([System.IO.Path]::GetFilenameWithoutExtension([System.String]::Copy('%(FileName)'))).resx</DependentUpon>
  </EmbeddedResource>
</ItemGroup>
```

You can also use the `TargetPath` to just overwrite the namespace

```xml
<ItemGroup>
  <EmbeddedResource Include="..\..\Another.Project\Translations\*.resx">
    <TargetPath>Resources\%(FileName)%(Extension)</TargetPath>
    <PublicClass>true</PublicClass>
    <StaticClass>false</StaticClass>
  </EmbeddedResource>
  <EmbeddedResource Update="..\..\Another.Project\Translations\*.*.resx">
    <DependentUpon>$([System.IO.Path]::GetFilenameWithoutExtension([System.String]::Copy('%(FileName)'))).resx</DependentUpon>
  </EmbeddedResource>
</ItemGroup>
```

It is also possible to set the namespace using the `CustomToolNamespace` setting. Unlike the `Link` and `TargetPath`, which will prepend the assemblies namespace and includes the filename, the `CustomToolNamespace` is taken verbatim.

```xml
<ItemGroup>
  <EmbeddedResource Update="**\*.resx">
    <CustomToolNamespace>MyNamespace.AllMyResourcesAreBelongToYouNamespace</CustomToolNamespace>
  </EmbeddedResource>
</ItemGroup>
```

## Excluding resx files

Individual resx files can also be excluded from being processed by setting the `SkipFile` metadata to true.

```xml
<ItemGroup>
    <EmbeddedResource Update="ExcludedFile.resx">
        <SkipFile>true</SkipFile>
    </EmbeddedResource>
</ItemGroup>
```

Alternatively it can be set with the attribute `SkipFile="true"`.

```xml
<ItemGroup>
	<EmbeddedResource Update="ExcludedFile.resx" SkipFile="true" />
</ItemGroup>
```

## References

-   [Introducing C# Source Generators | .NET Blog](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
-   [microsoft/CsWin32: A source generator to add a user-defined set of Win32 P/Invoke methods and supporting types to a C# project.](https://github.com/microsoft/cswin32)
-   [kenkendk/mdresxfilecodegenerator: Resx Designer Generator](https://github.com/kenkendk/mdresxfilecodegenerator)
-   [dotnet/ResXResourceManager: Manage localization of all ResX-Based resources in one central place.](https://github.com/dotnet/ResXResourceManager)
-   [roslyn/source-generators.cookbook.md at master · dotnet/roslyn](https://github.com/dotnet/roslyn/blob/master/docs/features/source-generators.cookbook.md)
-   [roslyn/Using Additional Files.md at master · dotnet/roslyn](https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Using%20Additional%20Files.md)
-   [ufcpp - YouTube](https://www.youtube.com/channel/UCY-z_9mau6X-Vr4gk2aWtMQ)
-   [amis92/csharp-source-generators: A list of C# Source Generators (not necessarily awesome) and associated resources: articles, talks, demos.](https://github.com/amis92/csharp-source-generators)
-   [A NuGet package workflow using GitHub Actions | by Andrew Craven | Medium](https://acraven.medium.com/a-nuget-package-workflow-using-github-actions-7da8c6557863)


:::

### About
:::note

Resources as string


:::

## How to use

### Example ( source csproj, source files )

<Tabs>

<TabItem value="csproj" label="CSharp Project">

This is the CSharp Project that references **ResXGenerator**
```xml showLineNumbers {11}
<Project Sdk="Microsoft.NET.Sdk">

	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net7.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
	</PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Aigamo.ResXGenerator" Version="4.2.0">
			<PrivateAssets>all</PrivateAssets>
			<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
		</PackageReference>
	</ItemGroup>

	<ItemGroup>
		<Compile Update="GenResources.Designer.cs">
		  <DesignTime>True</DesignTime>
		  <AutoGen>True</AutoGen>
		  <DependentUpon>GenResources.resx</DependentUpon>
		</Compile>
		<Compile Update="GenResources.resx">
			<GenerateCode>true</GenerateCode>					
			<GenerateResource>false</GenerateResource>
			<PublicClass>true</PublicClass>
		</Compile>

	</ItemGroup>

	<ItemGroup>
	  <EmbeddedResource Update="GenResources.resx">
	    <Generator>PublicResXFileCodeGenerator</Generator>
	    <LastGenOutput>GenResources.Designer.cs</LastGenOutput>
	  </EmbeddedResource>
	</ItemGroup>
	<PropertyGroup>
		<ResXGenerator_GenerateCode>true</ResXGenerator_GenerateCode>
		<ResXGenerator_ClassNamePostfix>Model</ResXGenerator_ClassNamePostfix>		
	</PropertyGroup>
	<PropertyGroup>
		<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
		<CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
	</PropertyGroup>

</Project>

```

</TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\ResXGenerator\src\ResXDemo\Program.cs" label="Program.cs" >

  This is the use of **ResXGenerator** in *Program.cs*

```csharp showLineNumbers 
Console.WriteLine(ResXDemo.GenResourcesModel.MyName);
Console.WriteLine(ResXDemo.GenResources.MyName);


```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\ResXGenerator\src\ResXDemo\GenResources.resx" label="GenResources.resx" >

  This is the use of **ResXGenerator** in *GenResources.resx*

```csharp showLineNumbers 
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name="MyName" xml:space="preserve">
    <value>Andrei Ignat {x}</value>
  </data>
</root>
```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\ResXGenerator\src\ResXDemo\GenResources.Designer.cs" label="GenResources.Designer.cs" >

  This is the use of **ResXGenerator** in *GenResources.Designer.cs*

```csharp showLineNumbers 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResXDemo {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class GenResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal GenResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ResXDemo.GenResources", typeof(GenResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Andrei Ignat {x}.
        /// </summary>
        public static string MyName {
            get {
                return ResourceManager.GetString("MyName", resourceCulture);
            }
        }
    }
}

```
  </TabItem>

</Tabs>

### Generated Files

Those are taken from $(BaseIntermediateOutputPath)\GX

<Tabs>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\ResXGenerator\src\ResXDemo\obj\GX\Aigamo.ResXGenerator\Aigamo.ResXGenerator.SourceGenerator\Aigamo.ResXGenerator..g.cs" label="Aigamo.ResXGenerator..g.cs" >


```csharp showLineNumbers 
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Aigamo.ResXGenerator;
internal static partial class Helpers
{
    public static string GetString_(string fallback) => System.Globalization.CultureInfo.CurrentUICulture.LCID switch
    {
        _ => fallback
    };
}

```

  </TabItem>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\ResXGenerator\src\ResXDemo\obj\GX\Aigamo.ResXGenerator\Aigamo.ResXGenerator.SourceGenerator\ResXDemo.GenResourcesModel.g.cs" label="ResXDemo.GenResourcesModel.g.cs" >


```csharp showLineNumbers 
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace ResXDemo;
using static Aigamo.ResXGenerator.Helpers;

internal static class GenResourcesModel
{

    /// <summary>
    /// Looks up a localized string similar to Andrei Ignat {x}.
    /// </summary>
    public static string? MyName => GetString_("Andrei Ignat {x}");
}

```

  </TabItem>


</Tabs>

## Usefull

### Download Example (.NET  C# )

:::tip

[Download Example project ResXGenerator ](/sources/ResXGenerator.zip)

:::


### Share ResXGenerator 

<ul>
  <li><a href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator&quote=ResXGenerator" title="Share on Facebook" target="_blank">Share on Facebook</a></li>
  <li><a href="https://twitter.com/intent/tweet?source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator&text=ResXGenerator:%20https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator" target="_blank" title="Tweet">Share in Twitter</a></li>
  <li><a href="http://www.reddit.com/submit?url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator&title=ResXGenerator" target="_blank" title="Submit to Reddit">Share on Reddit</a></li>
  <li><a href="http://www.linkedin.com/shareArticle?mini=true&url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator&title=ResXGenerator&summary=&source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FResXGenerator" target="_blank" title="Share on LinkedIn">Share on Linkedin</a></li>
</ul>

https://ignatandrei.github.io/RSCG_Examples/v2/docs/ResXGenerator

### In the same category (FilesToCode) - 13 other generators


#### [Chorn.EmbeddedResourceAccessGenerator](/docs/Chorn.EmbeddedResourceAccessGenerator)


#### [corecraft](/docs/corecraft)


#### [Datacute.EmbeddedResourcePropertyGenerator](/docs/Datacute.EmbeddedResourcePropertyGenerator)


#### [DotnetYang](/docs/DotnetYang)


#### [EmbedResourceCSharp](/docs/EmbedResourceCSharp)


#### [LingoGen](/docs/LingoGen)


#### [NotNotAppSettings](/docs/NotNotAppSettings)


#### [Podimo.ConstEmbed](/docs/Podimo.ConstEmbed)


#### [RSCG_JSON2Class](/docs/RSCG_JSON2Class)


#### [RSCG_Utils](/docs/RSCG_Utils)


#### [ThisAssembly_Resources](/docs/ThisAssembly_Resources)


#### [ThisAssembly.Strings](/docs/ThisAssembly.Strings)


#### [Weave](/docs/Weave)

