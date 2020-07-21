using System;
using System.Threading.Tasks;

namespace NftUnity.Test.Extensions
{
    public static class TaskExtensions
    {
        public static T CompletesIn<T>(this Task<T> task, TimeSpan interval)
        {
            var completedInTime = task.Wait(interval);
            if (task.Exception != null)
            {
                throw task.Exception;
            }

            if (!completedInTime)
            {
                throw new TimeoutException();
            }

            return task.Result;
        } 
    }
}