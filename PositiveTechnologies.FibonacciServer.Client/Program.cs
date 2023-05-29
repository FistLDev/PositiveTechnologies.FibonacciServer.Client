// See https://aka.ms/new-console-template for more information

using EasyNetQ;

using PositiveTechnologies.FibonacciServer.Client.Http;

int threadsCount = Int32.Parse(Console.ReadLine() ?? "1");

var schedulerPair = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, threadsCount);
Task.Factory.StartNew(
    () => HttpClientInternal.SendFibonacciRequest(1),
    CancellationToken.None,
    TaskCreationOptions.None,
    schedulerPair.ConcurrentScheduler);