﻿using Roozie.AutoInterface;

namespace Roozie.AutoInterfaceDemo;

[AutoInterface(IncludeMethods =true,IncludeProperties =true)]
public partial class Person
{
    public int ID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName()
    {
        return FirstName + " " + LastName;
    }
}
