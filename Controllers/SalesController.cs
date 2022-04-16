#nullable disable
using AutoMapper;
using CommerceApi.DatabaseContext;
using CommerceApi.Entities;
using CommerceApi.Models;
using CommerceApi.Models.UserModels;
using Imagekit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Claims;

namespace CommerceApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private NameValueCollection _configuration= System.Configuration.ConfigurationManager.AppSettings;
        private readonly DataBaseContext _context;
        private readonly ServerImagekit imageKit;
        private readonly IMapper _mapper;
        

        public SalesController(DataBaseContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            imageKit = new ServerImagekit(_configuration["ApiPublicKey"], _configuration["ApiPrivateKey"], _configuration["ImagekitEndpoint"], "path");
        }

        // GET: /Sales
        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> Getsales()
        {
            
            var y = await _context.userClass.ToListAsync();
            var x = await _context.sales
                .Include(sales => sales.UserClass)
                .Include(sales => sales.Images)
                .ToListAsync();
            y.ForEach(y1 =>
            {
                y1.SalesListId = new List<int>();
                y1.SalesListId.AddRange(from sale in x where sale.UserClassID == y1.Id select sale.Id);
            });
            return Ok(x);
        }
        //[AllowAnonymous]
        // GET: /Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _context.sales
                .Include(sales => sales.UserClass)
                .Include(sales => sales.Images)

                .FirstOrDefaultAsync(sale => sale.Id == id);


            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }
        [Authorize(Policy = "ApiScopeAuthenticated")]
        // PUT: /Sales/5 ####To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, EditSale editsale)
        {
            if (id != editsale.Id)
            {
                return BadRequest();
            }
            if (!SaleExists(id))
            {
                return NotFound();
            }
            Sale sale = await _context.sales
                .Include(saleP => saleP.Images)
                .FirstOrDefaultAsync(saleP => saleP.Id == id);
            _mapper.Map(editsale,sale);
            sale.UpdatedDate = DateTime.Now;
            _context.Entry(sale).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return BadRequest();

            }
            return NoContent();
        }
        // POST: /Sales #### To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "ApiScopeAuthenticated")]
        [HttpPost]
        public async Task<ActionResult<NewSale>> PostSale(NewSale newSale)
        {
            Sale sale = new Sale();
            
            sale.Images = new List<Images>();
            sale=_mapper.Map<NewSale, Sale>(newSale);
            foreach (Images y in sale.Images)
            {
                y.SaleId=sale.Id;
            }
            sale.UserClassID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            sale.IsDeleted = false;            
            sale.ImagesDeleteError = false;
            sale.CreatedDate = DateTime.Now;
            var x =await _context.userClass.FindAsync(sale.UserClassID);
            if (x == null)
            {
                bool i1=int.TryParse(User.FindFirst("PhoneNumber").Value,out int i);
                var userClass = new UserClass { Id = sale.UserClassID,PhoneNumber=i1?i:0,RegistredTime=DateTime.Parse(User.FindFirst("RegisteredTime").Value),UserName=User.FindFirst("UserName").Value };
                _context.userClass.Add(userClass);
            }
            _context.sales.Add(sale);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        }
        [Authorize(Policy = "ApiScopeAuthenticated")]
        // DELETE: /Sales/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int id)
        {
            var sale = await _context.sales
                .Include(sales => sales.UserClass)
                .Include(sales=>sales.Images)
                .FirstOrDefaultAsync(sale => sale.Id == id);
            if (sale == null)
            {
                return NotFound();
            }
            sale.IsDeleted = true;
            if (! (await DeleteImages(sale.Images)))
            {
                sale.ImagesDeleteError=true;
                
            }
            _context.sales.Remove(sale);
            //_context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();


        }

        private async Task<bool> DeleteImages(List<Images> imagesToDelete)
        {
            foreach (Images img in imagesToDelete)
            {
                DeleteAPIResponse deleteResp = await imageKit.DeleteFileAsync(img.Id);
                if (deleteResp.StatusCode > 299)
                {
                    return false;
                }
                
            }
            return true;
        }
        //ImageKit Authentication Parameter Generation endpoint api/Sales/Auth
        [Authorize(Policy = "ApiScopeAuthenticated")]
        [HttpGet("Auth/{expire}/{token}")]
        public async Task<AuthParamResponse> GetAuth(long expire, string token)
        {
            return imageKit.GetAuthenticationParameters(token, expire.ToString());
        }
        private bool SaleExists(int id)
        {
            return _context.sales.Any(e => e.Id == id);
        }
        //GET user.claims /Sales/claims
        [HttpGet("claims")]
        [Authorize(Policy = "ApiScopeAuthenticated")]
        public ActionResult GetClaims()
        {
            var currentUser=new RegisterModel();
            currentUser.Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentUser.Email = User.FindFirst(ClaimTypes.Email).Value;
            currentUser.UserName = User.FindFirstValue("UserName");
            currentUser.PhoneNumber = int.Parse(User.FindFirstValue("PhoneNumber"));
            return Ok(currentUser);
        }
    }
}
