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
    public class EventsController : ControllerBase
    {
        //IEventservice _service;
        HollywoodBetTestContext _db; // HACK: IEventservice MUST be used, will fix soon
        public EventsController(HollywoodBetTestContext db)
        {
            _db = db;
            //this._service = service;
        }

        [HttpGet("")]
        public async Task<List<Event>> Get()
        {
            return await this._db.Events.ToListAsync();
        }

  

        [HttpPost("")]
        public IActionResult Post(Event @event)
        {
            if (@event == null)
                throw new Exception("Tournament is null");
            
            if(@event.EventID==0)
            {
                this._db.Events.Add(@event);
                this._db.SaveChanges();
            }
            else
            {
                var dbEvent = this._db.Events.Find(@event.EventID);
                dbEvent.EventName = @event.EventName;

                this._db.Events.Update(dbEvent);
                this._db.SaveChanges();
            }


            

            return Ok(@event); // should actually return the created status code 201
        }

        [HttpPut("")]
        public IActionResult Put(Event @event)
        {   
            return Post(@event); // should actually return the updated status code 204
        }

        [HttpDelete("")]
        public IActionResult Delete(Event[] @events)
        {
            Event dbEvent = null;

            foreach (Event e in events)
            {
                dbEvent =  this._db.Events.Find(e.EventID);
                if (dbEvent != null)
                    this._db.Events.Remove(dbEvent);
            }

            this._db.SaveChanges();

            return NoContent(); // should actually return the updated status code 204
        }
    }
}
