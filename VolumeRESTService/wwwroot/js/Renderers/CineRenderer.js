
function CineRenderer(cineViewModel, canvas) {
    var self = this;

    self.CineViewModel = cineViewModel;
    self.Canvas = canvas;

    // Mouse interactions should really be separate classes that can be aggragated with renderers to create views.
    // This will do for now, though, since there's only one renderer and 2 interactions
    self.CineMinMouseYDelta = self.Canvas.height / 100;
    self.WLWWMinMouseXDelta = self.Canvas.width / 1000;
    self.WLWWMinMouseYDelta = self.Canvas.height / 1000;
    self.CinePressedMouseYPos = null;
    self.CinePressedSliceIndex = 0;
    self.WLWWLastMouseXPos = null;
    self.WLWWLastMouseYPos = null;
    self.WLWWLastWindowWidth = null;
    self.WLWWLastWindowLevel = null;

    self.Canvas.oncontextmenu = function (event) {
        return false;
    }

    self.Canvas.onmousedown = function (event) {
        if (event.buttons == 2) {
            self.CinePressedMouseYPos = event.layerY;
            self.CinePressedSliceIndex = self.CineViewModel.CurrentSliceIndex();
        }

        if (event.buttons == 3) {
            self.WLWWLastMouseXPos = event.layerX;
            self.WLWWLastMouseYPos = event.layerY;
            self.WLWWLastWindowWidth = self.CineViewModel.WindowWidth();
            self.WLWWLastWindowLevel = self.CineViewModel.WindowLevel();
        }
    }

    self.Canvas.onmousemove = function (event) {
        if (event.buttons == 0)
            return;

        if (event.buttons == 2) {
            if (Math.abs(self.CinePressedMouseYPos - event.layerY) >= self.CineMinMouseYDelta) {
                var delta = self.CinePressedMouseYPos - event.layerY;
                var sliceDelta = Math.round(delta / self.CineMinMouseYDelta);

                self.CineViewModel.ChangeCurrentSliceIndex(self.CinePressedSliceIndex - sliceDelta);
            }

            return false;
        }

        if (event.buttons == 3) {
            if (Math.abs(self.WLWWLastMouseXPos - event.layerX) >= self.WLWWMinMouseXDelta) {
                var delta = self.WLWWLastMouseXPos - event.layerX;

                var wlDelta = Math.round(delta / self.WLWWMinMouseXDelta);

                self.CineViewModel.WindowWidth(self.WLWWLastWindowWidth - wlDelta);
            }

            if (Math.abs(self.WLWWLastMouseYPos - event.layerY) >= self.WLWWMinMouseYDelta) {
                var delta = self.WLWWLastMouseYPos - event.layerY;

                var wwDelta = Math.round(delta / self.WLWWMinMouseYDelta);

                self.CineViewModel.WindowLevel(self.WLWWLastWindowLevel - wwDelta);
            }

            return false;
        }
    }

    // Renders a slice onto the canvas.
    // TODO:
    //      Make allowances for voxel shapes
    //      Allow for irregular slice dimensions (currently only allows square slices)
    //      Proper scaling of image - current method is very unsophisticated
    //      Plenyty of other stuff I have undoubtedly forgotten...
    self.Draw = function () {
        var context = self.Canvas.getContext("2d");

        var canvasWidth = 450;
        var canvasHeight = 450;

        var slice = self.CineViewModel.CurrentSlice();
        var sliceData = slice.radiodensities;

        if (sliceData == null || sliceData.length == 0) {
            context.fillStyle = "#000000";
            context.fillRect(0, 0, canvasWidth, canvasHeight);

            return;
        }


        var imageData = context.createImageData(canvasWidth, canvasHeight);
        var data = imageData.data;

        var windowLevel = parseInt(self.CineViewModel.WindowLevel());
        var windowWidth = parseInt(self.CineViewModel.WindowWidth());

        for (var x = 0; x < canvasWidth; x++) {
            for (var y = 0; y < canvasHeight; y++) {
                // Get voxel data
                var sliceX = Math.round((slice.width / canvasWidth) * x);
                var sliceY = Math.round((slice.height / canvasHeight) * y);
                var sliceIndex = (sliceY * slice.height) + sliceX;

                // convert voxel to RGB via WLWW
                var rgbArray = VoxelToPixel(sliceData[sliceIndex], windowLevel, windowWidth);

                var index = ((y * canvasHeight) + x) * 4; // data is a byte array of RGBA values

                data[index] = rgbArray[0];
                data[index + 1] = rgbArray[1];
                data[index + 2] = rgbArray[2];
                data[index + 3] = 255; // always at 255 - no need for lower alpha
            }
        }

        context.putImageData(imageData, 0, 0);
    }

    self.CineViewModel.Volume.subscribe(self.Draw);
    self.CineViewModel.WindowWidth.subscribe(self.Draw);
    self.CineViewModel.WindowLevel.subscribe(self.Draw);
    self.CineViewModel.CurrentSlice.subscribe(self.Draw);

    self.Draw();
}

function VoxelToPixel(voxelValueHU, windowLevel, windowWidth) {
    var halfWidth = Math.round(windowWidth / 2);
    var lowerBound = windowLevel - halfWidth;
    var upperBound = windowLevel + halfWidth;

    if (voxelValueHU <= lowerBound)
        return [0, 0, 0];

    if (voxelValueHU >= upperBound)
        return [255, 255, 255];

    var difference = upperBound - lowerBound;
    var scale = Math.round(difference / 256);

    var val = Math.round((voxelValueHU - lowerBound) / scale);

    return [val, val, val];
}