
function createTable() {

    ticketTable = $("#ticketsTable").DataTable({
        ajax: {
            "url": "/api/tickets",
            "dataSrc": ""
        },
        aaSorting: [],
        columns: [
            {
                "data": "title",
                "render": (data, type, ticket) => {
                    let ticketResolution = ticket.ticketResolution;
                    let resolved = "";

                    if (ticketResolution !== null) {
                        resolved = "<span class='text-success ticketResolved' title='Resolved!'><i class='fas fa-check-circle'></i></span>";
                    }

                    let userId = $("#userId").data("userid");
                    let detailsHref = "/Tickets/Details/" + ticket.id;
                    let infoCircle = '<a href=' + detailsHref + '><i class="fa fa-info-circle" aria-hidden="true" title="Details"></i></a>';
                    
                    if (userId == ticket.applicationUser.id && ticket.ticketResolution == null) {
                        let editHref = "/Tickets/Edit/" + ticket.id;
                        return resolved + "&nbsp;" +  "<a" + " href=" + editHref + ">" + ticket.title + "&nbsp;" + infoCircle + "</a>";
                    } else {
                        return resolved + "&nbsp;" + "<a>" + ticket.title + "&nbsp" + infoCircle + "</a>";
                    }

                }
            },
            { "data": "project.projectName" },
            { "data": "applicationUser.userName" },
            { "data": "ticketPriority.priorityName" },
            { "data": "ticketStatus.statusName" },
            { "data": "ticketType.typeName" },
            {
                "data": "createdOn",
                "render": (data) => {
                    let date = new Date(data);
                    let formated = moment(date).format("DD.MM.YYYY");
                    let dateForSort = moment(date).format("YYYYMMDD");
                    return "<span class='hiddenDate'>" + dateForSort + "</span>" + formated;
                }
            },
            {
                "data": "dueDate",
                "render": (data) => {
                    let date = new Date(data);
                    let formated = moment(date).format("DD.MM.YYYY");
                    let dateForSort = moment(date).format("YYYYMMDD");
                    return "<span class='hiddenDate'>" + dateForSort + "</span>" + formated;
                }
            },
            { "data": "assignedTo" }
        ],
        "columnDefs": [
            { "width": "200px", "targets": 0 }
        ],
        rowGroup: {
            dataSrc: "project.projectName"
        }
    });
}
