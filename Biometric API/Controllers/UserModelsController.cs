using Biometric_API.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Biometric_API.Controllers
{
    public class UserModelsController : ApiController
    {
        private Biometric_APIContext db = new Biometric_APIContext();

        //TODO: faster processing unsafe code fazon
        [Route("api/match")]
        public IHttpActionResult performMatching(string path)
        {
            Bitmap imgOrig = new Bitmap(Image.FromFile(path), 100, 50);
            List<string> imgPaths = db.Database.SqlQuery<string>("SELECT data FROM biometricdatamodels").ToList();
            foreach (string imgPath in imgPaths)
            {
                Bitmap img2compare = new Bitmap(imgPath);
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        if (imgOrig.GetPixel(i, j) != img2compare.GetPixel(i, j))
                            return NotFound();
                    }
                }
            }
            return Ok();
        }

        // GET: api/UserModels
        public IQueryable<UserModels> GetUserModels()
        {
            return db.UserModels;
        }

        // GET: api/UserModels/5
        [ResponseType(typeof(UserModels))]
        public async Task<IHttpActionResult> GetUserModels(int id)
        {
            UserModels userModels = await db.UserModels.FindAsync(id);
            if (userModels == null)
            {
                return NotFound();
            }

            return Ok(userModels);
        }

        // PUT: api/UserModels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserModels(int id, UserModels userModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModels.Id)
            {
                return BadRequest();
            }

            db.Entry(userModels).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelsExists(id))
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

        // POST: api/UserModels
        [ResponseType(typeof(UserModels))]
        public async Task<IHttpActionResult> PostUserModels(UserModels userModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserModels.Add(userModels);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userModels.Id }, userModels);
        }

        // DELETE: api/UserModels/5
        [ResponseType(typeof(UserModels))]
        public async Task<IHttpActionResult> DeleteUserModels(int id)
        {
            UserModels userModels = await db.UserModels.FindAsync(id);
            if (userModels == null)
            {
                return NotFound();
            }

            db.UserModels.Remove(userModels);
            await db.SaveChangesAsync();

            return Ok(userModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserModelsExists(int id)
        {
            return db.UserModels.Count(e => e.Id == id) > 0;
        }
    }
}