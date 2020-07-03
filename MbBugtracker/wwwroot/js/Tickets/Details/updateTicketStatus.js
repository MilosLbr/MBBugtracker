let currentSelectListValue = $("#TicketStatus_Id").val();

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
                $("#ticketStatusTableData").text(data.ticketStatusDto.statusName);

                prependActivityLogList(data.ticketActivityLogDto);
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

function prependActivityLogList(activityLogDto) {
    let blockquote = $("<blockquote></blockquote>");
    let bcParagraphContent = $("<p></p>");
    let userNameStrongElem = $("<strong></strong>");
    let bcFooter = $("<footer></footer>");

    blockquote.addClass("blockquote p-2 ml-4");
    bcFooter.addClass("blockquote-footer");

    userNameStrongElem.text(activityLogDto.applicationUser.userName);
    let description = " has: " + activityLogDto.activityDescription;

    bcParagraphContent.append(userNameStrongElem, description);

    bcFooter.text(moment(activityLogDto.activityDate).format("DD.MM.YYYY. HH:mm:ss"));

    blockquote.append(bcParagraphContent, bcFooter);

    $("#nav-activity").prepend(blockquote);
}