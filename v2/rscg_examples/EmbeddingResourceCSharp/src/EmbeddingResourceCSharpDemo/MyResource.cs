﻿
namespace EmbeddingResourceCSharpDemo;

public partial class MyResource
{
    [EmbedResourceCSharp.FileEmbed("createDB.txt")]
    public static partial System.ReadOnlySpan<byte> GetContentOfCreate();
}
