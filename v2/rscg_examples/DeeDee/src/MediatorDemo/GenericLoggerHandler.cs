﻿public sealed class GenericLoggerHandler : IPipelineAction<Ping, Pong>
{
    public Pong Invoke(Ping request, ref PipelineContext<Pong> context, Next<Pong> next)
    {
        Console.WriteLine("1) Running logger handler");
        try
        {
            var response = next(request , ref context);
            Console.WriteLine("5) No error!");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine("error:" + ex.Message);
            throw;
        }
    }
}