using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.EnumConstants
{
    public static class EnumConstants
    {
        public enum TicketStatuses
        {
            Open = 1 ,
            Closed = 2,
            In_progress = 3,
            To_be_tested = 4,
            Reopen = 5
        }
    }
}
