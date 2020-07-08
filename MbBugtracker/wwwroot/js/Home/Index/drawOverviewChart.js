function drawOverviewChart(myTickets) {
    let allTickets = myTickets;

    let closedTickets = allTickets.filter(t => t.ticketStatusId == 2).length;
    let activeTickets = allTickets.filter(t => t.ticketStatusId != 2).length;

    let context = document.getElementById("doughnutChart").getContext("2d");

    var myChart = new Chart(context, {
        type: "doughnut",
        data: {
            labels: ["Closed", "Active"],
            datasets: [{
                data: [closedTickets, activeTickets],
                backgroundColor: [
                    'rgba(66, 245, 164, 0.8)',
                    'rgba(245, 66, 78, 0.8)'
                ]
            }]
        }
    });
}