using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Dtos;
using NomoBucket.API.Models;

namespace NomoBucket.API.Controllers
{
  [ApiController]
  [Route("/api/[controller]")]
  public class FeedController : ControllerBase
  {
    private readonly IFeedRepository _feedRepo;
    private readonly IMapper _mapper;
    public FeedController(IFeedRepository feedRepo, IMapper mapper)
    {
      _mapper = mapper;
      _feedRepo = feedRepo;

    }
    [HttpGet]
    public async Task<IActionResult> getFeed()
    {
      var feed = await _feedRepo.GetFeed();
      if (feed == null)
      {
        throw new System.Exception("Could not retrieve feed");
      }
      var mappedFeed =  _mapper.Map<IEnumerable<FeedItem>>(feed);
      return Ok(mappedFeed);
    }
  }
}