// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Moment DOM is loaded, chart div is referenced and populated with a new ChartJS object
document.addEventListener("DOMContentLoaded", function () {
    const ctx = document.getElementById('myChart');
    if (!ctx || !window.pollChartData) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: pollChartData.labels,
            datasets: [{
                label: 'Votes per Option',
                data: pollChartData.data,
                backgroundColor: ['#ff5454', '#ffe354', '#54a4ff'],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
});
