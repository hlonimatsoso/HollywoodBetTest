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
    public class EventDetailStatusController : ControllerBase
    {
        //IEventDetailStatuservice _service;
        HollywoodBetTestContext _db; // HACK: IEventDetailStatuservice MUST be used, will fix soon
        public EventDetailStatusController(HollywoodBetTestContext db)
        {
            _db = db;
            //this._service = service;
        }

        [HttpGet("")]
        public async Task<List<EventDetailStatus>> Get()
        {
            return await this._db.EventDetailStatuses.ToListAsync();
        }

  

        [HttpPost("")]
        public IActionResult Post(EventDetailStatus @EventDetailStatus)
        {
            if (@EventDetailStatus == null)
                throw new Exception("Event Details Status is null");
            
            if(@EventDetailStatus.EventDetailStatusID==0)
            {
                this._db.EventDetailStatuses.Add(@EventDetailStatus);
                this._db.SaveChanges();
            }
            else
            {
                var dbEventDetailStatus = this._db.EventDetailStatuses.Find(@EventDetailStatus.EventDetailStatusID);
                dbEventDetailStatus.EventDetailStatusName = @EventDetailStatus.EventDetailStatusName;

                this._db.EventDetailStatuses.Update(dbEventDetailStatus);
                this._db.SaveChanges();
            }


            

            return Ok(@EventDetailStatus); // should actually return the created status code 201
        }

        [HttpPut("")]
        public IActionResult Put(EventDetailStatus @EventDetailStatus)
        {   
            return Post(@EventDetailStatus); // should actually return the updated status code 204
        }

        [HttpDelete("")]
        public IActionResult Delete(EventDetailStatus[] @EventDetailStatus)
        {
            EventDetailStatus dbEventDetailStatus = null;

            foreach (EventDetailStatus e in EventDetailStatus)
            {
                dbEventDetailStatus =  this._db.EventDetailStatuses.Find(e.EventDetailStatusID);
                if (dbEventDetailStatus != null)
                    this._db.EventDetailStatuses.Remove(dbEventDetailStatus);
            }

            this._db.SaveChanges();

            return NoContent(); // should actually return the updated status code 204
        }
    }
}
