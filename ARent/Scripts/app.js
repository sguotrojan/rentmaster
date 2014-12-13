var geocoder = new google.maps.Geocoder();
var map;
var markers = [];

var DetailsPaneViewModel = {
    pictures : ko.observableArray([
        { PictureUrl: 'http://ts2.mm.bing.net/th?id=HN.608049700277649466&pid=1.7' }
    ]),
    monthlyPrice: ko.observable(2100),
    deposit: ko.observable(2200),
    contact: ko.observable("Sheng Guo"),
    phone: ko.observable("2134770379"),
    email: ko.observable("sguotrojan@gmail.com"),
    notes: ko.observable("Nice apartment, worth it. You really need to take a look. Close to Microsoft campus"),
}

function updatePaneViewModel(details) {
    var pictures = [];
    DetailsPaneViewModel.monthlyPrice(details.MonthlyPrice);
    DetailsPaneViewModel.deposit(details.Deposit);
    DetailsPaneViewModel.contact(details.Contact.ContactName);
    DetailsPaneViewModel.email(details.Contact.Email);
    DetailsPaneViewModel.notes(details.Notes);
    if (details.PictureUrls == null || details.PictureUrls.length == 0) {
        pictures.push({ 'PictureUrl': 'http://ts1.mm.bing.net/th?&id=HN.608014906746734218&w=300&h=300&c=0&pid=1.9&rs=0&p=0' });
    }
    else {
        for (var i = 0; i < details.PictureUrls.length; i++) {
            pictures.push({ 'PictureUrl': details.PictureUrls[i] });
        }
    }
    DetailsPaneViewModel.pictures(pictures);
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
                    position: housePosition
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
        $("#houseDetail").slideUp();
        updatePaneViewModel(details);
        $("#houseDetail").slideDown();
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
}
google.maps.event.addDomListener(window, 'load', initialize);