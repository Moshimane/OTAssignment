
namespace OT.Assessment.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerManager;

        public PlayerController(IPlayerService playerManager) 
        {
            _playerManager = playerManager;
        }

        //POST api/player/casinowager
        [HttpPost]
        [Route("casinowager")]
        public IActionResult PostCasinoWager(CasinoWager message)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _playerManager.AddCasinoWager(message);
            return Ok("Message sent");
        }


        //GET api/player/{playerId}/wagers
        [HttpGet]
        [Route("{playerId}/casino")]
        public async Task<IActionResult> GetLatestPlayerWagers(string playerId, [FromQuery] int pageSize, [FromQuery] int page)
        {
            var requestedPage = await _playerManager.GetLatestPlayerWagersAsync(playerId, pageSize, page);
            return Ok(requestedPage);
        }

        //GET api/player/topSpenders?count=10
        [HttpGet]
        [Route("topSpenders")]
        public async Task<IActionResult> GetTopSpenders([FromQuery] int count)
        {
            var topSpenders = await _playerManager.TopSpendersAsync(count);
            return Ok(topSpenders);
        }
    }
}