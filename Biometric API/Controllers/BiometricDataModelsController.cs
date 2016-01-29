using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Biometric_API.Models;

namespace Biometric_API.Controllers
{
    public class BiometricDataModelsController : ApiController
    {
        private Biometric_APIContext db = new Biometric_APIContext();

        // GET: api/BiometricDataModels
        public IQueryable<BiometricDataModels> GetBiometricDataModels()
        {
            return db.BiometricDataModels;
        }

        // GET: api/BiometricDataModels/5
        [ResponseType(typeof(BiometricDataModels))]
        public async Task<IHttpActionResult> GetBiometricDataModels(int id)
        {
            BiometricDataModels biometricDataModels = await db.BiometricDataModels.FindAsync(id);
            if (biometricDataModels == null)
            {
                return NotFound();
            }

            return Ok(biometricDataModels);
        }

        // PUT: api/BiometricDataModels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBiometricDataModels(int id, BiometricDataModels biometricDataModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != biometricDataModels.Id)
            {
                return BadRequest();
            }

            db.Entry(biometricDataModels).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BiometricDataModelsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BiometricDataModels
        [ResponseType(typeof(BiometricDataModels))]
        public async Task<IHttpActionResult> PostBiometricDataModels(BiometricDataModels biometricDataModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BiometricDataModels.Add(biometricDataModels);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = biometricDataModels.Id }, biometricDataModels);
        }

        // DELETE: api/BiometricDataModels/5
        [ResponseType(typeof(BiometricDataModels))]
        public async Task<IHttpActionResult> DeleteBiometricDataModels(int id)
        {
            BiometricDataModels biometricDataModels = await db.BiometricDataModels.FindAsync(id);
            if (biometricDataModels == null)
            {
                return NotFound();
            }

            db.BiometricDataModels.Remove(biometricDataModels);
            await db.SaveChangesAsync();

            return Ok(biometricDataModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BiometricDataModelsExists(int id)
        {
            return db.BiometricDataModels.Count(e => e.Id == id) > 0;
        }
    }
}