using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BaseApp.Common;
using BaseApp.Common.Logs;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Injection;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Scheduler.Attributes;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.SchedulerActions;
using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers.Impl
{
    public class SchedulerWorkerService(IAppScopeFactory appScopeFactory, Func<IUnitOfWorkPerCall> unitOfWorkPerCallFunc)
        : WorkerServiceBase(unitOfWorkPerCallFunc), ISchedulerWorkerService
    {
        public override async Task LoadAndProcessAsync()
        {
            BaseApp.Data.DataContext.Entities.Scheduler scheduler;
            do
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    using (var tran = unitOfWork.BeginTransaction())
                    {
                        scheduler = await unitOfWork.Schedulers.GetNextSchedulerToProcessAsync();
                        if (scheduler == null)
                            continue;
                        scheduler.StartProcessDate = DateTime.Now;
                        await unitOfWork.SaveChangesAsync();
                        tran.Commit();
                    }

                    await ProcessItemAsync(unitOfWork, scheduler);
                }
            } while (scheduler != null);
        }

        public async Task ProcessSchedulerSynchronizedAsync(SchedulerData schedulerData)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                var scheduler = unitOfWork.Schedulers.Get(schedulerData.Id);
                scheduler.StartProcessDate = DateTime.Now;
                await unitOfWork.SaveChangesAsync();
                await ProcessItemAsync(unitOfWork, scheduler, true);    
            }
        }

        private async Task ProcessItemAsync(IUnitOfWork unitOfWork, BaseApp.Data.DataContext.Entities.Scheduler scheduler, bool isSync = false)
        {
            try
            {
                using (var scope = CreateScope(unitOfWork))
                {
                    var manager = GetSchedulerManager(scheduler.SchedulerActionType, new SchedulerActionArgs
                    {
                        UnitOfWork = unitOfWork, Scope = scope
                    });
                    await manager.ProcessAsync(MapScheduler(scheduler));

                    scheduler.EndProcessDate = DateTime.Now;
                    await unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await MarkFailedAsync(scheduler.Id, ex);

                if (isSync)
                    throw;
            }
        }

        private async Task MarkFailedAsync(int schedulerId, Exception ex)
        {
            LogHolder.MainLog.Error(ex, "Error processing scheduler - " + schedulerId);

            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    var scheduler = unitOfWork.Schedulers.Get(schedulerId);
                    scheduler.EndProcessDate = DateTime.Now;
                    scheduler.ErrorMessage = ex.GetBaseException().Message;

                    await unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                LogHolder.MainLog.Error(e, "Error occured while saving scheduler data in failed state - " + schedulerId);
            }
        }

        private IAppScope CreateScope(IUnitOfWork unitOfWork)
        {
            return appScopeFactory.CreateScope(GetLoggedClaims(unitOfWork), unitOfWork);
        }
        
        private LoggedClaims _loggedClaims;
        private LoggedClaims GetLoggedClaims(IUnitOfWork unitOfWork)
        {
            return _loggedClaims ??= new LoggedClaims(unitOfWork.Users.GetFirstAdminAccount());
        }

        private static ISchedulerAction GetSchedulerManager(Enums.SchedulerActionTypes schedulerActionType, SchedulerActionArgs args)
        {
            var modelType = Assembly.GetExecutingAssembly().GetTypes()
                .Single(t => typeof(SchedulerModelBase).IsAssignableFrom(t)
                           && t.GetCustomAttributes<SchedulerActionTypeAttribute>().Any(m => m.SchedulerActionsType == schedulerActionType));

            var builderType = typeof(SchedulerActionBase<>).MakeGenericType(modelType);

            var schedulerManagerType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(builderType.IsAssignableFrom)
                .SingleOrDefault();

            if(schedulerManagerType == null)
                throw new Exception("Scheduler manager not found for - " + modelType);

            var manager = (ISchedulerAction)Activator.CreateInstance(schedulerManagerType, args);
            return manager;
        }

        private SchedulerData MapScheduler(Data.DataContext.Entities.Scheduler m)
        {
            return new SchedulerData
            {
                Id = m.Id,
                SchedulerActionType = m.SchedulerActionType,
                CreatedByUserId = m.CreatedByUserId,
                OnDate = m.OnDate,
                EntityId1 = m.EntityId1,
                EntityId2 = m.EntityId2,
                EntityId3 = m.EntityId3,
                EntityId4 = m.EntityId4,
                EntityData1 = m.EntityData1,
                EntityData2 = m.EntityData2,
                EntityData3 = m.EntityData3,
                EntityData4 = m.EntityData4
            };
        }
    }
}