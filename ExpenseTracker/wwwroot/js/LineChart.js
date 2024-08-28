Chart.register(ChartDataLabels);


var ctx = document.getElementById('myLineChart').getContext('2d');

var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], // Replace with your actual labels (e.g., months)
        datasets: [
            {
                label: 'Income',
                data: [500, 700, 800, 600, 1000, 1200, 1500, 200, 500, 1000, 2000, 750], // Replace with your actual income data
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderWidth: 2,
                fill: false, // Fill the area under the line
                lineTension: 0.3
            },
            {
                label: 'Expense',
                data: [400, 500, 700, 800, 900, 1100, 1300, 200, 500, 10, 200, 3000], // Replace with your actual expense data
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderWidth: 2,
                fill: false, // Fill the area under the line
                lineTension: 0.3
            }
        ]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        layout: {
            padding: {
                left: 0,
                right: 0,
                bottom: 0,
                top: 0
            }
        },
        plugins: {
            legend: {
                position: 'top', // Position the legend
                labels: {
                    font: {
                        size: '10'
                    }
                },
            },
            title: {
                display: true,
                text: 'Income vs Expense'
            },

            datalabels: {
                color: '#FFFFFF',
                align: 'end',  // Aligns the label towards the end of the data point
                anchor: 'end', // Anchors the label to the end of the data point
                font: {
                    size: 8,  // Adjust the font size to align better
                },
                padding: {
                    top: 0,
                    bottom: 0
                }
            }
        },
        scales: {
            y: {
                beginAtZero: true,
                position: 'left', // Align y-axis labels on the left side
                ticks: {
                    padding: 15, // Add padding between ticks and labels
                    font: {
                        size: 10,  // Adjust the font size
                    },
                    color: '#FFFFFF'
                },
                grid: {

                    drawOnChartArea: false,
                    drawTicks: true,
                    display: false
                }
            },
            x: {
                position: 'bottom', // Align x-axis labels at the bottom
                ticks: {
                    padding: 10, // Add padding between ticks and labels
                    font: {
                        size: 10  // Adjust the font size
                    },
                    maxRotation: 0,  // Prevent label rotation
                    minRotation: 0,  // Ensure labels are horizontal
                                   
                    color: '#FFFFFF'
                },
                grid: {
                    drawOnChartArea: false,
                    drawTicks: true,
                    display: false,
                   
                }
            }
        },
    }
});