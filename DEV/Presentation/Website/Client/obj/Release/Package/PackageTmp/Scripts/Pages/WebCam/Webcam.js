//This is common implementation for web cam in web app
//Using https://github.com/bensonruan/webcam-easy

var UseWebcam = {
    //Jquery object
    webCamElement: null,
    //Jquery object
    canvasElement: null,
    //Jquery object
    snapSound: null,
    //WebCam.js Object For Futher Operations Like Snap ,Start,Stop ETC>
    webCamApi: null,
    //Initionlize Camera.
    InitWebcam: function ($webCamobj, $canvasObj, $snapSound) {
        webCamElement = $webCamobj;
        canvasElement = $canvasObj;
        snapSound = $snapSound;
        return webCamApi = new Webcam(webCamElement.get(0), 'enviroment', canvasElement.get(0), snapSound.get(0));
    },
    //This Function Will Start Camera
    StartCamera: function (sucessCallBackFunction, errorCallBackFunction) {
        webCamApi.start()
            .then(result => {
                if ($.isFunction(sucessCallBackFunction))
                    sucessCallBackFunction();
                console.log("webcam started");
            })
            .catch(err => {
                if ($.isFunction(errorCallBackFunction))
                    errorCallBackFunction();
            });
    },
    //This Function Will Stop Camera
    StopCamera: function () {
        webCamApi.stop();
    },
    //This Function Will Resume Camera After Snap.
    ResumeCamera: function (successCallBackFunction) {
        webCamApi.stream()
            .then(facingMode => {
                if ($.isFunction(successCallBackFunction))
                    successCallBackFunction();
            });
    },
    //Take Snap it Will Return Base64 of the png image.
    Snap: function () {
        return webCamApi.snap();
    },
    //Change Camera Front To Rear vise Versa.
    FlipCamera: function () {
        webCamApi.flip();
        webCamApi.start();
    },
    //Return No OF Camera's.
    CameraCounts: function () {
        return webCamApi.webcamList.length;
    }
};