﻿
using AutoSpectre;

namespace ReadData;
[AutoSpectreForm()]
public class ReadPerson
{

    [TextPrompt(Title = "Enter first name", DefaultValueStyle = "bold",
        DefaultValueSource = nameof(FirstNameDefaultValue))]
    public string? FirstName { get; set; }
    public readonly string? FirstNameDefaultValue = "Andrei";

    [TextPrompt(PromptStyle = "green bold")]
    public string? LastName { get; set; }
    public readonly string? LastNameDefaultValue = "Ignat";


}
