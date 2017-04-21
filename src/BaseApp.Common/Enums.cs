using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseApp.Common
{
    public class Enums
    {
        public enum SchedulerActionTypes
        {
            ResetPasswordEmail = 1,

            ExampleAction = 1000
        }

        public enum MenuItemTypes
        {

        }

        public enum DbDataInitStrategyTypes
        {
            MigrateValidate,
            MigrateToLatest,
            None
        }
    }
}
