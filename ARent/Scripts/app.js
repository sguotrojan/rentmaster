var geocoder = new google.maps.Geocoder();
var map;
var markers = [];
var defaultImageURL = '../Content/images/no_image.jpg';

var DetailsPaneViewModel = {
    pictures : ko.observableArray([
        {
            PictureUrl: defaultImageURL,
        }
    ]),
    monthlyPrice: ko.observable(0),
    deposit: ko.observable(0),
    contact: ko.observable(""),
    phone: ko.observable(""),
    email: ko.observable(""),
    notes: ko.observable(""),
    address:ko.observable("")
}

function updatePaneViewModel(details) {
    var pictures = [];
    DetailsPaneViewModel.monthlyPrice(details.MonthlyPrice);
    DetailsPaneViewModel.deposit(details.Deposit);
    DetailsPaneViewModel.contact(details.Contact.ContactName);
    DetailsPaneViewModel.email(details.Contact.Email);
    DetailsPaneViewModel.phone(details.Contact.PhoneNumber);
    DetailsPaneViewModel.notes(details.Notes);
    if (details.PictureUrls == null || details.PictureUrls.length == 0) {
        pictures.push({
            'PictureUrl': defaultImageURL
        });
    }
    else {
        for (var i = 0; i < details.PictureUrls.length; i++) {
            pictures.push({
                'PictureUrl': details.PictureUrls[i]
            });
        }
    }
    DetailsPaneViewModel.pictures(pictures);
    DetailsPaneViewModel.address(details.Address.Line1 + ',' + details.Address.City + ',' + details.Address.State + ',' + details.Address.Zip);
}

var searchByZip = function getAddressInfo(zipcode) {
    clearAllMarkers();
    var postData = { 'zip': zipcode };
    $.ajax(
        {
            url: "/home/SearchByZipCode",
            type: "POST",
            data: JSON.stringify(postData),
            contentType: "application/json",
            async: true,
            dataType: "JSON"
        })
        .done(function (returnValue) {
            if (returnValue == null) return;
            for (var i = 0; i < returnValue.length; i++) {
                var housePosition = new google.maps.LatLng(returnValue[i].Latitude, returnValue[i].Longitude);
                var marker = new google.maps.Marker({
                    position: housePosition,
                    icon: '../Content/images/houseIcon.jpg'
                });
                markers.push(marker);
                initializeRentWindow(marker, returnValue[i]);
            }
            setMarksMap(map);
            $(document).trigger("LoadHouseSuccess");
        });
}
function setMarksMap(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}
function clearAllMarkers() {
    setMarksMap(null);
    markers = [];
}
function searchHouse() {
    var address = document.getElementById('address').value;
    this.searchByZip(address);
    $(document).on("LoadHouseSuccess", function (event) {
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    });
}

function initializeRentWindow(marker, details) {
    google.maps.event.addListener(marker, 'click', function () {
        openPane();
        updatePaneViewModel(details);
    });
}

function initialize() {
    var myLatlng = new google.maps.LatLng(47.6519744, -122.19210509999998);
    var mapOptions = {
        zoom: 15,
        center: myLatlng,
        streetViewControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        zoomControl: true
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    ko.applyBindings(DetailsPaneViewModel, $("#houseDetail")[0]);
    this.closePane();
}

function closePane() {
    $("#map-canvas").addClass("col-md-12");
    $("#map-canvas").removeClass("col-md-9");
}
function openPane() {
    $("#map-canvas").addClass("col-md-9");
    $("#map-canvas").removeClass("col-md-12");
}
google.maps.event.addDomListener(window, 'load', initialize);