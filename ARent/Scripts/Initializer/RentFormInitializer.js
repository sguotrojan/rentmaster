var geocoder = new google.maps.Geocoder();
function getGeoLocation(address) {
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            $(document).trigger("GeoLocationEvent", [results[0].geometry.location]);
        } else {
            alert('Not a valid address');
            $(document).trigger("GeoLocationEvent");
        }
    })
}
$(function () {
    $('#formvalidation').click(function () {
        var address = $('#Address').val() + ',' + $('#City').val() + ',' + $('#State').val() + ',' + $('#Zip').val();
        getGeoLocation(address);
        $(document).on("GeoLocationEvent", function (event, location) {
            if (location == null) return;
            document.getElementById('Latitude').value = location.lat();
            document.getElementById('Longitude').value = location.lng();
            document.getElementById('Notes').value = $('#Notes').val();
            $('#formSubmit').removeAttr('disabled')
            $('#formSubmit').click();
        })
    });
});

$(document).ready(function () {
    $("#Houseimages").fileinput({
        'showUpload': false,
        'previewFileType': 'any',
        maxFileSize: 3000,
        allowedFileExtensions: ["jpg", "gif", "png"]
    });
    $('.registerForm').bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            ContactName: {
                message: 'The contact name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The contactName is required and cannot be empty'
                    },
                    stringLength: {
                        min: 1,
                        max: 50,
                        message: 'The contactName can not exceed 50 characters'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The contactName can only consist of alphabetical, number and underscore'
                    }
                }
            },
            PhoneNumber: {
                message: 'The phone number is not valid',
                validators: {
                    notEmpty: {
                        message: 'The phone number is required and cannot be empty'
                    },
                    stringLength: {
                        min: 1,
                        max: 20,
                        message: 'The phone number can not exceed 20 characters'
                    },
                    regexp: {
                        regexp: /^[0-9-()]+$/,
                        message: 'The phone number can only consist of number'
                    }
                }
            },
            Email: {
                validators: {
                    notEmpty: {
                        message: 'The email is required and cannot be empty'
                    },                    
                    emailAddress: {
                        message: 'The value is not a valid email address'
                    }
                }
            },
            Address: {
                validators: {
                    notEmpty: {
                        message: 'The address is required and cannot be empty'
                    },
                    stringLength: {
                        min: 1,
                        max: 200,
                        message: 'The address can not exceed 200 characters'
                    },
                }
            },
            Zip: {
                validators: {
                    notEmpty: {
                        message: 'The zip code is required and cannot be empty'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'zip code can only be number'
                    }
                }
            },
            City: {
                validators: {
                    notEmpty: {
                        message: 'The city is required and cannot be empty'
                    },                   
                }
            },
            State: {
                validators: {
                    notEmpty: {
                        message: 'The state is required and cannot be empty'
                    },
                }
            },
            Rent: {
                validators: {
                    notEmpty: {
                        message: 'The rent is required and cannot be empty'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Rent can only be number'
                    },
                    between: {
                        min: 0,
                        max: 100000,
                        message: 'The rent must be between 0 and 100000.0'
                    }
                }
            },
            Deposit: {
                validators: {                    
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Deposit can only be number'
                    },
                    between: {
                        min: 0,
                        max: 100000,
                        message: 'The deposit must be between 0 and 100000.0'
                    }
                }
            }
        }   
    });
});