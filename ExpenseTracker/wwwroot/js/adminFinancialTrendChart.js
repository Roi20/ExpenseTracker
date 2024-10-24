

var Labels = financialTrendData.map(m => m.Month);
var averageIncome = financialTrendData.map(item => item.AverageIncome);
var averageExpense = financialTrendData.map(item => item.AverageExpense);

var ctx = document.getElementById('adminFinancialChart').getContext('2d');

var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: Labels,
        datasets: [
            {
                label: 'Average Income',
                data: averageIncome,
                borderColor: '#4CB140',
                backgroundColor: '#4CB140',
                borderWidth: 1,
                fill: false,
                lineTension: 0.4,
                pointRadius: 0,
                spanGaps: true
            },
            {
                label: 'Average Expense',
                data: averageExpense,
                borderColor: '#C9190B',
                backgroundColor: '#C9190B',
                borderWidth: 1,
                fill: false,
                lineTension: 0.4,
                pointRadius: 0,
                spanGaps: true

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
                position: 'bottom',
                labels: {
                    font: {
                        size: '10',
                       
                    },
                    usePointStyle: true,
                    pointStyle: 'star',
                    color: '#FFFFFF',

                },

            },
            title: {
                display: true,
                text: 'Financial Trends',
                color: '#FFFFFF',
                padding: {
                    bottom: 30,
                    top: 10
                },
                font: {
                    size: 18
                },
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
                },
                display: false,
                formatter: function (value) {
                    return formatter.format(value)
                },
            }
        },
        scales: {
            y: {

                beginAtZero: false,
                position: 'left', // Align y-axis labels on the left side
                ticks: {
                    padding: 5, // Add padding between ticks and labels
                    font: {
                        size: 10,  // Adjust the font size
                    },
                    color: '#FFFFFF'
                },
                grid: {

                    drawOnChartArea: true,
                    drawTicks: false,
                    display: true,
                    borderWidth: 1,
                    color: 'rgba(255, 255, 255, 0.1)'
                },
                border: {
                    dash: [1, 2]
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
                    drawTicks: false,
                    display: true,
                    color: 'rgba(255, 255, 255, 0.2)',

                }
            }
        },
    }
});