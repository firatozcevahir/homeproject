
var connection = new signalR.HubConnectionBuilder().withUrl("/lighthub").withAutomaticReconnect().build();
var $console = $("#dvConsole");
var $txtCommand = $('#txtCommand');
var previousTxtCommand = [];
var previousTxtCommandIndex = 0;

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
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
    processPrevNextCommands();
    executeCommand($txtCommand.val(), 'Console');
    $txtCommand.val('');
}
function processPrevNextCommands() {
    if ($txtCommand.val() != "") {
        previousTxtCommand.push($txtCommand.val());
        previousTxtCommandIndex = previousTxtCommand.length;
    }
}

$txtCommand.keydown(function (e) {
    switch (e.which) {
        case 38: //up
            previousTxtCommandIndex -= 1;
            if (previousTxtCommandIndex <= 0) { previousTxtCommandIndex = 0; }
            $(this).val(previousTxtCommand[previousTxtCommandIndex]);
            break;
        case 40: // down
            previousTxtCommandIndex += 1;
            if (previousTxtCommandIndex >= previousTxtCommand.length) { previousTxtCommandIndex = previousTxtCommand.length - 1; }
            $(this).val(previousTxtCommand[previousTxtCommandIndex]);
            break;
        case 13: //enter
            processPrevNextCommands();
            executeCommand($(this).val(), 'Console');
            $txtCommand.val('');
            break;
        default: return;
    }
    e.preventDefault();
});
function clearConsole() {
    $($console).children().fadeOut(300).promise().then(function () {
        $($console).empty();
    });
}

function executeCommand(senderCommandText, basePage) {
    var commandText = senderCommandText;
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
            responseString = command + "<span class='text-danger'> (unknown command type or wrong format | Use <b><span class='text-info'>set/get/del/add</span></b> commands with correct format: (e.g. setXXXX01)</span>";
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