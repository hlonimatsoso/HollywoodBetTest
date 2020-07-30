using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Models
{
    public static class Roles
    {
        public const string Customer = "CUSTOMER";
        public const string Admin = "ADMIN";
        public const string Manager = "MANAGER";

    }

    public static class User_Access
    {
        public const string TOURNAMENT_READ = "tournaments.read";
        public const string TOURNAMENT_WRITE = "tournaments.write";
        public const string TOURNAMENT_DELETE = "tournaments.delete";

        public const string EVENT_READ = "events.read";
        public const string EVENT_WRITE = "events.write";
        public const string EVENT_DELETE = "events.delete";

        public const string HORSE_READ = "horses.read";
        public const string HORSE_WRITE = "horses.write";
        public const string HORSE_DELETE = "horses.delete";

    }
}
