using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Dtos;
using NomoBucket.API.Helpers;
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
public async Task<IActionResult> getFeed([FromQuery]FeedParams feedParams)
    {
      var feed = await _feedRepo.GetFeed(feedParams);
      if (feed == null)
      {
        throw new System.Exception("Could not retrieve feed");
      }
      var mappedFeed =  _mapper.Map<IEnumerable<FeedItem>>(feed);
      Response.AddPagination(feed.CurrentPage, feed.PageSize, feed.TotalCount, feed.TotalPages);
      return Ok(mappedFeed);
    }
  }
}