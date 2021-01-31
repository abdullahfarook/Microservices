//using System;
//using System.Collections.Concurrent;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.NodeServices;

//namespace Framework.Api.Core
//{
//    public class NodeServicesPool : INodeServices
//    {
//        private readonly ConcurrentBag<INodeServices> _serviceBag;
//        private readonly SemaphoreSlim _semaphore;

//        public NodeServicesPool(int serviceCount, IServiceProvider sp)
//        {
//            _serviceBag = new ConcurrentBag<INodeServices>();
//            var options = new NodeServicesOptions(sp);
//            for (int i = 0; i < serviceCount; i++)
//            {
//                _serviceBag.Add(NodeServicesFactory.CreateNodeServices(options));
//            }
//            _semaphore = new SemaphoreSlim(serviceCount);
//        }

//        public void Dispose()
//        {
//            foreach (var service in _serviceBag)
//            {
//                service.Dispose();
//            }
//        }

//        private async Task<T> InternalInvoke<T>(Func<INodeServices, Task<T>> callback)
//        {
//            INodeServices service = null;
//            try
//            {
//                await _semaphore.WaitAsync();
//                _serviceBag.TryTake(out service);

//                return await callback(service);
//            }
//            finally
//            {
//                if (service != null)
//                {
//                    _serviceBag.Add(service);
//                }

//                _semaphore.Release();
//            }
//        }

//        public async Task<T> InvokeAsync<T>(string moduleName, params object[] args) =>
//            await InternalInvoke(service => service.InvokeAsync<T>(moduleName, args));

//        public async Task<T> InvokeAsync<T>(CancellationToken cancellationToken, string moduleName, params object[] args) =>
//            await InternalInvoke(service => service.InvokeAsync<T>(cancellationToken, moduleName, args));

//        public async Task<T> InvokeExportAsync<T>(string moduleName, string exportedFunctionName, params object[] args) =>
//            await InternalInvoke(service => service.InvokeExportAsync<T>(moduleName, exportedFunctionName, args));

//        public async Task<T> InvokeExportAsync<T>(CancellationToken cancellationToken, string moduleName, string exportedFunctionName, params object[] args) =>
//            await InternalInvoke(service => service.InvokeExportAsync<T>(cancellationToken, moduleName, exportedFunctionName, args));
//    }
//}
