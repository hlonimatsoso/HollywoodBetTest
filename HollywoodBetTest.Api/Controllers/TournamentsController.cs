using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HollywoodBetTest.Data;
using HollywoodBetTest.Interfaces.Services;
using HollywoodBetTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HollywoodBetTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        //ITournamentService _service;
        HollywoodBetTestContext _db; // HACK: ITournamentService MUST be used, will fix soon
        public TournamentsController(HollywoodBetTestContext db)
        {
            _db = db;
            //this._service = service;
        }

        [HttpGet("")]
        public Task<List<Tournament>> Get()
        {
            return this._db.Tournaments.ToListAsync();
        }

        [HttpGet("DummyTournaments")]
        public List<Tournament> GetDummyTournaments()
        {

            List<Tournament> retVal = new List<Tournament>();

            //retVal = new List<Tournament> {
            //    new Tournament{
            //        TournamentID = 1,
            //        TournamentName = "Opening Tounament"
            //    },
            //    new Tournament{
            //        TournamentID = 2,
            //        TournamentName = "Closing Tounament"
            //    }
            //};

            //retVal = this._service.GetAll();

            return _db.Tournaments.ToList();
        }

        [HttpPost("")]
        public IActionResult Post(Tournament tournament)
        {
            if (tournament == null)
                throw new Exception("Tournament is null");
            
            if(tournament.TournamentID==0)
            {
                this._db.Tournaments.Add(tournament);
                this._db.SaveChanges();
            }
            else
            {
                var dbTournament = this._db.Tournaments.Find(tournament.TournamentID);
                dbTournament.TournamentName = tournament.TournamentName;

                this._db.Tournaments.Update(dbTournament);
                this._db.SaveChanges();
            }


            

            return Ok(); // should actually return the created status code 201
        }

        [HttpPut("")]
        public IActionResult Put(Tournament tournament)
        {
            if (tournament == null)
                throw new Exception("Tournament is null");
          

            return NoContent(); // should actually return the updated status code 204
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(Tournament[] tournaments)
        {
            Tournament dbTournament = null;

            foreach (Tournament tournament in tournaments)
            {
                dbTournament =  this._db.Tournaments.Find(tournament.TournamentID);
                if (dbTournament != null)
                    this._db.Tournaments.Remove(dbTournament);
            }

            this._db.SaveChanges();

            return NoContent(); // should actually return the updated status code 204
        }
    }
}
