﻿let currentSelectListValue = $("#TicketStatus_Id").val();

function updateTicketStatus(ticketId) {
    let selectList = $(event.target);
    var ticketStatusId = selectList.val();  // newly selected value

    if (currentSelectListValue !== ticketStatusId) {

        let updateData = {
            ticketId,
            ticketStatusId
        }

        $.ajax({
            url: "/api/tickets/updatestatus",
            method: "PUT",
            data: JSON.stringify(updateData),
            contentType: "application/json"
        })
            .done((data) => {
                toastr.success("Updated ticket status!");

                let className = getStyleClassBasedOnValue(ticketStatusId);
                replaceClassOnSelectListElement(selectList, className);

                currentSelectListValue = $("#TicketStatus_Id").val();
                $("#ticketStatusTableData").text(data.statusName);
            })
            .fail((er) => {
                toastr.error("An error has ocured!");
            })
    }

}

function getStyleClassBasedOnValue(ticketStatusId) {

    let classname;
    switch (ticketStatusId) {
        case "1":
            classname = "text-warning-custom";
            break;
        case "2":
            classname = "text-success";
            break;
        case "3":
            classname = "text-primary";
            break;
        case "4":
            classname = "text-info";
            break;
        case "5":
            classname = "text-danger";
            break;
    }

    return classname;
}

function replaceClassOnSelectListElement(selectList, className) {
    let classList = selectList.attr("class").split(/\s+/);

    classList.splice(classList.length - 1, 1); // remove last class name
    classList.push(className);

    selectList.removeClass();
    selectList.addClass(classList.join(" "));
}