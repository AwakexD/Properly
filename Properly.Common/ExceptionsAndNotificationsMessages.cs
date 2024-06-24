using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Properly.Common
{
    public static class ExceptionsAndNotificationsMessages
    {
        // General Errors
        public const string AnErrorOccurred = "An error occurred. Please try again later.";
        public const string InvalidDataProvidedError = "The provided data is invalid!";
        public const string EntityAlreadyExists = "The entity you are trying to create already exists!";
        public const string InvalidDataSubmitted = "Invalid data submitted, please re-upload the images and correct the erroneous fields.";
        public const string ErrorCreatingEntity = "An error occurred while creating the entity, please try again.";

        // Listing Errors
        public const string ListingDoesNotExistError = "The listing does not exist.";
        public const string ErrorCreatingTheListing = "An error occurred while creating the listing, please re-upload the images and try again.";
        public const string ListingIsAlreadyInCurrentUserFavorites = "The listing is already in your favorites!";
        public const string ListingIsNotInCurrentUserFavorites = "The listing is not in your favorites!";
        public const string ListingUpdateUnsuccessful = "An error occurred while updating the listing, please try again.";

        // Listing Notifications
        public const string ListingCreatedSuccessfully = "The listing was created successfully!";
        public const string ListingWasUpdatedSuccessfully = "The listing was updated successfully.";
        public const string ListingRemovedFromFavorites = "The listing was removed from your favorites.";
        public const string ListingDeactivated = "The listing was hidden and deactivated! You can reactivate it or delete it.";
        public const string ListingReactivated = "The listing was reactivated and is now public.";
        public const string ListingPermanentlyDeleted = "The listing was permanently deleted.";

        // Image Errors
        public const string ImageDoesNotExistError = "The image does not exist.";
        public const string ImageDeleteUnsuccessful = "Image deletion was unsuccessful!";
        public const string ImageUploadUnsuccessful = "Image upload was unsuccessful! Please try again.";
        public const string ImagesUploadUnsuccessful = "Images upload was unsuccessful! Please try again.";
        public const string InvalidImageForThumbnailProvided = "The image you selected for the listing thumbnail does not exist!";
        public const string UploadedImagesAreInvalid = "You can only upload images up to 10 MB!";

        // Image Notifications
        public const string ImageDeletedSuccessfully = "The image was deleted successfully!";
        public const string ThumbnailSelectedSuccessfully = "The thumbnail was selected successfully.";

        // Auction Errors
        public const string AuctionDoesNotExistError = "The auction does not exist.";
        public const string CannotDeactivateAuction = "You cannot deactivate an auction that has already started or has bids!";
        public const string AuctionUpdateUnsuccessful = "An error occurred while updating the auction, please try again.";
        public const string ErrorCreatingTheAuction = "Failed to create the auction. Please try again later or contact support.";
        public const string ErrorEditingTheAuction = "Failed to edit the auction. Please try again later or contact support.";
        public const string AuctionHasAlreadyStarted = "You do not have the permissions to change the auction after it has started or ended.";
        public const string CannotEditActiveAuctionWithBids = "You do not have the permissions to edit auctions that have bids!";

        // Auction Notifications
        public const string AuctionDeactivated = "The auction was deactivated.";
        public const string AuctionReactivated = "The auction was reactivated. Please refresh the auction details!";
        public const string AuctionDeletedSuccessfully = "The auction was permanently deleted!";
        public const string AuctionCreateSuccessfully = "The auction was created successfully.";
        public const string AuctionWasUpdatedSuccessfully = "The auction was updated successfully. Select a cover image.";
        public const string AuctionIsAlreadyInCurrentUserFavorites = "The auction is already in your favorites!";
        public const string AuctionRemovedFromFavorites = "The auction was removed from your favorites.";

        // User Account Notifications
        public const string LogoutSuccessful = "You have successfully logged out.";
        public const string LoginSuccessful = "You have successfully logged in.";
        public const string RegistrationSuccessful = "You have successfully registered!";
        public const string UnsuccessfulRegistration = "An error occurred during registration, please try again later.";
    }
}
