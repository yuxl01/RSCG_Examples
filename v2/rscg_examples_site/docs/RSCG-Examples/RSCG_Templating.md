---
sidebar_position: 690
title: 69 - RSCG_Templating
description: Templating every your data ( starting with class)
slug: /RSCG_Templating
---
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';
import TOCInline from '@theme/TOCInline';

# RSCG_Templating  by Andrei Ignat


<TOCInline toc={toc}  />

## Nuget / site data
[![Nuget](https://img.shields.io/nuget/dt/RSCG_Templating?label=RSCG_Templating)](https://www.nuget.org/packages/RSCG_Templating/)[![Nuget](https://img.shields.io/nuget/dt/RSCG_TemplatingCommon?label=RSCG_TemplatingCommon)](https://www.nuget.org/packages/RSCG_TemplatingCommon)
[![GitHub last commit](https://img.shields.io/github/last-commit/ignatandrei/rscg_templating?label=updated)](https://github.com/ignatandrei/rscg_templating/)
![GitHub Repo stars](https://img.shields.io/github/stars/ignatandrei/rscg_templating?style=social)

## Details

### Info
:::info

Name: **RSCG_Templating**

Roslyn Templating for all

Author: Andrei Ignat

NuGet: 
*https://www.nuget.org/packages/RSCG_Templating/*   

*https://www.nuget.org/packages/RSCG_TemplatingCommon*   


You can find more details at https://github.com/ignatandrei/rscg_templating/

Source : https://github.com/ignatandrei/rscg_templating/

:::

### Original Readme
:::note

# RSCG_Templating

Templating for generating everything from classes, methods from a Roslyn Code Generator

Templating is in SCRIBAN form

## How to use

Add reference to 

```xml
  <ItemGroup>
    <PackageReference Include="RSCG_Templating" Version="2023.1007.724" OutputItemType="Analyzer"  ReferenceOutputAssembly="false"   />
    <PackageReference Include="RSCG_TemplatingCommon" Version="2023.1007.724" />
  </ItemGroup>
<!-- this is just for debug purposes -->
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
<!-- those are the templates files, see IGenerateDataFromClass -->
  <ItemGroup>
    <AdditionalFiles Include="ClassTypeName.txt" />
    <AdditionalFiles Include="ClassPropByName.txt" />
  </ItemGroup>

```

Then add additional files , for example 
```scriban
//autogenerated by RSCG_Templating version {{data.Version}} from file {{fileName}}
namespace {{data.nameSpace}} {
	 
	partial class {{data.className}} {
		public string MyTypeName = "{{data.nameSpace}}.{{data.className}}";		
	}//end class

}//end namespace
```

Now add 

```csharp
//can have multiple attributes on partial classes
[IGenerateDataFromClass("ClassTypeName")]
public partial class Person
```

## Advanced uses

For the moment , RSCG_Templating generates definition for a class with properties + methods .
See example for generating enum from properties and setting properties by name

```csharp
var x = new Person();
Console.WriteLine("The generated string type is "+x.MyTypeName);
x.FirstName = "Andrei";
//set last name via prop
x.SetPropValue(ePerson_Properties.LastName, "Ignat");
Console.WriteLine("called directly first name : " + x.FirstName);
Console.WriteLine("called via enum of prop first name : " + x.GetPropValue(ePerson_Properties.FirstName));
Console.WriteLine("called get property :" + x.GetPropValue(ePerson_Properties.Name));
```

See example at https://github.com/ignatandrei/RSCG_Templating/tree/main/src/RSCG_Templating

## More templates


10. Template for having the class type name: ClassTypeName
20. Template for having the class properties as enum : ClassPropByName
30. Template for setting properties after name : ClassPropByName

:::

### About
:::note

Templating every your data ( starting with class)


:::

## How to use

### Example ( source csproj, source files )

<Tabs>

<TabItem value="csproj" label="CSharp Project">

This is the CSharp Project that references **RSCG_Templating**
```xml showLineNumbers {14}
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
	<ItemGroup>
		<AdditionalFiles Include="ClassTypeName.txt" />
		<AdditionalFiles Include="ClassPropByName.txt" />
	</ItemGroup>
	 <ItemGroup>
    <PackageReference Include="RSCG_Templating" Version="2023.1022.1748" OutputItemType="Analyzer"  />
    <PackageReference Include="RSCG_TemplatingCommon" Version="2023.1022.1748"  />
    
  </ItemGroup>

	<PropertyGroup>
		<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
		<CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
	</PropertyGroup>

</Project>

```

</TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\Program.cs" label="Program.cs" >

  This is the use of **RSCG_Templating** in *Program.cs*

```csharp showLineNumbers 
using RSCG_TemplatingDemo;

var x = new Person();
Console.WriteLine("The generated string type is " + x.MyTypeName);
x.FirstName = "Andrei";
//set last name via prop
x.SetPropValue(ePerson_Properties.LastName, "Ignat");
Console.WriteLine("called directly first name : " + x.FirstName);
Console.WriteLine("called via enum of prop first name : " + x.GetPropValue(ePerson_Properties.FirstName));
Console.WriteLine("called get property :" + x.GetPropValue(ePerson_Properties.Name));

Console.WriteLine("this will throw error because Name has not set ");
try
{
    x.SetPropValue(ePerson_Properties.Name, "asd");
}
catch (Exception)
{
    Console.WriteLine("this is good!");
}
Console.ReadLine();
```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\Person.cs" label="Person.cs" >

  This is the use of **RSCG_Templating** in *Person.cs*

```csharp showLineNumbers 
using RSCG_TemplatingCommon;

namespace RSCG_TemplatingDemo;

[IGenerateDataFromClass("ClassTypeName")]
[IGenerateDataFromClass("ClassPropByName")]
public partial class Person
{
    public string Name { get { return FullName(" "); } }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName(string separator = " ")
    {
        return FirstName + separator + LastName;
    }
    public void DisplayNameOnConsole()
    {
        Console.WriteLine(FullName());
    }
    public async Task<string> GetName()
    {
        await Task.Delay(1000);
        return FirstName ?? "";
    }
    public Task<string> GetFullName()
    {
        return Task.FromResult(FullName());
    }
    public Task SaveId(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("this is an error because is <0 ");
        }
        return Task.CompletedTask;
    }
}

```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\ClassPropByName.txt" label="ClassPropByName.txt" >

  This is the use of **RSCG_Templating** in *ClassPropByName.txt*

```csharp showLineNumbers 
//autogenerated by RSCG_Templating version {{data.Version}} from file {{fileName}}
namespace {{data.nameSpace}} {
	public enum e{{data.className}}_Properties {
		None = 0,
		{{ for prop in data.properties }}
		{{prop.PropertyName}},
		{{ end }}
	} 
	partial class {{data.className}} {

		public object GetPropValue(e{{data.className}}_Properties prop){
			switch(prop){
				{{ for prop in data.properties }}
					case e{{data.className}}_Properties.{{prop.PropertyName}}:
					{{ if prop.CanCallGetMethod }}
						return this.{{prop.PropertyName}};
					{{ else }}
						throw new NotImplementedException();
					{{ end}}
				{{ end }}
				default:
						throw new NotImplementedException();
			}
		}
		public void SetPropValue<T>(e{{data.className}}_Properties prop , T value){
			switch(prop){
				{{ for prop in data.properties }}
					case e{{data.className}}_Properties.{{prop.PropertyName}}:
					{{ if prop.CanCallSetMethod }}
						this.{{prop.PropertyName}} = ({{prop.PropertyType}})(dynamic)value;
						break;
					{{ else }}
						throw new NotImplementedException();
					{{ end}}
				{{ end }}
				default:
						throw new NotImplementedException();
			}
		}
	}//end class

}//end namespace
```
  </TabItem>

  <TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\ClassTypeName.txt" label="ClassTypeName.txt" >

  This is the use of **RSCG_Templating** in *ClassTypeName.txt*

```csharp showLineNumbers 
//autogenerated by RSCG_Templating version {{data.Version}} from file {{fileName}}
namespace {{data.nameSpace}} {
	 
	partial class {{data.className}} {
		public string MyTypeName = "{{data.nameSpace}}.{{data.className}}";		
	}//end class

}//end namespace
```
  </TabItem>

</Tabs>

### Generated Files

Those are taken from $(BaseIntermediateOutputPath)\GX

<Tabs>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\obj\GX\RSCG_Templating\RSCG_Templating.GeneratorIntercept\RSCG_TemplatingDemo.Person.ClassPropByName.cs" label="RSCG_TemplatingDemo.Person.ClassPropByName.cs" >


```csharp showLineNumbers 
//autogenerated by RSCG_Templating version 2023.1022.1748.0 from file Microsoft.CodeAnalysis.AdditionalTextFile
namespace RSCG_TemplatingDemo {
	public enum ePerson_Properties {
		None = 0,
		
				Name,
				
				FirstName,
				
				LastName,
				
	} 
	partial class Person {

		public object GetPropValue(ePerson_Properties prop){
			switch(prop){
				
									case ePerson_Properties.Name:
									
											return this.Name;
										
								
									case ePerson_Properties.FirstName:
									
											return this.FirstName;
										
								
									case ePerson_Properties.LastName:
									
											return this.LastName;
										
								
				default:
						throw new NotImplementedException();
			}
		}
		public void SetPropValue<T>(ePerson_Properties prop , T value){
			switch(prop){
				
									case ePerson_Properties.Name:
									
											throw new NotImplementedException();
										
								
									case ePerson_Properties.FirstName:
									
											this.FirstName = (string?)(dynamic)value;
											break;
										
								
									case ePerson_Properties.LastName:
									
											this.LastName = (string?)(dynamic)value;
											break;
										
								
				default:
						throw new NotImplementedException();
			}
		}
	}//end class

}//end namespace
```

  </TabItem>


<TabItem value="D:\gth\RSCG_Examples\v2\rscg_examples\RSCG_Templating\src\RSCG_TemplatingDemo\obj\GX\RSCG_Templating\RSCG_Templating.GeneratorIntercept\RSCG_TemplatingDemo.Person.ClassTypeName.cs" label="RSCG_TemplatingDemo.Person.ClassTypeName.cs" >


```csharp showLineNumbers 
//autogenerated by RSCG_Templating version 2023.1022.1748.0 from file Microsoft.CodeAnalysis.AdditionalTextFile
namespace RSCG_TemplatingDemo {
	 
	partial class Person {
		public string MyTypeName = "RSCG_TemplatingDemo.Person";		
	}//end class

}//end namespace
```

  </TabItem>


</Tabs>

## Usefull

### Download Example (.NET  C# )

:::tip

[Download Example project RSCG_Templating ](/sources/RSCG_Templating.zip)

:::


### Share RSCG_Templating 

<ul>
  <li><a href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating&quote=RSCG_Templating" title="Share on Facebook" target="_blank">Share on Facebook</a></li>
  <li><a href="https://twitter.com/intent/tweet?source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating&text=RSCG_Templating:%20https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating" target="_blank" title="Tweet">Share in Twitter</a></li>
  <li><a href="http://www.reddit.com/submit?url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating&title=RSCG_Templating" target="_blank" title="Submit to Reddit">Share on Reddit</a></li>
  <li><a href="http://www.linkedin.com/shareArticle?mini=true&url=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating&title=RSCG_Templating&summary=&source=https%3A%2F%2Fignatandrei.github.io%2FRSCG_Examples%2Fv2%2Fdocs%2FRSCG_Templating" target="_blank" title="Share on LinkedIn">Share on Linkedin</a></li>
</ul>

https://ignatandrei.github.io/RSCG_Examples/v2/docs/RSCG_Templating

### In the same category (Templating) - 11 other generators


#### [Gobie](/docs/Gobie)


#### [InterceptorTemplate](/docs/InterceptorTemplate)


#### [JKToolKit.TemplatePropertyGenerator](/docs/JKToolKit.TemplatePropertyGenerator)


#### [Microsoft.NET.Sdk.Razor.SourceGenerators](/docs/Microsoft.NET.Sdk.Razor.SourceGenerators)


#### [Minerals.AutoMixins](/docs/Minerals.AutoMixins)


#### [MorrisMoxy](/docs/MorrisMoxy)


#### [NTypewriter](/docs/NTypewriter)


#### [RazorBlade](/docs/RazorBlade)


#### [RazorSlices](/docs/RazorSlices)


#### [RSCG_IFormattable](/docs/RSCG_IFormattable)


#### [spreadcheetah](/docs/spreadcheetah)

