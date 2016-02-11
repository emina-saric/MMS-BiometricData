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
using BitmapProcessing;

namespace Biometric_API.Controllers
{
    public class UserModelsController : ApiController
    {
        private Biometric_APIContext db = new Biometric_APIContext();

        [Route("api/match")]
        [ResponseType(typeof(int))]
        unsafe public IHttpActionResult performMatching(string path)
        {
            Bitmap img = new Bitmap(Image.FromFile(path), 100, 50);
            FastBitmap imgOrig = new FastBitmap(img);
            List<BiometricDataModels> biometricData = db.BiometricDataModels.ToList();
            imgOrig.LockImage();
            foreach (BiometricDataModels model in biometricData)
            {
                Bitmap img2 = new Bitmap(model.Data);
                FastBitmap img2compare = new FastBitmap(img2);
                bool mismatch = false;
                float mismatchLvl = 0, minMismatch = 500;
                int userID = -1;
                img2compare.LockImage();                
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        if (imgOrig.GetPixel(i, j) != img2compare.GetPixel(i, j))
                        {
                            mismatchLvl++;
                            if (mismatchLvl < minMismatch)
                            {
                                minMismatch = mismatchLvl;
                                userID = model.UserId;
                            }                                
                            
                            if (mismatchLvl > 500)
                            {
                                mismatch = true;
                                break;
                            }
                        }
                    }
                    if (mismatch)
                        break;
                }
                img2compare.UnlockImage();
                img2.Dispose();
                if (userID != -1)
                    return Ok(userID);
            }
            imgOrig.UnlockImage();
            img.Dispose();
            return NotFound();

            //List<BiometricDataModels> biometricData = db.BiometricDataModels.ToList();
            //using (MagickImage img = new MagickImage(new Bitmap(path)))
            //{
            //    foreach (BiometricDataModels model in biometricData)
            //    {
            //        using (MagickImage img2compare = new MagickImage(new Bitmap(model.Data)))
            //        {
            //            MagickErrorInfo compInfo = img.Compare(img2compare);
            //            img.Compare(img2compare, ErrorMetric.Undefined)
            //        }
            //    }
            //}
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