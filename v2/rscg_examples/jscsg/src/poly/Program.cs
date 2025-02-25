﻿//https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-7-0
using JsonPolymorphicGeneratorDemo;
using System.Text.Json;

Person[] persons = new Person[2];
persons[0] = new Student() { Name="Student Ignat"};

persons[1] = new Teacher() { Name = "Teacher Ignat" };
//JsonSerializerOptions opt = new ()
//{
//    WriteIndented = true
//};
//var ser = JsonSerializer.Serialize(persons, opt);

var ser = JsonSerializer.Serialize(persons, ProjectJsonSerializerContext.Default.Options);

Console.WriteLine(ser);
var p = JsonSerializer.Deserialize<Person[]>(ser,ProjectJsonSerializerContext.Default.Options);
if(p != null)
foreach (var item in p)
{
    Console.WriteLine(item.Data());
}
