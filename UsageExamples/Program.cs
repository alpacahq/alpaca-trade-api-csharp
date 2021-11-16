try
{
    var algorithm = new UsageExamples.MeanReversionBrokerage();
    await algorithm.Run();
}
catch (Exception e)
{
    Console.Error.WriteLine(e);
}

Console.Read();
