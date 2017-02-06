
angular.module('nfsolidariaApp.services')
    .factory('CameraService', CameraService);

CameraService.$inject = ['$cordovaCamera']

function CameraService($cordovaCamera) {

    return {
        getPictureOptions: getPictureOptions
    };

    function getPictureOptions(sourceType) {
        return {
            quality: 90,
            destinationType: Camera.DestinationType.DATA_URL,
            sourceType: sourceType,
            encodingType: Camera.EncodingType.PNG,
            targetWidth: 800,
            targetHeight: 800,
            popoverOptions: CameraPopoverOptions,
            saveToPhotoAlbum: true,
            correctOrientation: true
        };
    };
};