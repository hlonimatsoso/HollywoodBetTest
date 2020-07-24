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
    public class EventDetailController : ControllerBase
    {
        //IEventDetailervice _service;
        HollywoodBetTestContext _db; // HACK: IEventDetailervice MUST be used, will fix soon
        public EventDetailController(HollywoodBetTestContext db)
        {
            _db = db;
            //this._service = service;
        }

        [HttpGet("")]
        public async Task<List<EventDetail>> Get()
        {
            return await this._db.EventDetails.ToListAsync();
        }

  

        [HttpPost("")]
        public IActionResult Post(EventDetail @EventDetail)
        {
            if (@EventDetail == null)
                throw new Exception("Tournament is null");
            
            if(@EventDetail.EventDetailID==0)
            {
                this._db.EventDetails.Add(@EventDetail);
                this._db.SaveChanges();
            }
            else
            {
                var dbEventDetail = this._db.EventDetails.Find(@EventDetail.EventDetailID);
                dbEventDetail.EventDetailName = @EventDetail.EventDetailName;
                dbEventDetail.EventDetailStatusID = @EventDetail.EventDetailStatusID;
                dbEventDetail.EventDetailNumber = @EventDetail.EventDetailNumber;
                dbEventDetail.EventDetailOdd = @EventDetail.EventDetailOdd;
                dbEventDetail.FinishingPosition = @EventDetail.FinishingPosition;
                dbEventDetail.FirstTimer = @EventDetail.FirstTimer;


                this._db.EventDetails.Update(dbEventDetail);
                this._db.SaveChanges();
            }


            

            return Ok(@EventDetail); // should actually return the created status code 201
        }

        [HttpPut("")]
        public IActionResult Put(EventDetail @EventDetail)
        {   
            return Post(@EventDetail); // should actually return the updated status code 204
        }

        [HttpDelete("")]
        public IActionResult Delete(EventDetail[] @EventDetail)
        {
            EventDetail dbEventDetail = null;

            foreach (EventDetail e in EventDetail)
            {
                dbEventDetail =  this._db.EventDetails.Find(e.EventDetailID);
                if (dbEventDetail != null)
                    this._db.EventDetails.Remove(dbEventDetail);
            }

            this._db.SaveChanges();

            return NoContent(); // should actually return the updated status code 204
        }
    }
}
