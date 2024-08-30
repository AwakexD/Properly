import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    const deleteButtons = document.querySelectorAll('.delete-button i.fa-trash');
    const listingId = document.querySelector('.listingId').value;
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]").value;

    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    // Handle newly uploaded images.
    document.querySelector('.ip-file').addEventListener('change', function (event) {
        const fileInput = event.target;
        const fileList = Array.from(fileInput.files);
        let displayImagesElement = document.querySelector('#display-uploaded-images');

        if (!displayImagesElement) {
            displayImagesElement = document.createElement('div');
            displayImagesElement.id = 'display-uploaded-images';
            displayImagesElement.className = 'd-flex flex-wrap justify-content-center align-items-center';

            const imagesWidgetDiv = document.querySelector('#images-widget');

            if (imagesWidgetDiv) {
                imagesWidgetDiv.insertBefore(displayImagesElement, document.querySelector('.box-uploadfile'));
            }
        }
        const fileMap = new Map();

        fileList.forEach((file, index) => {
            if (file.type.startsWith('image/')) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    const cardDiv = document.createElement('div');
                    cardDiv.className = 'card m-2 position-relative';
                    cardDiv.style.width = '18rem';

                    const img = document.createElement('img');
                    img.src = e.target.result;
                    img.className = 'card-img-top';

                    const deleteButton = document.createElement('a');
                    deleteButton.href = '#';
                    deleteButton.className = 'delete-button position-absolute top-0 end-0 m-2 text-danger';
                    deleteButton.title = 'Delete Image';

                    const deleteIcon = document.createElement('i');
                    deleteIcon.className = 'fa-solid fa-trash';
                    deleteIcon.style.color = '#ed2027';
                    deleteIcon.style.fontSize = '1.2rem';

                    deleteButton.appendChild(deleteIcon);

                    deleteButton.addEventListener('click', function (e) {
                        e.preventDefault();
                        cardDiv.remove();
                        fileMap.delete(index);

                        const dataTransfer = new DataTransfer();
                        fileMap.forEach(file => dataTransfer.items.add(file));
                        fileInput.files = dataTransfer.files; 
                    });

                    cardDiv.appendChild(img);
                    cardDiv.appendChild(deleteButton);

                    displayImagesElement.appendChild(cardDiv);

                    fileMap.set(index, file);
                };

                reader.readAsDataURL(file);
            }
        });
    });

    // Handle already uploaded images.
    const handleButtonClick = async (event) => {
        event.preventDefault();

        if (!listingId) {
            console.error("Listing ID not found.");
            return;
        }

        const cardElement = event.target.closest('.card');
        const imageUrl = event.target.closest('.card').querySelector('.card-img-top').src;

        const swalAlert = await Swal.fire({
            title: 'Are you sure you want to delete this photo?',
            text: 'If you confirm, the photo will be permanently deleted.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Cancel'
        });

        if (swalAlert.isConfirmed) {
            const data = {
                ListingId: listingId,
                ImageUrl: imageUrl,
            };
    
            try {
                const response = await post('/api/PropertyInteractions/DeleteImage', data, antiForgeryToken);

                if (response.success) {
                    cardElement.style.display = 'none';
                }
            } catch (error) {
                console.error("Error sending POST request.", error);
            }
        }

    }

    deleteButtons.forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });
});