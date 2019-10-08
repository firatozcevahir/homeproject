
var connection = new signalR.HubConnectionBuilder().withUrl("/lighthub").build();
var $console = $("#dvConsole");

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

$(connection).bind("onDisconnect", function (e, data) {
    callback.call(connection);
});

var $selectedBtn = null;
$(document).ready(function () {
    $('.btn-light').click(btnLightClick);
    $('#btnCommandText').click(btnCommandClick);
});

//Update the Light Status
function btnLightClick() {
    executeCommand($(this).data("item-commandtext"), 'Index');
}
function btnCommandClick() {
    executeCommand($('#txtCommand').val(), 'Console');
}

function clearConsole() {
    $($console).children().fadeOut(300).promise().then(function () {
        $($console).empty();
    });
}

function executeCommand(senderCommandText, basePage) {
    var commandText = senderCommandText
    var responseCode = 0;
    $.ajax({
        type: "POST",
        url: "/" + basePage + "?handler=Send&commandtext=" + commandText,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == 1) {
                responseCode = 1;
            } else if (response == 0) {
                responseCode = 0;
            } else if (response == 100) {
                responseCode = 100;
            } else if (response == 101) {
                responseCode = 101;
            } else if (response == 103) {
                responseCode = 103;
            } else if (response == 104) {
                responseCode = 104;
            } else {
                responseCode = 1000;
                commandText = response;
            }
            connection.invoke("SendMessage", responseCode, commandText).catch(function (err) {
                return console.error(err.toString());
            });
        },
        failure: function (response) {
            alert(response);
        }
    });
}

//Get Real-Time Light Status From The Server with SignalR
// And Send The Result To the Clients
connection.on("ReceiveMessage", function (result, command) {
    var responseString = "";
    switch (result) {
        case 0:
            responseString = command + "<span class='text-danger'> (item already exists)</span>";
            break;
        case 1:
        case 103:
        case 104:
            $('.btn-light').off('click');
            $.ajax({
                type: "GET",
                url: "/Index?handler=List",
                contentType: "application/json",
                dataType: "json",
                success: function (response) {
                    var $tblItems = $("#tblLights > tbody");
                    $tblItems.empty();

                    $.each(response, function (i, item) {
                        //table in index page
                        $tblItems.append("<tr>");
                        $tblItems.append("<td>" + response[i].id + "</td>");
                        $tblItems.append("<td>" + response[i].description + "</td>");
                        $tblItems.append("<td>" + response[i].code + "</td>");
                        if (response[i].status == true) {
                            $tblItems.append(`<td><button 
                                                data-item-commandtext='` + "set" + response[i].code + (response[i].status ? "00" : "01") + `' 
                                                data-item-id='` + response[i].id + `' 
                                                type='button' class='btn-no-style btn-light'>
                                                &nbsp;</button>
                                          </td>`);
                        } else {
                            $tblItems.append(`<td><button 
                                                data-item-commandtext='` + "set" + response[i].code + (response[i].status ? "00" : "01") + `' 
                                                data-item-id='` + response[i].id + `' 
                                                type='button' class='btn-no-style btn-no-style-off btn-light'>
                                                &nbsp;</button>
                                          </td>`);
                        }
                    });
                    $('.btn-light').click(btnLightClick);
                    //*******************************//
                    responseString = command + "<span class='text-success'> (command successfully executed)</span>";
                },
                failure: function (response) {
                    alert(response);
                }
            });
            responseString = command + "<span class='text-success'> (command successfully executed)</span>";
            break;
        case 100:
            responseString = command + "<span class='text-danger'> (unknown command type | Use <b><span class='text-info'>set/get/del/add</span></b> commands)</span>";
            break;
        case 101:
            responseString = command + "<span class='text-danger'> (couldn't find the object)</span>";
            break;
        case 1000:
            responseString = command.substr(0, 7) + "<span class='bg-dark text-white'>" + command.substr(command.length - 2, 2) + "</span><span class='text-success'> (command successfully executed)</span>";
            break;
        default:
            responseString = command + "<span class='text-success'> (something went wrong)</span>";
            break;
    }
    //console screen
    $console.append("<div class='console-text'><span class='text-danger'><b>[SERVER] :</b> </span>" + responseString + "</div>");
    $console.scrollTop($($console)[0].scrollHeight);
});

$('#deleteConfirmModal').on('shown.bs.modal', function (e) {
    $triggerBtn = $(e.relatedTarget);
    $('#hddnLightId').val($triggerBtn.data('item-id'));
    $('#spnLightDescription').html($triggerBtn.data('item-description'));
});