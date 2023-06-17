using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Extensions
{
    public static class TaskExtension
    {
        public static async Task EternalWaiting()
        {
            Logger.LogError("!!! ETERNAL WAITING !!!");
            while (true)
            {
                await Task.Delay(1000);
            }
        }
        
        public static async Task<T> AddTimeout<T>(this Task<T> task, int timeout) where T : class
        {
            var token = new CancellationTokenSource().Token;
            Logger.Log($"{task} await with timeout {timeout}");
            
            var result = await Task.WhenAny(task, Task.Delay(timeout)) == task;
            Logger.Log(result ? $"Task completed before timeout {timeout}" : $"Task canceled by timeout {timeout}");

            return result ? task.Result : null;
        }
        
        public static async Task<T> CreateTaskWithTimeout<T>(Func<Task<T>> func, int timeout) where T : class
        {
            var tokenSource = new CancellationTokenSource();
            var mainTask = Task.Run(func, tokenSource.Token);
            var (task, isTaskCompleted) = await mainTask.WaitForTimoutOrTask(timeout);

            if (!isTaskCompleted)
            {
                tokenSource.Cancel();
            }
            //Logger.Log(taskExecuted ? $"Task completed before timeout {timeout}" : $"Task canceled by timeout {timeout}");

            return isTaskCompleted ? task.Result : null;
        }

        public static async Task<bool> AddTimeout(this Task task, int timeout)
        {
            return await Task.WhenAny(task, Task.Delay(timeout)) == task;
        }
        
        public static async Task<bool> AddTimeout(this Task<bool> task, int timeout)
        {
            return await Task.WhenAny(task, Task.Delay(timeout)) == task;
        }
        
        public static async Task<(Task task, bool isTaskCompleted)> WaitForTimoutOrTask(this Task task, int timeout)
        {
            var isTaskCompletedBeforeTimeOut = await Task.WhenAny(task, Task.Delay(timeout)) == task;
            return (task, isTaskCompletedBeforeTimeOut);
        }
        
        public static async Task<(Task<T> task, bool isTaskCompleted)> WaitForTimoutOrTask<T>(this Task<T> task, int timeout)
        {
            var isTaskCompletedBeforeTimeOut = await Task.WhenAny(task, Task.Delay(timeout)) == task;
            return (task, isTaskCompletedBeforeTimeOut);
        }
        
        private static async void RecursiveTask(Func<Task> func, int maxAttemptCount = 10, int timeOut = 5000)
        {
            var i = 0;
            do
            {
                var task = Task.Run(func).AddTimeout(timeOut);

                var result = await task;
                
                if(result)
                {
                    break;
                }
                
            } while (i++ < maxAttemptCount);
        }
    }
}