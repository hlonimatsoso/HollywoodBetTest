using HollywoodBetTest.Interfaces.Repositories;
using HollywoodBetTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Data.Repositories
{
    public class TournamentRepository : BaseRepository<Tournament>, ITournamentRepository
    {
    
        public TournamentRepository(HollywoodBetTestContext context):base(context)
        { }

        public Tournament GetTournamentByName(string name)
        {
            throw new NotImplementedException();
        }

    }
}
