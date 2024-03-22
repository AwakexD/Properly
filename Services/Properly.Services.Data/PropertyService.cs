namespace Properly.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Listing> listingRepository;

        public PropertyService(
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository)
        {
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
        }

        public async Task<string> CreateListingAsync(CreateListingViewModel form, string userId)
        {
            var address = this.mapper.Map<Address>(form.Address);
            var property = this.mapper.Map<Property>(form.Property);
            property.Address = address;

            var listing = this.mapper.Map<Listing>(form.Listing);
            listing.Property = property;
            listing.ListingStatusId = 1;
            listing.CreatorId = userId;

            foreach (var photo in form.UploadedPhotos)
            {
                string url = await this.cloudinaryService.UploadImageAsync(photo);
                listing.Photos.Add(new Photo() { Url = url });
            }

            await this.listingRepository.AddAsync(listing);
            await this.listingRepository.SaveChangesAsync();

            return listing.Id.ToString();
        }

        public async Task<IEnumerable<ListingIndexViewModel>> GetAllListingsByAddedDate(int count)
        {
            var listings = await this.listingRepository.AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Include(l => l.Property.Address)
                .Include(l => l.Photos)
                .Take(count)
                .ToListAsync();

            var listingViewModels = this.mapper.Map<IEnumerable<ListingIndexViewModel>>(listings);

            return listingViewModels;
        }

        public async Task<IEnumerable<ListingInListViewModel>> GetAll(int page, string type, int itemsPerPage = 6)
        {
            var listings = await this.listingRepository.AllAsNoTracking()
                .OrderByDescending(l => l.CreatedOn)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Where(l => l.ListingType.Name == type)
                .To<ListingInListViewModel>().ToListAsync();

            return listings;
        }

        public int GetCount(string listingType)
        {
             int count = this.listingRepository
                .AllAsNoTracking()
                .Count(l => l.ListingType.Name == listingType);

             return count;
        }
    }
}
