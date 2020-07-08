using HollywoodBetTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Interfaces.Repositories
{
    public interface ITournamentRepository : IBaseRepository<Tournament>
    {
        Tournament GetTournamentByName(string name);
    }
}
